namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using DudesComparer.Services;

    using global::Eggplant.MVU.CompareDudes.Commands;
    using global::Eggplant.MVU.GroupRating.Commands;
    using global::Eggplant.MVU.MessageWithoutAnyCmd.Commands;
    using global::Eggplant.MVU.ShareCockSize.Commands;
    using global::Eggplant.MVU.UnknownCmd.Commands;
    using global::Eggplant.Types.Shared;

    using GroupRating.Models;

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
                    CommandTypes.CompareDudes, new CommandTypeInfo
                    {
                        Command = new CompareDudesCommand(),
                        CommandUsageInMode = CommandUsageInModes.MessageMode,
                        Description = "Bulling between some dudes. That compares their cocks."
                    }
                },
                {
                    CommandTypes.UnknownCommand, new CommandTypeInfo
                    {
                        Command = new UnknownCommand(),
                        CommandUsageInMode = CommandUsageInModes.ServiceMode,
                        Description = "It's a service command type to proccess commands in MVU."
                    }
                },
                {
                    CommandTypes.MessageWithoutAnyCmdCommand, new CommandTypeInfo
                    {
                        Command = new MessageWithoutAnyCmdCommand(),
                        CommandUsageInMode = CommandUsageInModes.ServiceMode,
                        Description = "It's a service comand type which signalize about get user message without any commands."
                    }
                }
            };

        private readonly IReadOnlyCollection<CommandTypes> _messageCommands;

        public CommandsFactory()
        {
            _messageCommands = Commands.Values.Where(x => x.CommandUsageInMode == CommandUsageInModes.MessageMode)
                                       .Select(x => x.Command.Type)
                                       .ToArray();
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

        public ICommand<CommandTypes> GetMessageWithoutAnyCmdCommand()
        {
            return Commands[CommandTypes.MessageWithoutAnyCmdCommand].Command;
        }

        public ICommand<CommandTypes> GetCockSizeCommand(string chatId)
        {
            var cmd = (ShareCockSizeCommand) Commands[CommandTypes.ShareCockSize].Command;

            var cockSizeCommand = cmd with
            {
                ChatId = chatId
            };

            return cockSizeCommand;
        }

        public ICommand<CommandTypes> GetCompareDudesCommand(string chatId, string? msg)
        {
            var cmd = (CompareDudesCommand) Commands[CommandTypes.CompareDudes].Command;
            if (string.IsNullOrEmpty(msg))
            {
                return cmd with
                {
                    ComparingDudes = null
                };
            }

            var parsedComparedDudes = msg.Split(" ")
                                         .Where(x => x.StartsWith("@"))
                                         .Select(x => x.Replace("@", "").Trim())
                                         .ToArray();
            var parsedChatId = new DudesComparer.Models.ChatId(chatId);
            var cmdWithParams = cmd with
            {
                ComparingDudes = new ComparingDudes
                {
                    ChatId = parsedChatId,
                    DudesUserNames = parsedComparedDudes
                }
            };

            return cmdWithParams;
        }
    }
}