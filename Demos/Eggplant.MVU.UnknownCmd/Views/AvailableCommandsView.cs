namespace Eggplant.MVU.UnknownCmd.Views
{
    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record AvailableCommandsView : BaseMenuView<CommandTypes>
    {
        public AvailableCommandsView(MenuBuilder<CommandTypes> menuBuilder)
        {
            InitialMenu = menuBuilder
                          .Row()
                          .Build("Have no available bot commands.");
            Menu = InitialMenu;
        }

        /// <inheritdoc />
        public sealed override Menu<CommandTypes> InitialMenu { get; }

        /// <inheritdoc />
        public override IView<CommandTypes> Update(IElement sourceMenu)
        {
            var view = this with
            {
                Menu = sourceMenu
            };

            return view;
        }
    }
}