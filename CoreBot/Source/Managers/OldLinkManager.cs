using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreBot.Helpers;
using CoreBot.Models;
using CoreBot.Settings;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Serilog;
using ServiceStack.OrmLite;

namespace CoreBot.Managers
{
    public class OldLinkManager
    {
        private readonly Regex _urlParser;

        public OldLinkManager()
        {
            // taken from https://stackoverflow.com/a/10576770
            _urlParser = new Regex(@"\b(?:https?://|www\.)\S+\b",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        private bool FilterBlacklisted(UriBuilder uri)
        {
            string uriHost = uri.Host.ToLower();
            string[] blacklist = BotSettings.Instance.OldLinkBlacklist;

            if (blacklist == null)
                return true;

            // try to filter out "www.example.com" and "example.com"
            return !blacklist.Any(blacklistUrl => uriHost.EndsWith(blacklistUrl, StringComparison.CurrentCultureIgnoreCase));
        }

        private IEnumerable<string> Normalize(string message)
        {
            return _urlParser.Matches(message)
                .OfType<Match>()
                .Select(m => new UriBuilder(m.Value))  // www.google.com -> http://www.google.com/
                .Where(FilterBlacklisted)
                .Select(uri => uri.Uri.ToString())
                .Distinct();
        }

        public async Task ReplyToLinks(IEnumerable<string> urls, SocketUserMessage message)
        {
            using (var conn = Database.Open())
            {
                foreach (string url in urls)
                {
                    var originalLink = await conn.SingleAsync<Link>(l => l.Url == url);
                    if (originalLink == null)
                    {
                        Log.Information($"saving link \"{url}\"");
                        await conn.InsertAsync(new Link(url, message.Author.Id));
                    }
                    else
                    {
                        await SendMessage(message, originalLink);
                    }
                }
            }
        }

        private async Task SendMessage(SocketMessage message, Link originalLink)
        {
            string ago = (DateTime.Now - originalLink.Timestamp).Humanize();
            var users = await message.Channel.GetUsersAsync(CacheMode.AllowDownload).Flatten();
            string msg = $"Old link <:ewPalm:256415457622360064> sent {ago} ago";

            var originalSender = (SocketGuildUser)users
                .FirstOrDefault(user => user.Id == originalLink.AuthorId);

            if (originalSender != null)
            {
                // ???
                string sender = string.IsNullOrWhiteSpace(originalSender.Nickname) ?
                    originalSender.Username : originalSender.Nickname;

                msg += " by " + sender;
            }
            await message.Channel.SendMessageAsync(msg);
        }

        public async Task Check(SocketUserMessage message)
        {
            var normalizedUrls = Normalize(message.Content);
            if (!normalizedUrls.Any())
                return;

            await ReplyToLinks(normalizedUrls, message);
        }
    }
}