﻿using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Enum;
using CoreBot.Helpers;
using CoreBot.Models;
using Discord.WebSocket;

namespace CoreBot.Handlers
{
    internal class MessageHandler
    {
        private DiscordSocketClient client;

        public Task Install(DiscordSocketClient discordClient)
        {
            client = discordClient;
            client.MessageReceived += HandleMessageAsync;
            return Task.CompletedTask;
        }

        private async Task HandleMessageAsync(SocketMessage message)
        {
            bool matchFound = false;
            foreach (var msg in UserMessages.Instance.Messages)
            {
                if (msg.User == message.Author.Username)
                {
                    msg.Message = message.Content;
                    msg.DateTime = message.CreatedAt.DateTime;
                    matchFound = true;
                    await FileHelper.SaveFile(FileType.MessagesFile);
                    break;
                }
            }
            if (!matchFound)
            {
                UserMessages.Instance.Messages.Add(new UserMessage(message.Author.Username, message.Content));
                await FileHelper.SaveFile(FileType.MessagesFile);
            }
        }
    }
}