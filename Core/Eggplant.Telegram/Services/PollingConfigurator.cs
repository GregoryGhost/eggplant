namespace Eggplant.Telegram.Services
{
    using Lagalike.Telegram.Shared.Services;

    public class PollingConfigurator : IHostedService
    {
        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly ILogger<PollingConfigurator> _logger;

        private readonly ReceiverOptions _receiverOptions;

        private readonly CancellationTokenSource _telegramClientCancellationToken;

        private readonly PollingUpdateHandler _updateHandler;

        public PollingConfigurator(ILogger<PollingConfigurator> logger, ConfiguredTelegramBotClient botClient,
            PollingUpdateHandler updateHandler)
        {
            _logger = logger;
            _botClient = botClient;
            _updateHandler = updateHandler;
            _telegramClientCancellationToken = new CancellationTokenSource();

            var receiveAllUpdateTypes = Array.Empty<UpdateType>();
            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = receiveAllUpdateTypes
            };
        }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RemoveWebhookAsync(cancellationToken);

            await StartReceiveTelegramMessangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _telegramClientCancellationToken.Cancel();
            return Task.CompletedTask;
        }

        private async Task RemoveWebhookAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing webhook");
            await _botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }

        private async Task StartReceiveTelegramMessangesAsync(CancellationToken cancellationToken)
        {
            var me = await _botClient.GetMeAsync(cancellationToken);

            _botClient.StartReceiving(
                _updateHandler,
                _receiverOptions,
                cancellationToken
            );

            _logger.LogInformation($"Start listening for @{me.Username}");
        }
    }
}