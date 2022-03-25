namespace Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using global::PatrickStar.MVU;

    using global::Telegram.Bot.Types.InlineQueryResults;
    using global::Telegram.Bot.Types.ReplyMarkups;

    using Lagalike.Telegram.Shared.Services;

    using Newtonsoft.Json;

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

        /// <inheritdoc />
        public async Task ProccessAsync(IView<TCmdType> view, TelegramUpdate update)
        {
            if (update.RequestType is RequestTypes.InlineQuery)
            {
                var inlineQueryResult = GetInlineQueryResult(view);
                await _client.AnswerInlineQueryAsync(
                    update.Update.InlineQuery.Id,
                    inlineQueryResult);
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

        private static IReadOnlyCollection<InlineQueryResultBase> GetInlineQueryResult(IView<TCmdType> view)
        {
            var inlineMenu = (InlineQueryMenu)view.Menu;
            var results = inlineMenu.Buttons.Select(button => new InlineQueryResultArticle(
                                        button.Id,
                                        button.Label,
                                        new InputTextMessageContent(button.Value)))
                                    .ToArray();

            return results;
            // return new InlineQueryResultBase[]
            // {
            //     new InlineQueryResultArticle("kekw23444", "kekw23444", new InputTextMessageContent("kekw text241222"))
            // };
        }

        private async Task ProccessStatefullUpdate(IView<TCmdType> view, TelegramUpdate update)
        {
            var menu = (Menu<TCmdType>)view.Menu;
            var keyboard = GetInlineKeyboard(menu);

            if (update.RequestType is RequestTypes.CallbackData)
                await _client.EditMessageTextAsync(
                    update.ChatId,
                    update.Update.CallbackQuery.Message.MessageId,
                    menu.MessageElement.Text,
                    replyMarkup: keyboard);
            else if (update.RequestType is RequestTypes.Message or RequestTypes.EditedMessage)
                await _client.SendTextMessageAsync(update.ChatId, menu.MessageElement.Text, replyMarkup: keyboard);
            else
                throw new NotImplementedException($"Cannot proccess this action {update.RequestType}");
        }
    }
}