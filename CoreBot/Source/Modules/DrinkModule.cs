using System.Threading.Tasks;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class DrinkModule : ModuleBase
    {
        private readonly DrinkManager _drinkManager;

        public DrinkModule(DrinkManager drinkManager)
        {
            _drinkManager = drinkManager;
        }

        [Command("juoma"), Summary("Returns a random drink from Alko website.")]
        public async Task RandomDrink()
        {
            string link = _drinkManager.RandomLink();
            await ReplyAsync(link);
        }
    }
}