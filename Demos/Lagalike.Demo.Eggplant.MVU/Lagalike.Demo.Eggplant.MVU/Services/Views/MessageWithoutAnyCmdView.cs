namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using Lagalike.Demo.Eggplant.MVU.Commands;

    using PatrickStar.MVU;

    public record MessageWithoutAnyCmdView: BaseMenuView<CommandTypes>
    {
        public MessageWithoutAnyCmdView(MenuBuilder<CommandTypes> menuBuilder)
        {
            InitialMenu = menuBuilder
                          .Row()
                          .Build("Empty. Nothing.");
            Menu = InitialMenu;
        }

        public sealed override Menu<CommandTypes> InitialMenu { get; }

        /// <inheritdoc />
        public override IView<CommandTypes> Update(IElement sourceMenu)
        {
            return this;
        }
    }
}