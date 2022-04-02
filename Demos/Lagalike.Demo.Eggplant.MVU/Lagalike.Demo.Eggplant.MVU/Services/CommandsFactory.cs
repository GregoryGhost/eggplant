namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System.Collections.Generic;

    using Lagalike.Demo.Eggplant.MVU.Commands;

    using PatrickStar.MVU;

    /// <summary>
    /// The demo commands factory.
    /// </summary>
    public class CommandsFactory
    {
        private static readonly IReadOnlyDictionary<CommandTypes, ICommand<CommandTypes>> Commands =
            new Dictionary<CommandTypes, ICommand<CommandTypes>>
            {
                { CommandTypes.GetCockSize, new GetCockSizeCommand() },
                { CommandTypes.ShareCockSize, new ShareCockSizeCommand() }
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
        /// Decrement command.
        /// </summary>
        public ICommand<CommandTypes> GetCockSizeCommand => Commands[CommandTypes.GetCockSize];

        /// <summary>
        /// Menu command.
        /// </summary>
        public ICommand<CommandTypes> ShareCockSizeCommand => Commands[CommandTypes.ShareCockSize];
    }
}