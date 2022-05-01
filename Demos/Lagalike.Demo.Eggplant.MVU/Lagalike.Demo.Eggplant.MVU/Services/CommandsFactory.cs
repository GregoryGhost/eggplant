namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System.Collections.Generic;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    /// <summary>
    /// The demo commands factory.
    /// </summary>
    public class CommandsFactory
    {
        private static readonly IReadOnlyDictionary<CommandTypes, ICommand<CommandTypes>> Commands =
            new Dictionary<CommandTypes, ICommand<CommandTypes>>
            {
                { CommandTypes.ShareCockSize, new ShareCockSizeCommand() },
                { CommandTypes.GroupRating, new GroupRatingCommand() }
            };

        /// <summary>
        /// Get available commands of the demo.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<CommandTypes, ICommand<CommandTypes>> GetCommands()
        {
            return Commands;
        }

        /// <summary>
        /// Menu command.
        /// </summary>
        public ICommand<CommandTypes> ShareCockSizeCommand => Commands[CommandTypes.ShareCockSize];

        public GroupRatingCommand GetGroupRatingCommand(string groupId)
        {
            var cmd = (GroupRatingCommand)Commands[CommandTypes.GroupRating];

            var groupRatingCmd = cmd with
            {
                GroupId = new GroupId
                {
                    Value = groupId
                }
            };

            return groupRatingCmd;
        }
    }
}