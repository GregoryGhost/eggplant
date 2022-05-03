namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using Lagalike.Demo.Eggplant.MVU.Commands;

    using PatrickStar.MVU;

    public class MessageWithoutAnyCmdViewMapper : IViewMapper<CommandTypes>
    {
        private readonly MessageWithoutAnyCmdView _view;

        public MessageWithoutAnyCmdViewMapper(MessageWithoutAnyCmdView view)
        {
            _view = view;
        }

        public IView<CommandTypes> Map(IModel model)
        {
            return _view;
        }
    }
}