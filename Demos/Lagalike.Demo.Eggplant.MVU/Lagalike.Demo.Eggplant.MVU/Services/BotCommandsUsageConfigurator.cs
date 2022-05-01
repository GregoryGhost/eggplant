namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using global::Telegram.Bot.Types;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Telegram.Shared.Contracts;
    
    public class BotCommandsUsageConfigurator
    {
        private readonly string _availableBotUsage;

        private readonly IEnumerable<BotCommand> _botCommands;

        public BotCommandsUsageConfigurator()
        {
            _botCommands = new BotCommand[]
            {
                new ()
                {
                    Command = "group_rating",//CommandTypes.GroupRating.ToString(),
                    Description = "get group rating"
                },
            };
            
            var availableCmds = _botCommands.ToList().Select(x => $"/{x.Command} - {x.Description}");
            var formatCmds = string.Join("\n", availableCmds);
            _availableBotUsage = $"Usage:\n{formatCmds}";
        }
        
        public string GetBotUsage()
        {
            return _availableBotUsage;
        }

        public IEnumerable<BotCommand> GetAvailableBotCommands()
        {
            return _botCommands;
        }
    }
}