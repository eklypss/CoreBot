﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using CoreBot.Interfaces;
using CoreBot.Settings;
using Serilog;

namespace CoreBot.Services
{
    /// <summary>
    /// Get a random example text from Kotus old literary finnish website.
    /// </summary>
    public class QuoteService : IQuoteService
    {
        private readonly HtmlParser _parser;
        private readonly Random _random;

        public QuoteService()
        {
            _parser = new HtmlParser();
            _random = new Random();
        }

        public async Task<string> GetQuoteAsync(string searchTerm)
        {
            var payload = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("keyword", searchTerm),
                new KeyValuePair<string, string>("type", "2")
            });

            var cookies = new CookieContainer();
            using (var handler = new HttpClientHandler { CookieContainer = cookies })
            {
                using (var client = new HttpClient(handler))
                {
                    try
                    {
                        var pagingResponse = await client.PostAsync(DefaultValues.VKS_PAGING, payload);
                        var bodyStream = await pagingResponse.Content.ReadAsStreamAsync();
                        return await StartQuoteFetchAsync(bodyStream, cookies);
                    }
                    catch (Exception e)
                    {
                        Log.Error("Quote fail: " + e.ToString());
                        return "500";
                    }
                }
            }
        }

        public async Task<string> StartQuoteFetchAsync(Stream body, CookieContainer cookies)
        {
            var soup = await _parser.ParseAsync(body);

            var pagingLinks = soup.QuerySelectorAll("div.paging-item");
            int linkCount = pagingLinks.Length;
            int pageCount = linkCount == 0 ? 1 : int.Parse(pagingLinks[linkCount - 1].TextContent);

            // Shuffle page numbers for iteration
            var pageOrder = Enumerable.Range(1, pageCount).OrderBy(_ => _random.Next());

            string quote = await pageOrder.Select(async page =>
            {
                Log.Information("Downloading page...");
                var wordLinks = await FetchLinksFromPageAsync(page, cookies);
                return await SelectQuoteAsync(wordLinks, cookies);
            }).FirstOrDefault(q => q != null);

            return quote ?? "no matches";
        }

        /// <summary>
        /// VKS uses pagination with 15 links per page. Fetch all valid links from one page.
        /// </summary>
        /// <param name="pageNumber">paging number which should be selected by random.</param>
        /// <param name="cookies">cookies for specific search term</param>
        /// <returns>
        /// Task for valid link suffixes to VKS word pages. Empty enumerable if all invalid.
        /// </returns>
        public async Task<IEnumerable<string>> FetchLinksFromPageAsync(int pageNumber, CookieContainer cookies)
        {
            string url = $"{DefaultValues.VKS_URL}?p=searchresults&page={pageNumber}";
            var soup = await CookieFetchAsync(url, cookies);

            var linkElements = soup.QuerySelectorAll("td.search-item-word");

            // Ignore links which have right arrow indicating a reference word. They are just a link
            // to the main word.
            return linkElements
                .Where(link => !link.TextContent.Contains("→"))
                .Select(link => link.QuerySelector("a").GetAttribute("href"));
        }

        /// <summary>
        /// Download one page of words in parallel. Using the same HttpClient instance will raise
        /// "TaskCanceledException" on linux so use a workaround.
        /// </summary>
        /// <param name="wordLinks">0..15 link suffixes to word pages</param>
        /// <param name="cookies"></param>
        /// <returns>null if enumerable is empty</returns>
        public async Task<string> SelectQuoteAsync(IEnumerable<string> wordLinks, CookieContainer cookies)
        {
            var timer = Stopwatch.StartNew();
            int fail = 0;
            var soupTasks = wordLinks.Select(async link =>
            {
                try
                {
                    return await CookieFetchAsync(DefaultValues.VKS_URL + link, cookies);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    fail++;
                    return null;
                }
            });

            var rawSoups = await Task.WhenAll(soupTasks);
            var soups = rawSoups.Where(s => s != null);

            timer.Stop();
            Log.Debug($"Fetched {soups.Count()} pages in {timer.ElapsedMilliseconds} ms, + {fail} failed");
            if (!soups.Any())
            {
                return null;
            }

            return RandomQuote(soups);
        }

        /// <summary>
        /// Pick one random example phrase
        /// </summary>
        /// <param name="soups">pages representing one word at VKS</param>
        /// <returns>cleaned quote phrase, or null on no quotes</returns>
        public string RandomQuote(IEnumerable<IHtmlDocument> soups)
        {
            var quotes = soups
                .SelectMany(soup => soup.QuerySelectorAll("li.esimerkki span.esimteksti"))
                .Select(RemoveBookLink)
                .Select(element => element.TextContent.Replace("\n", " "))
                .ToList();

            if (quotes.Count == 0)
            {
                return null;
            }
            return quotes[_random.Next(quotes.Count)];
        }

        public IElement RemoveBookLink(IElement e)
        {
            IElement spanElement;
            while ((spanElement = e.QuerySelector("span")) != null)
            {
                e.RemoveChild(spanElement);
            }

            return e;
        }

        // Workaround for "TaskCanceledException" on linux
        public async Task<IHtmlDocument> CookieFetchAsync(string url, CookieContainer cookies)
        {
            var handler = new HttpClientHandler { CookieContainer = cookies };
            using (var client = new HttpClient(handler))
            {
                var stream = await client.GetStreamAsync(url);
                return await _parser.ParseAsync(stream);
            }
        }
    }
}
