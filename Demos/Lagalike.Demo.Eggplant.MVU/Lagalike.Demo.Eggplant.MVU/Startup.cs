namespace Lagalike.Demo.Eggplant.MVU
{
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
            services.AddStaffServices()
                    .AddCockSizeServices()
                    .AddGroupRatingServices()
                    .AddAvalableCommandsServices()
                    .AddMessageWithoutAnyCmdServices();
        }
    }

    internal static class ServiceRegistration
    {
        public static IServiceCollection AddAvalableCommandsServices(this IServiceCollection services)
        {
            return services.AddSingleton<AvailableCommandsViewMapper>()
                           .AddSingleton<AvailableCommandsView>()
                           .AddSingleton<BotCommandsUsageConfigurator>()
                           .AddHostedService<TelegramBotCommandsRegistrator>();
        }

        public static IServiceCollection AddCockSizeServices(this IServiceCollection services)
        {
            var gammaDistribution = new Gamma(6, 4);

            return services.AddSingleton<UserCockSizeInfoView>()
                           .AddSingleton<PersonCockSizeViewMapper>()
                           .AddSingleton<CockSizeFactory>()
                           .AddSingleton<InlineQueryMenuBuilder>()
                           .AddSingleton(gammaDistribution)
                           .AddSingleton<CockSizerUpdater>()
                           .AddSingleton<CockSizerInfo>()
                           .AddSingleton<CockSizerCache>()
                           .AddSingleton<EmotionBotReactionsHandler>();
        }

        public static IServiceCollection AddGroupRatingServices(this IServiceCollection services)
        {
            return services
                   .AddSingleton<GroupRatingViewMapper>()
                   .AddSingleton<GroupRatingView>()
                   .AddSingleton<GroupRatingHandler>()
                   .AddSingleton<GroupRatingStore>();
        }

        public static IServiceCollection AddMessageWithoutAnyCmdServices(this IServiceCollection services)
        {
            return services.AddSingleton<MessageWithoutAnyCmdViewMapper>()
                           .AddSingleton<MessageWithoutAnyCmdView>();
        }

        public static IServiceCollection AddStaffServices(this IServiceCollection services)
        {
            return services.AddSingleton<BackedCockSizerSystemModule>()
                           .AddSingleton<HandleUpdateService>()
                           .AddSingleton<DataFlowManager>()
                           .AddSingleton<ViewsFactory>()
                           .AddSingleton<ViewMapper>()
                           .AddSingleton<CockSizerPostProccessor>()
                           .AddSingleton<BotPostProccessor>()
                           .AddSingleton<CommandsFactory>()
                           .AddSingleton<MenuBuilder<CommandTypes>>();
        }
    }
}