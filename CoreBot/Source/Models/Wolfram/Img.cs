using Newtonsoft.Json;

namespace CoreBot.Models.Wolfram
{
    public class Img
    {
        public string src { get; set; }
        public string alt { get; set; }
        public string title { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
