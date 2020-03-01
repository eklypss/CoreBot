FROM microsoft/dotnet:2.0.7-sdk-2.1.200-stretch

COPY CoreBot/CoreBot.csproj /CoreBot/CoreBot.csproj
WORKDIR /CoreBot

RUN dotnet restore
COPY CoreBot /CoreBot
COPY CoreBot.Test /CoreBot.Test


CMD dotnet watch run --config BotSettings.json
