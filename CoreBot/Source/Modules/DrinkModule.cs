using System.Threading.Tasks;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class DrinkModule : ModuleBase
    {
        private readonly DrinkManager drinkManager;

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