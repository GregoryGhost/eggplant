namespace Lagalike.Demo.Eggplant.MVU
{
    using System;

    using Lagalike.Demo.Eggplant.MVU.Services;
    using Lagalike.Demo.Eggplant.MVU.Services.Domen;
    using Lagalike.Demo.Eggplant.MVU.Services.ModuleSettings;
    using Lagalike.Demo.Eggplant.MVU.Services.Views;
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.DependencyInjection;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class Startup : IStartup
    {
        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<BackedCockSizerSystemModule>()
                    .AddSingleton<HandleUpdateService>()
                    .AddSingleton<DataFlowManager>()
                    .AddSingleton<ViewsFactory>()
                    .AddSingleton<MenuView>()
                    .AddSingleton<InlineQueryMenuBuilder>()
                    .AddSingleton<CockSizeFactory>()
                    .AddSingleton<ViewMapper>()
                    .AddSingleton(new Random())
                    .AddSingleton<DefaultViewMapper>()
                    .AddSingleton<CockSizerUpdater>()
                    .AddSingleton<CockSizerInfo>()
                    .AddSingleton<CockSizerCache>()
                    .AddSingleton<EmotionBotReactionsHandler>()
                    .AddSingleton<CockSizerPostProccessor>()
                    .AddSingleton<CommandsFactory>();
        }
    }
}