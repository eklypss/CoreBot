using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;

namespace CoreBot.Interfaces
{
    public interface IQuoteService
    {
        Task<string> Quote(string searchTerm);

        Task<string> StartQuoteFetch(Stream body, CookieContainer cookies);

        Task<IEnumerable<string>> FetchLinksFromPage(int pageNumber, CookieContainer cookies);

        Task<string> SelectQuote(IEnumerable<string> wordLinks, CookieContainer cookies);

        string RandomQuote(IEnumerable<IHtmlDocument> soups);

        IElement RemoveBookLink(IElement e);

        Task<IHtmlDocument> CookieFetch(string url, CookieContainer cookies);
    }
}