namespace Lagalike.Telegram.Shared.Contracts
{
    /// <summary>
    ///     Contract for a demo startup.
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        ///     Configure services for a demo.
        /// </summary>
        /// <param name="services">DI.</param>
        void ConfigureServices(IServiceCollection services);
    }
}