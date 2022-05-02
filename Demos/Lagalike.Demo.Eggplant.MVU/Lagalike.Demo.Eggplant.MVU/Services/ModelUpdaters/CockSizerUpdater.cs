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

        public CockSizerUpdater(CockSizeFactory cockSizeFactory, GroupRatingHandler groupRatingHandler)
        {
            _cockSizeFactory = cockSizeFactory;
            _groupRatingHandler = groupRatingHandler;
        }
        
        /// <inheritdoc />
        public async Task<(ICommand<CommandTypes>? OutputCommand, Model UpdatedModel)> UpdateAsync(ICommand<CommandTypes> command,
            Model model)
        {
            var updatedModel = command.Type switch
            {
                CommandTypes.ShareCockSize => RandomCockSize(model),
                CommandTypes.GroupRating => await GetGroupRatingAsync(model, (GroupRatingCommand)command),
                _ => throw new ArgumentOutOfRangeException($"Unknown {nameof(command)}: {command}")
            };
            ICommand<CommandTypes> emptyCmd = null!;

            return (emptyCmd, updatedModel);
        }

        private async Task<Model> GetGroupRatingAsync(Model model, GroupRatingCommand cmd)
        {
            var hasGroupRating = model.GroupRatingModel?.GroupRating?.TopUsers?.Any() ?? false;
            model = model with
            {
                CurrentCommand = CommandTypes.GroupRating
            };
            if (hasGroupRating)
                return model;

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