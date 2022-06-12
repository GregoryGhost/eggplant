namespace Eggplant.MVU.ShareCockSize.Views
{
    using System;

    using CockSizer.Services;

    using Eggplant.MVU.ShareCockSize.Models;
    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    /// <summary>
    ///     A user cock size info view.
    /// </summary>
    public record UserCockSizeInfoView : BaseMenuView<CommandTypes>
    {
        private readonly EmotionBotReactionsHandler _emotionBotReactionsHandler;

        private readonly InlineQueryButton _initialShareCockSizeButton = new()
        {
            Id = $"InitialMenuId_{Guid.NewGuid()}",
            Value = "I don't know your cock size.",
            Label = "Share your cock size"
        };

        public UserCockSizeInfoView(EmotionBotReactionsHandler emotionBotReactionsHandler, InlineQueryMenuBuilder inlineQueryMenuBuilder)
        {
            _emotionBotReactionsHandler = emotionBotReactionsHandler;
            InitialMenu = inlineQueryMenuBuilder
                          .Button(_initialShareCockSizeButton)
                          .Build();
            Menu = InitialMenu;
        }

        /// <inheritdoc />
        public sealed override InlineQueryMenu InitialMenu { get; }

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
        ///     Update the view.
        /// </summary>
        /// <param name="model">A cock size root model.</param>
        /// <returns>Return a updated view.</returns>
        public IView<CommandTypes> UpdateMenu(PersonCockSizeModel model)
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