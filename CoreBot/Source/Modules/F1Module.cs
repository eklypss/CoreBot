﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBot.Services;
using CoreBot.Settings;
using Discord.Commands;
using Humanizer;
using Serilog;

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

        [Command("schedule"), Summary("Returns the F1 schedule for the given season")]
        public async Task GetUrbanQuoteAsync(int season = -1)
        {
            if (season == -1) season = DateTime.Now.Year; // Use current year if no year is specified
            if (season <= 1950 || season > DateTime.Now.Year)
            {
                Log.Error($"Schedule not found for the given season: {season}.");
                await ReplyAsync("Invalid season.");
                return;
            }
            var schedule = await _f1Service.GetRaceSchedule(season);
            var raceList = new List<string>();

            foreach (var race in schedule.RaceData.RaceTable.Races)
            {
                var raceDate = race.Date.Date + race.Time.TimeOfDay;
                var remainder = raceDate.Subtract(DateTime.Now);
                if (remainder.TotalSeconds >= 0) raceList.Add($"[{race.Round}] **{race.Circuit.CircuitName}** *({race.Circuit.Location.Locality}, {race.Circuit.Location.Country})* in {remainder.Humanize(maxUnit: BotSettings.Instance.HumanizerMaxUnit, precision: BotSettings.Instance.HumanizerPrecision)}.");
                else raceList.Add($"[{race.Round}] **{race.Circuit.CircuitName}** *({race.Circuit.Location.Locality}, {race.Circuit.Location.Country})* {remainder.Humanize(maxUnit: BotSettings.Instance.HumanizerMaxUnit, precision: BotSettings.Instance.HumanizerPrecision)} ago.");
            }
            await ReplyAsync(string.Join(Environment.NewLine, raceList));
        }
    }
}
