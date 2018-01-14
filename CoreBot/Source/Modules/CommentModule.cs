using System.Threading.Tasks;
using CoreBot.Services;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class CommentModule : ModuleBase
    {
        private readonly CommentService _commentService;

        public CommentModule(CommentService commentService)
        {
            _commentService = commentService;
        }

        [Command("kommentti"), Summary("Returns random Ilta-Sanomat comment")]
        [Alias("comment", "iscomment", "iltasanomat")]
        public async Task RandomIsCommentAsync()
        {
            var embed = await _commentService.RandomCommentEmbed();
            await Context.Channel.SendMessageAsync("", embed: embed);

        }
    }
}
