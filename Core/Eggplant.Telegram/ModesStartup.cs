namespace Eggplant.Telegram
{
    using Lagalike.Demo.Eggplant.MVU.Services.ModuleSettings;
    using Lagalike.Telegram.Shared.Contracts;

    /// <summary>
    ///     Startup of all bot modes (demos).
    /// </summary>
    public static class ModesStartup
    {
        /// <summary>
        ///     Add demo modules to DI.
        /// </summary>
        /// <param name="services">DI.</param>
        /// <returns>Returns DI.</returns>
        public static IServiceCollection AddDemoModules(this IServiceCollection services)
        {
            services.AddModule<BackedCockSizerSystemModule>();

            return services;
        }

        /// <summary>
        ///     Add a demo system module.
        /// </summary>
        /// <param name="services">The host service collection.</param>
        /// <typeparam name="TBackedModeSystem">Kekw</typeparam>
        /// <returns>Updated service collection.</returns>
        private static IServiceCollection AddModule<TBackedModeSystem>(this IServiceCollection services)
            where TBackedModeSystem : IBackedModeSystem, new()
        {
            var backedMode = new TBackedModeSystem();
            backedMode.Startup.ConfigureServices(services);

            return services;
        }
    }
}