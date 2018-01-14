using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using CoreBot.Database.Dao;
using CoreBot.Models;
using Discord;

namespace CoreBot.Services
{
    public class CommentService
    {

        private readonly int MAX_FIELD_LEN = 1024;

        private readonly CommentDao _commentDao;
        private readonly HtmlParser _htmlParser;

        public CommentService(CommentDao commentDao)
        {
            _commentDao = commentDao;
            _htmlParser = new HtmlParser();
        }

        public static async Task <CommentService> Create()
        {
            var commentDao = await CommentDao.Create();
            return new CommentService(commentDao);
        }

        public async Task<Embed> RandomCommentEmbed()
        {
            var comment = await _commentDao.RandomCommentAsync();

            var embed = new EmbedBuilder()
                .AddField(comment.Author, FormatHtmlMessage(comment))
                .WithFooter($"+{comment.Upvotes} 👍")
                .WithColor(Color.Purple)
                .Build();

            return embed;
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
