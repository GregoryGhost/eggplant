namespace Eggplant.MVU.GroupRating.Views
{
    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record GroupRatingView : BaseMenuView<CommandTypes>
    {
        public GroupRatingView(MenuBuilder<CommandTypes> menuBuilder)
        {
            InitialMenu = menuBuilder
                           .Row()
                           .Build("It's no a group or members to size their cocks.");
            Menu = InitialMenu;
        }

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