namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using global::Eggplant.Types.Shared;

    using Lagalike.Demo.Eggplant.MVU.Models;
    using Lagalike.Demo.Eggplant.MVU.Services.Views;

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