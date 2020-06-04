FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster

COPY CoreBot/CoreBot.csproj /app/CoreBot/CoreBot.csproj
COPY CoreBot.Test/CoreBot.Test.csproj /app/CoreBot.Test/CoreBot.Test.csproj
COPY CoreBot.sln /app/CoreBot.sln
WORKDIR /app

RUN dotnet restore
COPY CoreBot /app/CoreBot
COPY CoreBot.Test /app/CoreBot.Test


CMD dotnet watch --project CoreBot run --config BotSettings.json
