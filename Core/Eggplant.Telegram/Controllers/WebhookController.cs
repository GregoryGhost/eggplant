namespace Eggplant.Telegram.Controllers
{
    using System.Reflection;
    using System.Threading.Tasks;

    using Eggplant.Telegram.Services;

    using global::Telegram.Bot.Types;

    using Microsoft.AspNetCore.Mvc;

    public class WebhookController : ControllerBase
    {
        private const string DEFAULT_APP_VERSION = "0.0.0";

        [HttpGet("/version")]
        public Task<IActionResult> AppVersion()
        {
            var appVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? DEFAULT_APP_VERSION;
            return Task.FromResult<IActionResult>(Ok(appVersion));
        }

        [HttpGet("/")]
        [HttpGet("/health-check")]
        public Task<IActionResult> HealthCheck()
        {
            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpPost("/bot")]
        public async Task<IActionResult> Post([FromServices] TelegramHandleUpdateService telegramHandleUpdateService,
            [FromBody] Update update)
        {
            await telegramHandleUpdateService.HandleUpdateAsync(update);
            return Ok();
        }
    }
}