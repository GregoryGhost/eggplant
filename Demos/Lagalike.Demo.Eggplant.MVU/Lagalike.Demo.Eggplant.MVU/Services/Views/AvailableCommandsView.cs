namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using Lagalike.Demo.Eggplant.MVU.Commands;

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

        public override Menu<CommandTypes> InitialMenu { get; }

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