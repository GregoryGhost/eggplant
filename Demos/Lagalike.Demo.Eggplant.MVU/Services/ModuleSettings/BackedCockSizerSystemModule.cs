namespace Lagalike.Demo.Eggplant.MVU.Services.ModuleSettings
{
    /// <inheritdoc />
    public class BackedCockSizerSystemModule : IBackedModeSystem
    {
        /// <summary>
        ///     Initialize dependencies.
        /// </summary>
        public BackedCockSizerSystemModule()
        {
            Startup = new Startup();
        }

        /// <summary>
        ///     Startup the demo with configured services.
        /// </summary>
        public IStartup Startup { get; }
    }
}