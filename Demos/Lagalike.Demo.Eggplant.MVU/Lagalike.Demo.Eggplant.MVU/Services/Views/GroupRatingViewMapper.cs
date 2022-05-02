namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using System.Collections.Generic;
    using System.Linq;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;
    using Lagalike.Demo.Eggplant.MVU.Services.ModelUpdaters;

    using PatrickStar.MVU;

    public class GroupRatingViewMapper : IViewMapper<CommandTypes>
    {
        private readonly GroupRatingView _view;

        public GroupRatingViewMapper(GroupRatingView view)
        {
            _view = view;
        }

        /// <inheritdoc />
        public IView<CommandTypes> Map(IModel sourceModel)
        {
            var model = (GroupRatingModel)sourceModel;
            if (model.GroupRating == null)
            {
                return _view.Update(_view.InitialMenu);
            }

            var menu = (Menu<CommandTypes>) _view.Menu;
            var formattedTopUsers = FormatTopUsers(model.GroupRating.TopUsers);
            var msg = menu.MessageElement with
            {
                Text = formattedTopUsers
            };
            var updatedMenu = menu with
            {
                MessageElement = msg
            };
            var updatedView = _view.Update(updatedMenu);

            return updatedView;
        }

        private static string FormatTopUsers(IReadOnlyCollection<GroupUserInfo> topUsers)
        {
            var haveNoTopUsers = !topUsers.Any();
            if (haveNoTopUsers)
            {
                const string HaveNoTopUsersMsg = "The group rating is empty.";
                
                return HaveNoTopUsersMsg;
            }
            
            var formattedUsers = topUsers.Select((x, i) => $"{i + 1}. {x.FullName} <b>{x.CockSize.Size} cm</b>");
            var formatted = string.Join("\n", formattedUsers);

            return formatted;
        }
    }
}