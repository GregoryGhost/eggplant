namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    public class GroupRatingViewMapper : IViewMapper<CommandTypes>
    {
        private readonly GroupRatingView _view;

        private readonly GroupRatingHandler _groupRatingHandler;

        public GroupRatingViewMapper(GroupRatingView view, GroupRatingHandler groupRatingHandler)
        {
            _view = view;
            _groupRatingHandler = groupRatingHandler;
        }

        /// <inheritdoc />
        public IView<CommandTypes> Map(IModel model)
        {
            var defaultModel = (Model)model;
            if (defaultModel.GroupId == null)
            {
                return _view.Update(_view.InitialMenu);
            }

            var menu = (Menu<CommandTypes>) _view.Menu;
            var formattedTopUsers = GetTopUsers(defaultModel);
            var updatedMenu = menu.MessageElement with
            {
                Text = formattedTopUsers
            };
            var updatedView = _view.Update(updatedMenu);

            return updatedView;
        }

        //TODO: to async
        private string GetTopUsers(Model defaultModel)
        {
            if (defaultModel.GroupId == null)
            {
                throw new ArgumentException("Have no any value", nameof(defaultModel.GroupId));
            }
            var topUsers = _groupRatingHandler.GetRatingAsync(defaultModel.GroupId.Value).Result.TopUsers;
            var formattedTopUsers = FormatTopUsers(topUsers);
            
            return formattedTopUsers;
        }

        private static string FormatTopUsers(IReadOnlyCollection<GroupUserInfo> topUsers)
        {
            var formattedUsers = topUsers.Select((x, i) => $"{i}. {x.FullName} {x.CockSize}");
            var formatted = string.Join("\n", formattedUsers);

            return formatted;
        }
    }
}