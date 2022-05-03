namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;
    using Lagalike.Demo.Eggplant.MVU.Services.Domain;
    using Lagalike.Demo.Eggplant.MVU.Services.ModelUpdaters;
    using Lagalike.Demo.Eggplant.MVU.Services.Views;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class CockSizerUpdater : IUpdater<CommandTypes, Model>
    {
        private readonly CockSizeFactory _cockSizeFactory;

        private readonly GroupRatingHandler _groupRatingHandler;

        private readonly BotCommandsUsageConfigurator _botCommandsUsageConfigurator;
        
        public CockSizerUpdater(CockSizeFactory cockSizeFactory, GroupRatingHandler groupRatingHandler, BotCommandsUsageConfigurator botCommandsUsageConfigurator)
        {
            _cockSizeFactory = cockSizeFactory;
            _groupRatingHandler = groupRatingHandler;
            _botCommandsUsageConfigurator = botCommandsUsageConfigurator;
        }
        
        /// <inheritdoc />
        public async Task<(ICommand<CommandTypes>? OutputCommand, Model UpdatedModel)> UpdateAsync(ICommand<CommandTypes> command,
            Model model)
        {
            var updatedModel = command.Type switch
            {
                CommandTypes.ShareCockSize => RandomCockSize(model),
                CommandTypes.GroupRating => await GetGroupRatingAsync(model, (GroupRatingCommand)command),
                CommandTypes.UnknownCommand => GetAvailableBotCommandModel(model, (UnknownCommand)command),
                _ => throw new ArgumentOutOfRangeException($"Unknown {nameof(command)}: {command}")
            };
            ICommand<CommandTypes> emptyCmd = null!;

            return (emptyCmd, updatedModel);
        }

        private Model GetAvailableBotCommandModel(Model model, UnknownCommand command)
        {
            model = model with
            {
                CurrentCommand = command.Type,
            };

            if (model.AvailableCommandsModel is not null)
                return model;
            
            
            var msgCommands = new AvailableCommandsModel
            {
                AvailableCommands = _botCommandsUsageConfigurator.GetBotUsage()
            };

            var updatedModel = model with
            {
                AvailableCommandsModel = msgCommands
            };

            return updatedModel;

        }

        private async Task<Model> GetGroupRatingAsync(Model model, GroupRatingCommand cmd)
        {
            model = model with
            {
                CurrentCommand = cmd.Type
            };

            var groupRating = await _groupRatingHandler.GetRatingAsync(cmd.GroupId);
            var initialized = model with
            {
                GroupRatingModel = new GroupRatingModel
                {
                    GroupRating = groupRating,
                }
            };

            return initialized;
        }

        private Model RandomCockSize(Model model)
        {
            model = model with
            {
                CurrentCommand = CommandTypes.ShareCockSize
            };
            if (model.CockSizeModel?.CockSize is not null)
                return model;
            
            var initialized = model with
            {
                CockSizeModel = new PersonCockSizeModel
                {
                    CockSize = _cockSizeFactory.GetRandomCockSize()
                }
            };

            return initialized;
        }
    }
}