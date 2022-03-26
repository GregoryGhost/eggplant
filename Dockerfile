FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Core/Eggplant.Telegram/Eggplant.Telegram.csproj", "Core/Eggplant.Telegram/"]
COPY ["Core/Lagalike.Telegram.Shared/Lagalike.Telegram.Shared.csproj", "Core/Lagalike.Telegram.Shared/"]

COPY ["Packages/PatrickStar.MVU/PatrickStar.MVU.csproj", "Packages/PatrickStar.MVU/"]

COPY ["Demos/Lagalike.Demo.Eggplant.MVU/Lagalike.Demo.Eggplant.MVU/Lagalike.Demo.Eggplant.MVU.csproj", "Demos/Lagalike.Demo.Eggplant.MVU/"]

RUN dotnet restore "Core/Eggplant.Telegram/Eggplant.Telegram.csproj"
COPY . .
WORKDIR "/src/Core/Eggplant.Telegram"
RUN dotnet build "Eggplant.Telegram.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Eggplant.Telegram.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Eggplant.Telegram.dll"]
