namespace Eggplant.MVU.CompareDudes.Views
{
    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record CompareDudesView : BaseMenuView<CommandTypes>
    {
        public CompareDudesView(MenuBuilder<CommandTypes> menuBuilder)
        {
            InitialMenu = menuBuilder
                          .Row()
                          .Build("Unknown dudes to compare their cocks.");
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