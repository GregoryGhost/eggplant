namespace Eggplant.Telegram.Services
{
    using System;
    using System.Threading.Tasks;

    using global::Telegram.Bot.Exceptions;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;

    using Lagalike.Demo.Eggplant.MVU.Services;

    using Microsoft.Extensions.Logging;

    public class TelegramHandleUpdateService
    {
        private readonly ILogger<TelegramHandleUpdateService> _logger;

        private readonly HandleUpdateService _telegramHandleUpdateService;

        public TelegramHandleUpdateService(ILogger<TelegramHandleUpdateService> logger,
            HandleUpdateService handleUpdateService)
        {
            _logger = logger;
            _telegramHandleUpdateService = handleUpdateService;
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogCritical(errorMessage);

            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            try
            {
                await GetHandlerAsync(update);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        private async Task GetHandlerAsync(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message or UpdateType.EditedMessage or UpdateType.CallbackQuery or UpdateType.InlineQuery =>
                    HandleBotUpdateAsync(update),
                _ => UnknownUpdateHandlerAsync(update)
            };

            await handler;
        }

        private async Task HandleBotUpdateAsync(Update telegramUserUpdate)
        {
            await _telegramHandleUpdateService.HandleUpdateAsync(telegramUserUpdate);
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger.LogInformation($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
    }
}