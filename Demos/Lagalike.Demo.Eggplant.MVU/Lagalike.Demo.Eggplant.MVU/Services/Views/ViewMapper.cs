namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class ViewMapper : BaseMainViewMapper<CommandTypes, ModelTypes>
    {
        /// <inheritdoc />
        public ViewMapper(ViewsFactory viewsFactory)
            : base(viewsFactory.GetViews())
        {
        }
    }
}