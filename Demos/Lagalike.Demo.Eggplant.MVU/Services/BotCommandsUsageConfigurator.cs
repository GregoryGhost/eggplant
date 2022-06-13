namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using global::Eggplant.Types.Shared;

    public record BotCommandInfo
    {
        public string CommandName { get; init; } = null!;

        public CommandTypes Type { get; init; }
    }

    public class BotCommandsUsageConfigurator
    {
        private readonly string _availableBotUsage;

        private readonly IEnumerable<BotCommand> _botCommands;

        private readonly IEnumerable<BotCommandInfo> _botCommanInfos;

        public BotCommandsUsageConfigurator(CommandsFactory commandsFactory)
        {
            _botCommanInfos = commandsFactory.GetMessageCommands()
                                             .Select(
                                                 x => new BotCommandInfo
                                                 {
                                                     CommandName = $"/{commandsFactory.GetCommandName(x)}",
                                                     Type = x
                                                 });

            _botCommands = commandsFactory.GetMessageCommands()
                                          .Select(
                                              x => new BotCommand
                                              {
                                                  Command = commandsFactory.GetCommandName(x),
                                                  Description = commandsFactory.GetDescription(x)
                                              });

            var availableCmds = _botCommands.ToList().Select(x => $"/{x.Command} - {x.Description}");
            var formatCmds = string.Join("\n", availableCmds);
            _availableBotUsage = $"Usage:\n{formatCmds}";
        }

        public IEnumerable<BotCommand> GetAvailableBotCommands()
        {
            return _botCommands;
        }

        public IEnumerable<BotCommandInfo> GetBotCommandInfos()
        {
            return _botCommanInfos;
        }

        public string GetBotUsage()
        {
            return _availableBotUsage;
        }
    }
}