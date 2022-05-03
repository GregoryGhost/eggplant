namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    internal record CommandTypeInfo
    {
        public ICommand<CommandTypes> Command { get; init; } = null!;

        public CommandUsageInModes CommandUsageInMode { get; init; }

        public string Description { get; init; } = null!;
    }

    internal enum CommandUsageInModes
    {
        InlineMode,

        MessageMode,

        CallbackMode,

        /// <summary>
        ///     It's a service command type for proccess commands in MVU.
        /// </summary>
        ServiceMode
    }

    /// <summary>
    ///     The demo commands factory.
    /// </summary>
    public class CommandsFactory
    {
        private static readonly IReadOnlyDictionary<CommandTypes, CommandTypeInfo> Commands =
            new Dictionary<CommandTypes, CommandTypeInfo>
            {
                {
                    CommandTypes.ShareCockSize, new CommandTypeInfo
                    {
                        Command = new ShareCockSizeCommand(),
                        CommandUsageInMode = CommandUsageInModes.InlineMode,
                        Description = "Size a user cock."
                    }
                },
                {
                    CommandTypes.GroupRating, new CommandTypeInfo
                    {
                        Command = new GroupRatingCommand(),
                        CommandUsageInMode = CommandUsageInModes.MessageMode,
                        Description = "Get the group rating users and their cock sizes."
                    }
                },
                {
                    CommandTypes.UnknownCommand, new CommandTypeInfo
                    {
                        Command = new UnknownCommand(),
                        CommandUsageInMode = CommandUsageInModes.ServiceMode,
                        Description = "It's a service command type for proccess commands in MVU"
                    }
                }
            };

        private readonly IReadOnlyCollection<CommandTypes> _messageCommands;

        public CommandsFactory()
        {
            _messageCommands = new[]
            {
                CommandTypes.GroupRating
            };
        }

        /// <summary>
        ///     Menu command.
        /// </summary>
        public ICommand<CommandTypes> ShareCockSizeCommand => Commands[CommandTypes.ShareCockSize].Command;

        public string GetCommandName(CommandTypes command)
        {
            var text = command.ToString();
            if (text.Length < 2)
                return text;

            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(text[0]));
            for (var i = 1; i < text.Length; ++i)
            {
                var c = text[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }

            var commandName = sb.ToString();

            return commandName;
        }

        /// <summary>
        ///     Get available commands of the demo.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<CommandTypes, ICommand<CommandTypes>> GetCommands()
        {
            return Commands.ToDictionary(x => x.Key, x => x.Value.Command);
        }

        public string GetDescription(CommandTypes command)
        {
            return Commands[command].Description;
        }

        public ICommand<CommandTypes> GetGroupRatingCommand(string groupId)
        {
            var cmd = (GroupRatingCommand) Commands[CommandTypes.GroupRating].Command;

            var groupRatingCmd = cmd with
            {
                GroupId = new GroupId
                {
                    Value = groupId
                }
            };

            return groupRatingCmd;
        }

        public IEnumerable<CommandTypes> GetMessageCommands()
        {
            return _messageCommands;
        }

        public ICommand<CommandTypes> GetUnknownCommand()
        {
            return Commands[CommandTypes.UnknownCommand].Command;
        }
    }
}