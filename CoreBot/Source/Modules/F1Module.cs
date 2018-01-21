using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBot.Services;
using CoreBot.Settings;
using Discord.Commands;
using Humanizer;

namespace CoreBot.Modules
{
    [Group("f1")]
    public class F1Module : ModuleBase
    {
        private readonly F1Service _f1Service;

        public F1Module(F1Service f1Service)
        {
            _f1Service = f1Service;
        }

        [Command("schedule")]
        public async Task GetUrbanQuoteAsync(int season)
        {
            try
            {
                var schedule = await _f1Service.GetRaceSchedule(season);
                var raceList = new List<string>();

                foreach (var race in schedule.RaceData.RaceTable.Races)
                {
                    var raceDate = race.Date.Date + race.Time.TimeOfDay;
                    var remainder = raceDate.Subtract(DateTime.Now);
                    if (remainder.TotalSeconds >= 0) raceList.Add($"[{race.Round}] **{race.Circuit.CircuitName}** *({race.Circuit.Location.Locality}, {race.Circuit.Location.Country})* in {remainder.Humanize(BotSettings.Instance.HumanizerPrecision)}.");
                    else raceList.Add($"[{race.Round}] **{race.Circuit.CircuitName}** *({race.Circuit.Location.Locality}, {race.Circuit.Location.Country})* {remainder.Humanize(BotSettings.Instance.HumanizerPrecision)} ago.");
                }
                await ReplyAsync(string.Join(Environment.NewLine, raceList));
            }
            catch (Exception ex)
            {
                await ReplyAsync($"Failed to get schedule: {ex.Message}");
            }
        }
    }
}
