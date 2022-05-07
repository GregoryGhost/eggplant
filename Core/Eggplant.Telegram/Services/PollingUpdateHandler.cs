namespace Eggplant.Telegram.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using global::Telegram.Bot;
    using global::Telegram.Bot.Extensions.Polling;
    using global::Telegram.Bot.Types;

    /// <summary>
    ///     Wrapper default update handler for polling mode.
    /// </summary>
    public class PollingUpdateHandler : IUpdateHandler
    {
        private readonly TelegramHandleUpdateService _updateHandler;

        public PollingUpdateHandler(TelegramHandleUpdateService updateHandler)
        {
            _updateHandler = updateHandler;
        }

        /// <inheritdoc />
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await _updateHandler.HandleErrorAsync(exception);
        }

        /// <inheritdoc />
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await _updateHandler.HandleUpdateAsync(update);
        }
    }
}