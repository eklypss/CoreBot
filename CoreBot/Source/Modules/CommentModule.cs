using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using CoreBot.Database.Dao;
using CoreBot.Models;
using Discord;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class CommentModule : ModuleBase
    {

        private readonly int MAX_FIELD_LEN = 1024;

        private readonly CommentDao _commentDao;
        private readonly HtmlParser _htmlParser;
        private readonly EmbedBuilder _embedBuilder;

        // db query is slow so use preloaded value
        private IltasanomatComment _comment;

        public CommentModule()
        {
            _htmlParser = new HtmlParser();
            _commentDao = new CommentDao();
            _embedBuilder = new EmbedBuilder();
        }

        [Command("kommentti"), Summary("Returns random Ilta-Sanomat comment")]
        [Alias("comment", "iscomment", "iltasanomat")]
        public async Task RandomIsCommentAsync()
        {
            if (_comment == null) _comment = await _commentDao.RandomCommentAsync();

            var embed = _embedBuilder
                .AddField(_comment.Author, FormatHtmlMessage(_comment))
                .WithFooter($"+{_comment.Upvotes} 👍")
                .WithColor(Color.Purple)
                .Build();

            await Context.Channel.SendMessageAsync("", embed: embed);

            _comment = null;
            _comment = await _commentDao.RandomCommentAsync();
        }

        private string FormatHtmlMessage(IltasanomatComment comment)
        {
            var doc = _htmlParser.Parse(comment.Message).DocumentElement;

            // TODO: implement quoted comment support
            IElement blockquote;
            while ((blockquote = doc.QuerySelector("blockquote")) != null)
            {
                blockquote.Remove();
            }

            var message = doc.TextContent;

            if (message.Length > MAX_FIELD_LEN)
            {
                return message.Substring(0, MAX_FIELD_LEN - 3) + "...";
            }

            return message;
        }
    }
}
