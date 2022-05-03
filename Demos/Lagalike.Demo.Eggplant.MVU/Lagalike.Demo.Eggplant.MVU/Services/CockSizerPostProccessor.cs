namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System.Threading.Tasks;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Services.Views;
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