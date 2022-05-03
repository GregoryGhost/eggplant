namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System;
    using System.Threading.Tasks;

    using CockSizer.Services;

    using global::Eggplant.MVU.GroupRating.Commands;
    using global::Eggplant.MVU.GroupRating.Models;
    using global::Eggplant.MVU.ShareCockSize.Models;
    using global::Eggplant.MVU.UnknownCmd.Commands;
    using global::Eggplant.MVU.UnknownCmd.Models;
    using global::Eggplant.Types.Shared;

    using GroupRating.Services;

    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class CockSizerUpdater : IUpdater<CommandTypes, Model>
    {
        private readonly BotCommandsUsageConfigurator _botCommandsUsageConfigurator;

        private readonly CockSizeFactory _cockSizeFactory;

        private readonly GroupRatingHandler _groupRatingHandler;

        public CockSizerUpdater(CockSizeFactory cockSizeFactory, GroupRatingHandler groupRatingHandler,
            BotCommandsUsageConfigurator botCommandsUsageConfigurator)
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
                CommandTypes.GroupRating => await GetGroupRatingAsync(model, (GroupRatingCommand) command),
                CommandTypes.UnknownCommand => GetAvailableBotCommandModel(model, (UnknownCommand) command),
                CommandTypes.MessageWithoutAnyCmdCommand => GetMessageWithouAnyCmdModel(model),
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

        private static Model GetMessageWithouAnyCmdModel(Model model)
        {
            return model with
            {
                CurrentCommand = CommandTypes.MessageWithoutAnyCmdCommand
            };
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