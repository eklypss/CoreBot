using Discord.Commands;
using System.Threading.Tasks;

namespace CoreBot.Source.Modules
{
    public class DrinkModule : ModuleBase
    {
        private DrinkManager drinkManager;

        public DrinkModule(DrinkManager drinkManager)
        {
            this.drinkManager = drinkManager;
        }

        [Command("juoma")]
        public async Task RandomDrink()
        {
            string link = drinkManager.RandomLink();
            await ReplyAsync(link);
        }
    }
}