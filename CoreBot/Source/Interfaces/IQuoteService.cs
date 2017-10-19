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
        Task<string> GetQuoteAsync(string searchTerm);

        Task<string> StartQuoteFetchAsync(Stream body, CookieContainer cookies);

        Task<IEnumerable<string>> FetchLinksFromPageAsync(int pageNumber, CookieContainer cookies);

        Task<string> SelectQuoteAsync(IEnumerable<string> wordLinks, CookieContainer cookies);

        string RandomQuote(IEnumerable<IHtmlDocument> soups);

        IElement RemoveBookLink(IElement e);

        Task<IHtmlDocument> CookieFetchAsync(string url, CookieContainer cookies);
    }
}
