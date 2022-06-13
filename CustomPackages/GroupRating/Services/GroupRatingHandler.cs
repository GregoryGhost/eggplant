namespace GroupRating.Services
{
    using GroupRating.Models;

    public class GroupRatingHandler
    {
        private readonly IGroupRatingStore _groupRatingStore;

        private readonly ICockSizerCache _userCockSizer;

        public GroupRatingHandler(ICockSizerCache userCockSizer,
            IGroupRatingStore groupRatingStore)
        {
            _userCockSizer = userCockSizer;
            _groupRatingStore = groupRatingStore;
        }

        public async Task<GroupRatingInfo> GetRatingAsync(GroupId groupId)
        {
            const byte TopUsersAmount = 3;
            var checkedUsersInGroup = (await GetUsersInGroupAsync(groupId.Value))
                                      .OrderByDescending(x => x.CockSize.Size)
                                      .Take(TopUsersAmount)
                                      .ToArray();
            var groupRatingInfo = new GroupRatingInfo
            {
                TopUsers = checkedUsersInGroup
            };

            return groupRatingInfo;
        }

        private static string FormatUserFullName(ChatMember chatMember)
        {
            var fullName = $"{chatMember.User.FirstName} {chatMember.User.LastName}";
            var userName = $"(@{chatMember.User.Username})";
            var formatted = $"{fullName} {userName}".Trim();

            return formatted;
        }

        private async Task<IReadOnlyCollection<GroupUserInfo>> GetUsersInGroupAsync(string groupId)
        {
            var chatId = new ChatId(groupId);
            var checkedUsersInGroupTasks = _userCockSizer.GetCheckedUsers()
                                                         .Select(
                                                             async userCockSize =>
                                                             {
                                                                 var chatMember = await _groupRatingStore.GetChatMemberAsync(
                                                                     chatId,
                                                                     userCockSize.UserId);

                                                                 return (UserCockSize: userCockSize.CockSize,
                                                                     ChatMember: chatMember);
                                                             });
            var checkedUsersInGroup = (await Task.WhenAll(checkedUsersInGroupTasks))
                                      .Where(x => x.ChatMember.IsMember)
                                      .Select(
                                          x => new GroupUserInfo
                                          {
                                              FullName = FormatUserFullName(x.ChatMember),
                                              CockSize = x.UserCockSize
                                          })
                                      .ToList();

            return checkedUsersInGroup;
        }
    }
}