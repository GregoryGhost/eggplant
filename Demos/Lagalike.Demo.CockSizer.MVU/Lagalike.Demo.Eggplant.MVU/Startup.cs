namespace Lagalike.Demo.Eggplant.MVU
{
    using Lagalike.Demo.Eggplant.MVU.Services;
    using Lagalike.Demo.Eggplant.MVU.Services.ModuleSettings;
    using Lagalike.Demo.Eggplant.MVU.Services.Views;
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.DependencyInjection;

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
                    .AddSingleton<ViewMapper>()
                    .AddSingleton<DefaultViewMapper>()
                    .AddSingleton<CockSizerUpdater>()
                    .AddSingleton<CockSizerInfo>()
                    .AddSingleton<CockSizerCache>()
                    .AddSingleton<CockSizerPostProccessor>()
                    .AddSingleton<CommandsFactory>();
        }
    }
}