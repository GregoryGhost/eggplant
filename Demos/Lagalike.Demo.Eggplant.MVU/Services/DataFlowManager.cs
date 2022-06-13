namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using global::Eggplant.Types.Shared;

    using Lagalike.Demo.Eggplant.MVU.Models;
    using Lagalike.Demo.Eggplant.MVU.Services.Views;

    /// <summary>
    ///     The demo data flow manager which controls Telegram.
    /// </summary>
    public class DataFlowManager : IDataFlowManager<Model, ViewMapper, TelegramUpdate, CommandTypes>
    {
        private readonly BotCommandsUsageConfigurator _botCommandUsageConfigurator;

        private readonly IReadOnlyDictionary<CommandTypes, ICommand<CommandTypes>> _commands;

        private readonly CommandsFactory _commandsFactory;

        /// <summary>
        ///     Initialize dependencies.
        /// </summary>
        /// <param name="model">The demo model.</param>
        /// <param name="postProccessor">The Telegram update post processor.</param>
        /// <param name="updater">The Telegram update handler.</param>
        /// <param name="viewMapper">The view mapper.</param>
        /// <param name="commandsFactory">The demo commands factory.</param>
        public DataFlowManager(IModelCache model, BotPostProccessor postProccessor, CockSizerUpdater updater,
            ViewMapper viewMapper, CommandsFactory commandsFactory, BotCommandsUsageConfigurator botCommandUsageConfigurator)
        {
            _commandsFactory = commandsFactory;
            _botCommandUsageConfigurator = botCommandUsageConfigurator;
            Model = model;
            PostProccessor = postProccessor;
            Updater = updater;
            ViewMapper = viewMapper;
            InitialModel = new Model
            {
                CockSizeModel = null,
                GroupRatingModel = null,
                AvailableCommandsModel = null,
                CompareDudesModel = null,
                CurrentCommand = CommandTypes.MessageWithoutAnyCmdCommand,
            };
            _commands = commandsFactory.GetCommands();
        }

        /// <inheritdoc />
        public Model InitialModel { get; init; }

        /// <inheritdoc />
        public IModelCache<Model> Model { get; init; }

        /// <inheritdoc />
        public IPostProccessor<CommandTypes, TelegramUpdate> PostProccessor { get; init; }

        /// <inheritdoc />
        public IUpdater<CommandTypes, Model> Updater { get; init; }

        /// <inheritdoc />
        public ViewMapper ViewMapper { get; init; }

        /// <inheritdoc />
        public ICommand<CommandTypes> GetInputCommand(TelegramUpdate update)
        {
            var commandType = update.RequestType switch
            {
                RequestTypes.Message or RequestTypes.EditedMessage =>
                    TryReadCommand(update),
                RequestTypes.CallbackData =>
                    JsonConvert.DeserializeObject<BaseCommand<CommandTypes>>(update.Update.CallbackQuery?.Data),
                RequestTypes.InlineQuery =>
                    _commandsFactory.ShareCockSizeCommand,
                _ => throw new ArgumentOutOfRangeException($"{nameof(update.RequestType)} has unknown value {update.RequestType}")
            };
            if (commandType?.Type == null)
                throw new NullReferenceException(
                    $"Command type is null, the source update data: {update.Update.CallbackQuery?.Data}");

            if (_commands.ContainsKey(commandType.Type))
            {
                commandType = PassParametersToCommand(commandType, update);

                return commandType;
            }

            throw new KeyNotFoundException($"Not found the command type {commandType.Type}");
        }

        private ICommand<CommandTypes> PassParametersToCommand(ICommand<CommandTypes> commandType, TelegramUpdate update)
        {
            var command = commandType.Type switch
            {
                CommandTypes.GroupRating => _commandsFactory.GetGroupRatingCommand(update.ChatId),
                CommandTypes.ShareCockSize => _commandsFactory.GetCockSizeCommand(update.ChatId),
                CommandTypes.CompareDudes => _commandsFactory.GetCompareDudesCommand(update.ChatId, update.Update.Message?.Text),
                _ => commandType
            };

            return command;
        }

        private ICommand<CommandTypes>? TryReadCommand(TelegramUpdate update)
        {
            var inputRawCmd = update.Update.Message?.Text;
            if (inputRawCmd is null)
                return null;

            var foundCmd = _botCommandUsageConfigurator.GetBotCommandInfos()
                                                       .SingleOrDefault(x => inputRawCmd.Contains(x.CommandName));

            var haveNoPassedCommand = foundCmd is null && !inputRawCmd.Contains("/");
            if (haveNoPassedCommand)
                return _commandsFactory.GetMessageWithoutAnyCmdCommand();

            if (foundCmd is null)
                return _commandsFactory.GetUnknownCommand();

            _commands.TryGetValue(foundCmd.Type, out var foundCmdByType);

            return foundCmdByType;
        }
    }
}