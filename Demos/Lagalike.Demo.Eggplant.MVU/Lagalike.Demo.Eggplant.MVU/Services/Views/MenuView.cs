namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using System;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;
    using Lagalike.Demo.Eggplant.MVU.Services.Domen;

    using PatrickStar.MVU;

    /// <summary>
    ///     A demo menu view.
    /// </summary>
    public record MenuView : BaseMenuView<CommandTypes>
    {
        private readonly EmotionBotReactionsHandler _emotionBotReactionsHandler;

        private readonly InlineQueryButton _initialShareCockSizeButton = new()
        {
            Id = $"InitialMenuId_{Guid.NewGuid()}",
            Value = "I don't know your cock size.",
            Label = "Share your cock size"
        };

        public MenuView(EmotionBotReactionsHandler emotionBotReactionsHandler, InlineQueryMenuBuilder inlineQueryMenuBuilder)
        {
            _emotionBotReactionsHandler = emotionBotReactionsHandler;
            InitialMenu = inlineQueryMenuBuilder
                          .Button(_initialShareCockSizeButton)
                          .Build();
            Menu = InitialMenu;
        }

        /// <inheritdoc />
        public override InlineQueryMenu InitialMenu { get; }

        /// <inheritdoc />
        public override IView<CommandTypes> Update(IElement sourceMenu)
        {
            var view = this with
            {
                Menu = sourceMenu
            };

            return view;
        }

        /// <summary>
        ///     Update the view menu.
        /// </summary>
        /// <param name="model">A cock size model.</param>
        /// <returns>Return a updated view.</returns>
        public IView<CommandTypes> UpdateMenu(Model model)
        {
            if (model.CockSize is null)
                return Update(InitialMenu);

            var botEmoution = _emotionBotReactionsHandler.GetBotEmoution(model.CockSize);
            var shareButton = _initialShareCockSizeButton with
            {
                Id = Guid.NewGuid().ToString(),
                Value = $"Your cock size is {model.CockSize.Size} cm {botEmoution.Reaction}"
            };
            var updatedMenu = InitialMenu with
            {
                Buttons = new []
                {
                    shareButton
                }
            };

            return Update(updatedMenu);
        }
    }
}