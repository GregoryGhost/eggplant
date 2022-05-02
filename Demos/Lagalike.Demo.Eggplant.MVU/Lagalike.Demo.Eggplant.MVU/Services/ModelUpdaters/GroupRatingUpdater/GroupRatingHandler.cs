namespace Lagalike.Demo.Eggplant.MVU.Services.ModelUpdaters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;

    using Lagalike.Demo.Eggplant.MVU.Models;
    using Lagalike.Telegram.Shared.Services;

    public class GroupRatingHandler
    {
        private readonly GroupRatingStore _groupRatingStore;

        private readonly CockSizerCache _userCockSizer;

        private readonly ConfiguredTelegramBotClient _telegramClient;

        public GroupRatingHandler(GroupRatingStore groupRatingStore, CockSizerCache userCockSizer,
            ConfiguredTelegramBotClient telegramClient)
        {
            _groupRatingStore = groupRatingStore;
            _userCockSizer = userCockSizer;
            _telegramClient = telegramClient;
        }

        public async Task<GroupRatingInfo> GetRatingAsync(GroupId groupId)
        {
            const byte TopUsersAmount = 3;
            var checkedUsersInGroup = (await GetUsersInGroupAsync(groupId.Value))
                                      .OrderByDescending(x => x.CockSize)
                                      .Take(TopUsersAmount)
                                      .ToArray();
            var groupRatingInfo = new GroupRatingInfo
            {
                TopUsers = checkedUsersInGroup
            };

            return groupRatingInfo;
        }

        private async Task<IReadOnlyCollection<GroupUserInfo>> GetUsersInGroupAsync(string groupId)
        {
            var chatId = new ChatId(groupId);
            var checkedUsersInGroupTasks = _userCockSizer.GetCheckedUsers()
                                                         .Select(
                                                             async userCockSize =>
                                                             {
                                                                 var chatMember = await _telegramClient.GetChatMemberAsync(
                                                                     chatId,
                                                                     userCockSize.UserId);

                                                                 return (UserCockSize: userCockSize.CockSize,
                                                                     ChatMember: chatMember);
                                                             });
            var checkedUsersInGroup = (await Task.WhenAll(checkedUsersInGroupTasks))
                                      .Where(x => x.ChatMember.Status is ChatMemberStatus.Administrator or ChatMemberStatus.Creator or ChatMemberStatus.Member)
                                      .Select(
                                          x => new GroupUserInfo
                                          {
                                              FullName = FormatUserFullName(x.ChatMember),
                                              CockSize = x.UserCockSize
                                          })
                                      .ToList();

            return checkedUsersInGroup;
        }

        private static string FormatUserFullName(ChatMember chatMember)
        {
            var fullName = $"{chatMember.User.FirstName} {chatMember.User.LastName}";
            var userName = $"(@{chatMember.User.Username})";
            var formatted = $"{fullName} {userName}".Trim();
            
            return formatted;
        }
    }
}