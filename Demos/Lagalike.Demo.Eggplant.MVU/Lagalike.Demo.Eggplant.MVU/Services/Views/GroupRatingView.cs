namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using Lagalike.Demo.Eggplant.MVU.Commands;

    using PatrickStar.MVU;

    public record GroupRatingView : BaseMenuView<CommandTypes>
    {
        public GroupRatingView(CommandsFactory commandsFactory, MenuBuilder<CommandTypes> menuBuilder)
        {
            InitialMenu = menuBuilder
                           .Row()
                           .Build("It's no a group or members to size their cocks.");
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