namespace Lagalike.Demo.Eggplant.MVU
{
    using CockSizer.Services;

    using DudesComparer.Services;

    using global::Eggplant.MVU.CompareDudes.Services;
    using global::Eggplant.MVU.CompareDudes.Views;
    using global::Eggplant.MVU.GroupRating.Services;
    using global::Eggplant.MVU.GroupRating.Views;
    using global::Eggplant.MVU.MessageWithoutAnyCmd.Views;
    using global::Eggplant.MVU.ShareCockSize.Views;
    using global::Eggplant.MVU.UnknownCmd.Views;
    using global::Eggplant.Types.Shared;

    using GroupRating.Services;

    using Lagalike.Demo.Eggplant.MVU.Services;
    using Lagalike.Demo.Eggplant.MVU.Services.ModuleSettings;
    using Lagalike.Demo.Eggplant.MVU.Services.Views;
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.DependencyInjection;

    using PatrickStar.MVU;

    using IGroupRatingCockSizerCache = GroupRating.Services.ICockSizerCache;
    using ICompareDudesCache = DudesComparer.Services.ICockSizerCache;

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
                    .AddMessageWithoutAnyCmdServices()
                    .AddCompareDudesServices();
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

        public static IServiceCollection AddCompareDudesServices(this IServiceCollection services)
        {
            return services.AddSingleton<IDudesHandler, DudesHandler>()
                           .AddSingleton<IDudesComparerStore, DudesComparerStore>()
                           .AddSingleton<CompareDudesView>()
                           .AddSingleton<ICompareDudesCache>(x => x.GetRequiredService<CockSizerCache>())
                           .AddSingleton<CompareDudesViewMapper>();
        }

        public static IServiceCollection AddCockSizeServices(this IServiceCollection services)
        {
            return services.AddSingleton<UserCockSizeInfoView>()
                           .AddSingleton<PersonCockSizeViewMapper>()
                           .AddSingleton<CockSizeFactory>()
                           .AddSingleton<InlineQueryMenuBuilder>()
                           .AddSingleton<IDistribution, Distribution>()
                           .AddSingleton<CockSizerUpdater>()
                           .AddSingleton<CockSizerInfo>()
                           .AddSingleton<CockSizerCache>()
                           .AddSingleton<IGroupRatingCockSizerCache>(x => x.GetRequiredService<CockSizerCache>())
                           .AddSingleton<IModelCache>(x => x.GetRequiredService<CockSizerCache>())
                           .AddSingleton<EmotionBotReactionsHandler>();
        }

        public static IServiceCollection AddGroupRatingServices(this IServiceCollection services)
        {
            return services
                   .AddSingleton<GroupRatingViewMapper>()
                   .AddSingleton<GroupRatingView>()
                   .AddSingleton<IGroupRatingStore, GroupRatingStore>()
                   .AddSingleton<GroupRatingHandler>();
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