namespace Eggplant.MVU.MessageWithoutAnyCmd.Views
{
    using Eggplant.Types.Shared;

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