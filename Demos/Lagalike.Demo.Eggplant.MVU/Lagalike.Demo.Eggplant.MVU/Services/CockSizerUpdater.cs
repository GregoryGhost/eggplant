namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System;
    using System.Threading.Tasks;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;
    using Lagalike.Demo.Eggplant.MVU.Services.Domain;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class CockSizerUpdater : IUpdater<CommandTypes, Model>
    {
        private readonly CockSizeFactory _cockSizeFactory;

        public CockSizerUpdater(CockSizeFactory cockSizeFactory)
        {
            _cockSizeFactory = cockSizeFactory;
        }
        
        /// <inheritdoc />
        public async Task<(ICommand<CommandTypes>? OutputCommand, Model UpdatedModel)> UpdateAsync(ICommand<CommandTypes> command,
            Model model)
        {
            var updatedModel = command.Type switch
            {
                CommandTypes.ShareCockSize => RandomCockSize(model),
                CommandTypes.GroupRating => GetGroupRating(model, (GroupRatingCommand)command),
                _ => throw new ArgumentOutOfRangeException($"Unknown {nameof(command)}: {command}")
            };
            ICommand<CommandTypes> emptyCmd = null!;

            return (emptyCmd, updatedModel);
        }

        private static Model GetGroupRating(Model model, GroupRatingCommand cmd)
        {
            return model with
            {
                GroupId = cmd.GroupId
            };
        }

        private Model RandomCockSize(Model model)
        {
            return model.CockSize is null
                ? model with { CockSize = _cockSizeFactory.GetRandomCockSize() }
                : model;
        }
    }
}