namespace Eggplant.MVU.ShareCockSize.Views
{
    using Eggplant.MVU.ShareCockSize.Models;
    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    /// <summary>
    ///     A default view mapper which shows actual demo model.
    /// </summary>
    public class PersonCockSizeViewMapper : IViewMapper<CommandTypes>
    {
        private readonly UserCockSizeInfoView _userCockSizeInfoView;

        /// <summary>
        ///     Initial dependencies.
        /// </summary>
        /// <param name="userCockSizeInfoView">A demo menu view.</param>
        public PersonCockSizeViewMapper(UserCockSizeInfoView userCockSizeInfoView)
        {
            _userCockSizeInfoView = userCockSizeInfoView;
        }

        /// <inheritdoc />
        public IView<CommandTypes> Map(IModel model)
        {
            var defaultModel = (PersonCockSizeModel)model;
            var menu = _userCockSizeInfoView.UpdateMenu(defaultModel);

            return menu;
        }
    }
}