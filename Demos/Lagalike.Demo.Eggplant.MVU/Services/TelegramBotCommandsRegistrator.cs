namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    using global::Telegram.Bot;

    using Lagalike.Telegram.Shared.Services;

    using Microsoft.Extensions.Hosting;

    public class TelegramBotCommandsRegistrator : IHostedService
    {
        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly BotCommandsUsageConfigurator _commandsConfigurator;

        public TelegramBotCommandsRegistrator(ConfiguredTelegramBotClient botClient, BotCommandsUsageConfigurator commandsConfigurator)
        {
            _botClient = botClient;
            _commandsConfigurator = commandsConfigurator;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RegistrateBotCommandsAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task RegistrateBotCommandsAsync()
        {
            var botCommands = _commandsConfigurator.GetAvailableBotCommands();
            await _botClient.SetMyCommandsAsync(botCommands);
        }
    }
}