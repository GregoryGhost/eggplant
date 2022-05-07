namespace Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU
{
    using System;
    using System.Threading.Tasks;

    using global::PatrickStar.MVU;

    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;

    /// <inheritdoc />
    public abstract class BaseMvuUpdateHandlerService<TModel, TViewMapper, TCommandType> : ITelegramUpdateHandler
        where TModel : IModel, IEquatable<TModel>
        where TViewMapper : IViewMapper<TCommandType>
        where TCommandType : Enum
    {
        private readonly IDataFlowManager<TModel, TViewMapper, TelegramUpdate, TCommandType> _dataFlowManager;

        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        /// <param name="dataFlowManager">A manager wich controls Telegram data flow.</param>
        protected BaseMvuUpdateHandlerService(IDataFlowManager<TModel, TViewMapper, TelegramUpdate, TCommandType>  dataFlowManager)
        {
            _dataFlowManager = dataFlowManager;
        }
        
        /// <inheritdoc />
        public async Task HandleUpdateAsync(Update update)
        {
            if (update.Type is UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery is null)
                    throw new ArgumentNullException(nameof(update.CallbackQuery));
                
                var callbackUpdate = new TelegramCallbackDataUpdate
                {
                    ChatId = update.CallbackQuery.From.Id.ToString(),
                    Update = update,
                };
                await _dataFlowManager.ProccessMessageAsync(callbackUpdate);
            }
            else if (update.Type is UpdateType.Message)
            {
                if (update.Message is null)
                    throw new ArgumentNullException(nameof(update.Message));
                
                var messageUpdate = new TelegramMsgUpdate
                {
                    ChatId = update.Message.Chat.Id.ToString(),
                    Update = update,
                };
                await _dataFlowManager.ProccessMessageAsync(messageUpdate);
            }
            else if (update.Type is UpdateType.EditedMessage)
            {
                if (update.EditedMessage is null)
                    throw new ArgumentNullException(nameof(update.EditedMessage));
                
                if (update.EditedMessage.From is null)
                    throw new ArgumentNullException(nameof(update.EditedMessage.From));
                
                var editedMsgUpdate = new TelegramEditedMsgUpdate
                {
                    ChatId = update.EditedMessage.From.Id.ToString(),
                    Update = update,
                };
                await _dataFlowManager.ProccessMessageAsync(editedMsgUpdate);
            }
            else if (update.Type is UpdateType.InlineQuery)
            {
                if (update.InlineQuery is null)
                    throw new ArgumentNullException(nameof(update.InlineQuery));
                
                var inlineQueryUpdate = new TelegramInlineQueryUpdate
                {
                     ChatId = update.InlineQuery.From.Id.ToString(),
                     Update = update,
                };
                await _dataFlowManager.ProccessMessageAsync(inlineQueryUpdate);
            }
            else
            {
                //Don't proccess other types of update because it doesn't matter
            }
        }
    }
}