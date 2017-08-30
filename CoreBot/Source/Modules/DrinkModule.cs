using System.Threading.Tasks;
using CoreBot.Database.Dao;
using Discord.Commands;

namespace CoreBot.Modules
{
    public class DrinkModule : ModuleBase
    {
        private readonly DrinkDao _drinkDao;

        public DrinkModule(DrinkDao drinkDao)
        {
            _drinkDao = drinkDao;
        }

        [Command("juoma"), Summary("Returns a random drink from Alko website.")]
        [Alias("drink")]
        public async Task GetRandomDrinkAsync()
        {
            string link = _drinkDao.RandomLink();
            await ReplyAsync(link);
        }
    }
}