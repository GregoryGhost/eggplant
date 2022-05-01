namespace Lagalike.Demo.Eggplant.MVU
{
    using System;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Services;
    using Lagalike.Demo.Eggplant.MVU.Services.Domain;
    using Lagalike.Demo.Eggplant.MVU.Services.ModelUpdaters;
    using Lagalike.Demo.Eggplant.MVU.Services.ModuleSettings;
    using Lagalike.Demo.Eggplant.MVU.Services.Views;
    using Lagalike.Telegram.Shared.Contracts;

    using MathNet.Numerics.Distributions;

    using Microsoft.Extensions.DependencyInjection;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class Startup : IStartup
    {
        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services)
        {
            var gammaDistribution = new Gamma(6, 4);
            services.AddSingleton<BackedCockSizerSystemModule>()
                    .AddSingleton<HandleUpdateService>()
                    .AddSingleton<DataFlowManager>()
                    .AddSingleton<ViewsFactory>()
                    .AddSingleton<UserCockSizeInfoView>()
                    .AddSingleton<InlineQueryMenuBuilder>()
                    .AddSingleton<CockSizeFactory>()
                    .AddSingleton<ViewMapper>()
                    .AddSingleton(gammaDistribution)
                    .AddSingleton<PersonCockSizeViewMapper>()
                    .AddSingleton<CockSizerUpdater>()
                    .AddSingleton<CockSizerInfo>()
                    .AddSingleton<CockSizerCache>()
                    .AddSingleton<EmotionBotReactionsHandler>()
                    .AddSingleton<CockSizerPostProccessor>()
                    .AddSingleton<CommandsFactory>()
                    .AddSingleton<GroupRatingHandler>()
                    .AddSingleton<GroupRatingViewMapper>()
                    .AddSingleton<GroupRatingView>()
                    .AddSingleton<GroupRatingStore>()
                    .AddSingleton<BotCommandsUsageConfigurator>()
                    .AddHostedService<TelegramBotCommandsRegistrator>()
                    .AddSingleton<MenuBuilder<CommandTypes>>();
        }
    }
}