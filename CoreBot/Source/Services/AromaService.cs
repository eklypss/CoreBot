using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Database.Dao;
using MathNet.Numerics.Distributions;

namespace CoreBot.Services
{
    public class AromaService
    {
        private readonly AromaDao _aromaDao;

        public AromaService(AromaDao aromaDao)
        {
            _aromaDao = aromaDao;
        }

        public async Task<string> RandomAromas()
        {
            var aromas = await _aromaDao.RandomAromas(NextAromaCount());
            return string.Join(", ", aromas.Select(a => a.Name));
        }

        // Get the aroma count. 2-4 quite common, 6+ very rare. Presents half-normal distribution
        // with mean = 2, sd = 1.5
        private int NextAromaCount()
        {
            double i = Math.Abs(Normal.Sample(0, 1.5));
            i += 2;
            return (int)Math.Round(i);
        }
    }
}