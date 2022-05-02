namespace Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU
{
    using global::PatrickStar.MVU;

    using global::Telegram.Bot.Types;

    /// <summary>
    /// A Telegram update for the Patrick Star.
    /// </summary>
    public abstract record TelegramUpdate : IUpdate
    {
        /// <inheritdoc />
        public string ChatId { get; init; } = null!;

        /// <inheritdoc />
        public virtual bool IsSendingCachedModel { get; init; } = false;

        /// <summary>
        /// A Telegram update.
        /// </summary>
        public Update Update { get; init; } = null!;

        /// <summary>
        /// A request type of a Telegram update.
        /// </summary>
        public virtual RequestTypes RequestType { get; init; }
    }

    /// <summary>
    /// A Telegram edited message update type.
    /// </summary>
    public record TelegramEditedMsgUpdate : TelegramUpdate
    {
        /// <inheritdoc />
        public override RequestTypes RequestType => RequestTypes.EditedMessage;
    }
    
    /// <summary>
    /// A Telegram callback data update type.
    /// </summary>
    public record TelegramCallbackDataUpdate : TelegramUpdate
    {
        /// <inheritdoc />
        public override RequestTypes RequestType => RequestTypes.CallbackData;
    }
    
    /// <summary>
    /// A Telegram inline query update type.
    /// </summary>
    public record TelegramInlineQueryUpdate : TelegramUpdate
    {
        /// <inheritdoc />
        public override RequestTypes RequestType => RequestTypes.InlineQuery;

        /// <inheritdoc />
        public override bool IsSendingCachedModel { get; init; } = true;
    }
    
    /// <summary>
    /// A Telegram message update type.
    /// </summary>
    public record TelegramMsgUpdate : TelegramUpdate
    {
        /// <inheritdoc />
        public override RequestTypes RequestType => RequestTypes.Message;
        
        /// <inheritdoc />
        public override bool IsSendingCachedModel { get; init; } = true;
    }
}