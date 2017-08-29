using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

namespace CoreBot.Modules
{
    [Group("emote"), Summary("Module for listing custom server emotes.")]
    public class EmoteModule : ModuleBase
    {
        [Command("find"), Summary("Lists all server emotes that contain the given string.")]
        public async Task GetEmotes(string searchTerm)
        {
            var emoteNames = Context.Guild.Emotes
                .Where(x => x.Name.Equals(searchTerm, StringComparison.InvariantCultureIgnoreCase))
                .Select(emote => $"<:{emote.Name}:{emote.Id}>");

            if (emoteNames.Any()) await ReplyAsync(string.Join(" ", emoteNames));
            else await ReplyAsync("No matches.");
        }

        [Command("list"), Summary("Lists all the server emotes.")]
        [Alias("all")]
        public async Task ListEmotes()
        {
            var emoteNames = new List<string>();
            foreach (var emote in Context.Guild.Emotes)
            {
                emoteNames.Add($"<:{emote.Name}:{emote.Id}>");
            }
            if (emoteNames.Count > 0) await ReplyAsync(string.Join(" ", emoteNames));
            else await ReplyAsync("This server has no custom emotes.");
        }
    }
}