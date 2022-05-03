namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    public class AvailableCommandsViewMapper: IViewMapper<CommandTypes>
    {
        private readonly AvailableCommandsView _view;

        public AvailableCommandsViewMapper(AvailableCommandsView view)
        {
            _view = view;
        }

        public IView<CommandTypes> Map(IModel model)
        {
            var sourceModel = (AvailableCommandsModel) model;
            var hasEmptyModel = string.IsNullOrEmpty(sourceModel.AvailableCommands);
            if (hasEmptyModel)
            {
                return _view.Update(_view.InitialMenu);
            }
            
            var menu = (Menu<CommandTypes>) _view.Menu;
            var msg = menu.MessageElement with
            {
                Text = sourceModel.AvailableCommands
            };
            var updatedMenu = menu with
            {
                MessageElement = msg
            };
            var updatedView = _view.Update(updatedMenu);

            return updatedView;
        }
    }
}