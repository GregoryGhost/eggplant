namespace Eggplant.MVU.CompareDudes.Views
{
    using DudesComparer.Services;

    using Eggplant.MVU.CompareDudes.Models;
    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public class CompareDudesViewMapper : IViewMapper<CommandTypes>
    {
        private readonly CompareDudesView _view;

        public CompareDudesViewMapper(CompareDudesView view)
        {
            _view = view;
        }

        /// <inheritdoc />
        public IView<CommandTypes> Map(IModel sourceModel)
        {
            var model = (CompareDudesModel)sourceModel;
            if (model.ComparedDudes is null)
                return _view.Update(_view.InitialMenu);

            var menu = (Menu<CommandTypes>)_view.Menu;
            var comparedDudes = (Result<ComparedDudes, ComparedDudesErrors>)model.ComparedDudes;
            var formattedUsers = GetComparedDudes(comparedDudes);
            var msg = menu.MessageElement with
            {
                Text = formattedUsers
            };
            var updatedMenu = menu with
            {
                MessageElement = msg
            };
            var updatedView = _view.Update(updatedMenu);

            return updatedView;
        }

        private static string FormatComparedDudes(IReadOnlyCollection<DudeInfo> comparedDudes)
        {
            var haveNoDudes = !comparedDudes.Any();
            if (haveNoDudes)
            {
                const string HaveNoDudesInGroup = "The provided dudes are not in the group.";

                return HaveNoDudesInGroup;
            }

            var formattedDudes = FormatDudes(comparedDudes);

            return formattedDudes;
        }

        private static string FormatDudes(IReadOnlyCollection<DudeInfo> comparedDudes)
        {
            var winnerInfo = comparedDudes.First(x => x.DudeType == DudeTypes.Winner);
            var losers = comparedDudes.Where(x => x.DudeType == DudeTypes.Loser)
                                      .Select(x => $"@{x.CheckedDude.Username}")
                                      .ToArray();

            var winner = winnerInfo.CheckedDude;
            var winnerCockSize = winnerInfo.CockSize.Size;
            var winnerUser = $"<b>{winner.LastName} {winner.FirstName}</b> (@{winner.Username})";
            var formattedWinner = $"💕👄👄👄💕💕💕\n{winnerUser} [<b>{winnerCockSize} cm]</b>\n💕💕💕👄👄👄💕";
            if (!losers.Any())
                return formattedWinner;

            var formattedListLosers = string.Join(";\n", losers);
            var formattedLosers = $"🦶🎢 🗑🗑🗑💩💩💩👇:\n{formattedListLosers}";
            var formatted = $"{formattedWinner}\n\n{formattedLosers}";

            return formatted;
        }

        private static string FormatErrorMsg(ComparedDudesErrors error)
        {
            var formatted = string.Empty;
            error.When(ComparedDudesErrors.EmptyChatDudes)
                 .Then(() => formatted = "The provided dudes are unknown.")
                 .When(ComparedDudesErrors.UnknownChatDudes)
                 .Then(() => formatted = FormatUnknownChatDudes(error.DudeUserNames))
                 .Default(() => throw new ArgumentOutOfRangeException(nameof(error)));

            return formatted;
        }

        private static string FormatUnknownChatDudes(IEnumerable<string> unknownDudeUserNames)
        {
            var formatted = string.Join("\n", unknownDudeUserNames.Select(nickname => $"@{nickname}"));

            return $"Unknown dudes - might be not measured or wrong nicknames dudes:\n{formatted}";
        }

        private static string GetComparedDudes(Result<ComparedDudes, ComparedDudesErrors> comparedDudes)
        {
            var formatted = comparedDudes.Match(
                value => FormatComparedDudes(value.DudeInfos),
                FormatErrorMsg);

            return formatted;
        }
    }
}