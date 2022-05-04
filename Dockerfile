FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Core/Eggplant.Telegram/Eggplant.Telegram.csproj", "Core/Eggplant.Telegram/"]
COPY ["Core/Lagalike.Telegram.Shared/Lagalike.Telegram.Shared.csproj", "Core/Lagalike.Telegram.Shared/"]

COPY ["CustomPackages/PatrickStar.MVU/PatrickStar.MVU.csproj", "CustomPackages/PatrickStar.MVU/"]
COPY ["CustomPackages/GroupRating/GroupRating.csproj", "CustomPackages/GroupRating/"]
COPY ["CustomPackages/CockSizer/CockSizer.csproj", "CustomPackages/CockSizer/"]

COPY ["Demos/Lagalike.Demo.Eggplant.MVU/Lagalike.Demo.Eggplant.MVU.csproj", "Demos/Lagalike.Demo.Eggplant.MVU/"]
COPY ["Demos/Eggplant.MVU.GroupRating/Eggplant.MVU.GroupRating.csproj", "Demos/Eggplant.MVU.GroupRating/"]
COPY ["Demos/Eggplant.MVU.MessageWithoutAnyCmd/Eggplant.MVU.MessageWithoutAnyCmd.csproj", "Demos/Eggplant.MVU.MessageWithoutAnyCmd/"]
COPY ["Demos/Eggplant.MVU.ShareCockSize/Eggplant.MVU.ShareCockSize.csproj", "Demos/Eggplant.MVU.ShareCockSize/"]
COPY ["Demos/Eggplant.MVU.UnknownCmd/Eggplant.MVU.UnknownCmd.csproj", "Demos/Eggplant.MVU.UnknownCmd/"]
COPY ["Demos/Eggplant.Types.Shared/Eggplant.Types.Shared.csproj", "Demos/Eggplant.Types.Shared/"]

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
