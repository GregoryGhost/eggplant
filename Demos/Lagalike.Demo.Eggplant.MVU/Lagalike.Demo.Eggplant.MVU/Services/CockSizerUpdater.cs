namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System;
    using System.Threading.Tasks;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class CockSizerUpdater : IUpdater<CommandTypes, Model>
    {
        private static readonly Random Random = new ();

        /// <inheritdoc />
        public async Task<(ICommand<CommandTypes>? OutputCommand, Model UpdatedModel)> UpdateAsync(ICommand<CommandTypes> command,
            Model model)
        {
            var updatedModel = command.Type switch
            {
                CommandTypes.GetCockSize or CommandTypes.ShareCockSize => RandomCockSize(model),
                _ => throw new ArgumentOutOfRangeException($"Unknown {nameof(command)}: {command}")
            };
            ICommand<CommandTypes> emptyCmd = null!;

            return (emptyCmd, updatedModel);
        }

        private static Model RandomCockSize(Model model)
        {
            const byte MinCockSize = 1;
            const byte MaxCockSize = 50;
            
            return model.CockSize is null
                ? model with { CockSize = (byte)Random.Next(MinCockSize, MaxCockSize)}
                : model;
        }
    }
}