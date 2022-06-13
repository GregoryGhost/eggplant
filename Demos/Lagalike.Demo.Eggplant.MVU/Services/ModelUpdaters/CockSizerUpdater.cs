namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using CockSizer.Services;

    using DudesComparer.Models;
    using DudesComparer.Services;

    using global::Eggplant.MVU.CompareDudes.Commands;
    using global::Eggplant.MVU.CompareDudes.Models;
    using global::Eggplant.MVU.GroupRating.Commands;
    using global::Eggplant.MVU.GroupRating.Models;
    using global::Eggplant.MVU.ShareCockSize.Commands;
    using global::Eggplant.MVU.ShareCockSize.Models;
    using global::Eggplant.MVU.UnknownCmd.Commands;
    using global::Eggplant.MVU.UnknownCmd.Models;
    using global::Eggplant.Types.Shared;

    using GroupRating.Services;

    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    using ChatId = GroupRating.Models.ChatId;

    /// <inheritdoc />
    public class CockSizerUpdater : IUpdater<CommandTypes, Model>
    {
        private readonly BotCommandsUsageConfigurator _botCommandsUsageConfigurator;

        private readonly CockSizeFactory _cockSizeFactory;

        private readonly GroupRatingHandler _groupRatingHandler;

        private readonly IDudesHandler _compareDudesHandler;

        private readonly IGroupRatingStore _groupRatingStore;

        public CockSizerUpdater(CockSizeFactory cockSizeFactory, GroupRatingHandler groupRatingHandler,
            BotCommandsUsageConfigurator botCommandsUsageConfigurator, IDudesHandler compareDudesHandler,
            IGroupRatingStore groupRatingStore)
        {
            _cockSizeFactory = cockSizeFactory;
            _groupRatingHandler = groupRatingHandler;
            _botCommandsUsageConfigurator = botCommandsUsageConfigurator;
            _compareDudesHandler = compareDudesHandler;
            _groupRatingStore = groupRatingStore;
        }

        /// <inheritdoc />
        public async Task<(ICommand<CommandTypes>? OutputCommand, Model UpdatedModel)> UpdateAsync(ICommand<CommandTypes> command,
            Model model)
        {
            var updatedModel = command.Type switch
            {
                CommandTypes.ShareCockSize => await RandomCockSizeAsync(model, (ShareCockSizeCommand) command),
                CommandTypes.GroupRating => await GetGroupRatingAsync(model, (GroupRatingCommand) command),
                CommandTypes.UnknownCommand => GetAvailableBotCommandModel(model, (UnknownCommand) command),
                CommandTypes.CompareDudes => await CompareDudesAsync(model, (CompareDudesCommand) command),
                CommandTypes.MessageWithoutAnyCmdCommand => GetMessageWithouAnyCmdModel(model),
                _ => throw new ArgumentOutOfRangeException($"Unknown {nameof(command)}: {command}")
            };
            ICommand<CommandTypes> emptyCmd = null!;

            return (emptyCmd, updatedModel);
        }

        private async Task<Model> CompareDudesAsync(Model model, CompareDudesCommand command)
        {
            model = model with
            {
                CurrentCommand = command.Type
            };

            var comparedDudes = await _compareDudesHandler.CompareDudesAsync(command.ComparingDudes);

            var compareDudesModel = model.CompareDudesModel is null
                ? new CompareDudesModel
                {
                    ComparedDudes = comparedDudes,
                    CheckedDude = null
                }
                : model.CompareDudesModel with
                {
                    ComparedDudes = comparedDudes
                };
            var initialized = model with
            {
                CompareDudesModel = compareDudesModel
            };

            return initialized;
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

        private async Task<Model> RandomCockSizeAsync(Model model, ShareCockSizeCommand command)
        {
            model = model with
            {
                CurrentCommand = command.Type
            };
            if (model.CockSizeModel?.CockSize is not null)
                return model;

            var checkedDude = await GetCheckedDude(command);
            var compareDudesModel = model.CompareDudesModel is null
                ? new CompareDudesModel
                {
                    ComparedDudes = null,
                    CheckedDude = checkedDude
                }
                : model.CompareDudesModel with
                {
                    CheckedDude = checkedDude
                };
            var initialized = model with
            {
                CockSizeModel = new PersonCockSizeModel
                {
                    CockSize = _cockSizeFactory.GetRandomCockSize()
                },
                CompareDudesModel = compareDudesModel
            };

            return initialized;
        }

        private async Task<CheckedDude> GetCheckedDude(ShareCockSizeCommand command)
        {
            var userId = long.Parse(command.ChatId);
            var chatMember = await _groupRatingStore.GetChatMemberAsync(
                new ChatId(command.ChatId),
                userId);
            var checkedDude = new CheckedDude
            {
                UserId = userId,
                FirstName = chatMember.User.FirstName,
                LastName = chatMember.User.LastName,
                Username = chatMember.User.Username
            };
            
            return checkedDude;
        }
    }
}