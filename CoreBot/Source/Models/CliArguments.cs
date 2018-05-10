using NClap.Metadata;

namespace CoreBot.Models
{
    public class CliArguments
    {
        [NamedArgument(
            ArgumentFlags.Required,
            LongName = "config",
            Description = "path to configuration json")]
        public string ConfigPath { get; set; }
    }
}
