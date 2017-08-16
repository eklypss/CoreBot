using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBot.Models;
using Discord.WebSocket;

namespace CoreBot.Interfaces
{
    public interface IOldLinkService
    {
        bool FilterBlacklisted(UriBuilder uri);

        IEnumerable<string> Normalize(string message);

        Task ReplyToLinksAsync(IEnumerable<string> urls, SocketMessage message);

        Task SendMessageAsync(SocketMessage message, Link originalLink);
    }
}