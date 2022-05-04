namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using System.Collections.Generic;

    using global::Eggplant.MVU.GroupRating.Views;
    using global::Eggplant.MVU.MessageWithoutAnyCmd.Views;
    using global::Eggplant.MVU.ShareCockSize.Views;
    using global::Eggplant.MVU.UnknownCmd.Views;
    using global::Eggplant.Types.Shared;

    using PatrickStar.MVU;

    /// <summary>
    ///     A factory of the all demo views.
    /// </summary>
    public class ViewsFactory
    {
        private readonly IDictionary<ModelTypes, IViewMapper<CommandTypes>> _views;

        /// <summary>
        ///     Initialize dependencies.
        /// </summary>
        /// <param name="personCockSizeViewMapper">A default view mapper of the demo.</param>
        public ViewsFactory(PersonCockSizeViewMapper personCockSizeViewMapper, GroupRatingViewMapper groupRatingViewMapper,
            AvailableCommandsViewMapper availableCommandsViewMapper,
            MessageWithoutAnyCmdViewMapper messageWithoutAnyCmdViewMapper)
        {
            _views = new Dictionary<ModelTypes, IViewMapper<CommandTypes>>
            {
                {ModelTypes.PersonCockSizeModel, personCockSizeViewMapper},
                {ModelTypes.GroupRatingModel, groupRatingViewMapper},
                {ModelTypes.AvailableCommandsModel, availableCommandsViewMapper},
                {ModelTypes.MessageWithoutAnyCmdModel, messageWithoutAnyCmdViewMapper}
            };
        }

        /// <summary>
        ///     Get available demo views.
        /// </summary>
        /// <returns>Returns available demo views.</returns>
        public IDictionary<ModelTypes, IViewMapper<CommandTypes>> GetViews()
        {
            return _views;
        }
    }
}