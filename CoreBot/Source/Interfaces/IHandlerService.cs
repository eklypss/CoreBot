﻿using System.Threading.Tasks;
using CoreBot.Handlers;

namespace CoreBot.Interfaces
{
    public interface IHandlerService
    {
        CommandHandler CommandHandler { get; set; }
        LogHandler LogHandler { get; set; }

        Task CreateHandlers();
    }
}