namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using global::Eggplant.Types.Shared;
    
    using Lagalike.Demo.Eggplant.MVU.Models;
    using Lagalike.Demo.Eggplant.MVU.Services.Views;
    using Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU;

    /// <inheritdoc />
    public class HandleUpdateService : BaseMvuUpdateHandlerService<Model, ViewMapper, CommandTypes>
    {
        /// <inheritdoc />
        public HandleUpdateService(DataFlowManager dataFlowManager)
            : base(dataFlowManager)
        {
        }
    }
}