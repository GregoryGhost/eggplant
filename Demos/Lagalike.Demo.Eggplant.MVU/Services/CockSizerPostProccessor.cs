namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System.Threading.Tasks;

    using global::Eggplant.MVU.MessageWithoutAnyCmd.Views;
    using global::Eggplant.Types.Shared;

    using Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU;
    using Lagalike.Telegram.Shared.Services;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class CockSizerPostProccessor : TelegramPostProccessor<CommandTypes>
    {
        /// <inheritdoc />
        public CockSizerPostProccessor(ConfiguredTelegramBotClient client)
            : base(client)
        {
        }
    }

    public class BotPostProccessor : IPostProccessor<CommandTypes, TelegramUpdate>
    {
        private readonly CockSizerPostProccessor _postProccessor;

        public BotPostProccessor(CockSizerPostProccessor postProccessor)
        {
            _postProccessor = postProccessor;
        }

        public async Task ProccessAsync(IView<CommandTypes> view, TelegramUpdate update)
        {
            if (view is MessageWithoutAnyCmdView)
            {
                //NOTE: Nothing to do
                return;
            }

            await _postProccessor.ProccessAsync(view, update);
        }
    }
}