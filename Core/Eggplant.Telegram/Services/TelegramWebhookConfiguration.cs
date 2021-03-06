namespace Eggplant.Telegram.Services
{
    using Lagalike.Telegram.Shared;

    public class TelegramWebhookConfiguration
    {
        public TelegramWebhookConfiguration(IOptions<TelegramBotConfiguration> configuration)
        {
            WebhookAddress = $"{configuration.Value.HostAddress}/bot";
        }

        public string WebhookAddress { get; }
    }
}