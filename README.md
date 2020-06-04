# CoreBot

Cross-platform Discord bot built with .NET Core

### Requirements

* docker

### Usage

1. Copy `./CoreBot/BotSettings.json.example` to `./CoreBot/BotSettings.json`.
1. At minimum specify `BotToken` available from https://discordapp.com/developers/applications
1. Run `docker-compose up`

Run unit tests with `docker-compose exec corebot dotnet test`
