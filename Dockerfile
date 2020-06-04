FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster

COPY CoreBot/CoreBot.csproj /CoreBot/CoreBot.csproj
WORKDIR /CoreBot

RUN dotnet restore
COPY CoreBot /CoreBot
COPY CoreBot.Test /CoreBot.Test


CMD dotnet watch run --config BotSettings.json
