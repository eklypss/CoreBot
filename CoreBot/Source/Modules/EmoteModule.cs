using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

namespace CoreBot.Modules
{
    [Group("emote"), Summary("Module for listing custom server emotes.")]
    public class EmoteModule : ModuleBase
    {
        [Command("find")]
        public async Task GetEmotes(string searchTerm)
        {
            var emotes = Context.Guild.Emotes.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            var emoteNames = new List<string>();
            foreach (var emote in emotes)
            {
                emoteNames.Add($"<:{emote.Name}:{emote.Id}>");
            }
            if (emoteNames.Count > 0) await ReplyAsync(string.Join(" ", emoteNames));
            else await ReplyAsync("No matches.");
        }

        [Command("list")]
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