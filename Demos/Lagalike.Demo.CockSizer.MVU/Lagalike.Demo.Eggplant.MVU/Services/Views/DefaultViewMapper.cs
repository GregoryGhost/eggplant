namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    /// <summary>
    /// A default view mapper which shows actual demo model.
    /// </summary>
    public class DefaultViewMapper : IViewMapper<CommandTypes>
    {
        private readonly MenuView _menuView;

        /// <summary>
        /// Initial dependencies.
        /// </summary>
        /// <param name="menuView">A demo menu view.</param>
        public DefaultViewMapper(MenuView menuView)
        {
            _menuView = menuView;
        }

        /// <inheritdoc />
        public IView<CommandTypes> Map(IModel model)
        {
            var defaultModel = (Model)model;
            var menu = _menuView.UpdateMenu(defaultModel);

            return menu;
        }
    }
}