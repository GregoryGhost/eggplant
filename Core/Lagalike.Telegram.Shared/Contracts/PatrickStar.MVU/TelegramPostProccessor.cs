namespace Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU
{
    using global::PatrickStar.MVU;

    using Lagalike.Telegram.Shared.Services;

    /// <summary>
    ///     Telegram update processor.
    /// </summary>
    /// <typeparam name="TCmdType">A command type, which contains in views of Patrick Star MVU.</typeparam>
    public abstract class TelegramPostProccessor<TCmdType> : IPostProccessor<TCmdType, TelegramUpdate>
        where TCmdType : Enum
    {
        private readonly ConfiguredTelegramBotClient _client;

        /// <summary>
        ///     Initialize dependencies.
        /// </summary>
        /// <param name="client">Telegram client.</param>
        protected TelegramPostProccessor(ConfiguredTelegramBotClient client)
        {
            _client = client;
        }

        /// <exception cref="ArgumentNullException">Throws when an inline query id is empty.</exception>
        /// <inheritdoc />
        public async Task ProccessAsync(IView<TCmdType> view, TelegramUpdate update)
        {
            if (update.RequestType is RequestTypes.InlineQuery)
            {
                var inlineQueryResult = GetInlineQueryResult(view);

                if (update.Update.InlineQuery?.Id is null)
                    throw new ArgumentNullException(nameof(update.Update.InlineQuery.Id));

                await _client.AnswerInlineQueryAsync(
                    update.Update.InlineQuery.Id,
                    inlineQueryResult,
                    isPersonal: true);
            }
            else
            {
                await ProccessStatefullUpdate(view, update);
            }
        }

        private static InlineKeyboardMarkup GetInlineKeyboard(Menu<TCmdType> menu)
        {
            var inlineButtons = menu.Buttons.Select(
                                        buttons => buttons.Select(
                                            button =>
                                            {
                                                var serializedCommand = JsonConvert.SerializeObject(button.Cmd);
                                                return InlineKeyboardButton.WithCallbackData(
                                                    button.Label,
                                                    serializedCommand);
                                            })
                                    )
                                    .ToArray();
            var keyboard = new InlineKeyboardMarkup(inlineButtons);

            return keyboard;
        }

        private static IReadOnlyCollection<InlineQueryResult> GetInlineQueryResult(IView<TCmdType> view)
        {
            var inlineMenu = (InlineQueryMenu)view.Menu;
            var results = inlineMenu.Buttons.Select(
                                        button => new InlineQueryResultArticle(
                                            button.Id,
                                            button.Label,
                                            new InputTextMessageContent(button.Value)))
                                    .ToArray();

            return results;
        }

        private async Task ProccessStatefullUpdate(IView<TCmdType> view, TelegramUpdate update)
        {
            var menu = (Menu<TCmdType>)view.Menu;
            var keyboard = GetInlineKeyboard(menu);

            if (update.RequestType is RequestTypes.CallbackData)
            {
                var msgId = update.Update.CallbackQuery?.Message?.MessageId;
                if (msgId is null)
                    throw new ArgumentNullException(nameof(update.Update.CallbackQuery.Message.MessageId));

                await _client.EditMessageTextAsync(
                    update.ChatId,
                    msgId.Value,
                    menu.MessageElement.Text,
                    ParseMode.Html,
                    replyMarkup: keyboard);
            }
            else if (update.RequestType is RequestTypes.Message or RequestTypes.EditedMessage)
            {
                await _client.SendTextMessageAsync(
                    update.ChatId,
                    menu.MessageElement.Text,
                    ParseMode.Html,
                    replyMarkup: keyboard);
            }
            else
            {
                throw new NotImplementedException($"Cannot proccess this action {update.RequestType}");
            }
        }
    }
}