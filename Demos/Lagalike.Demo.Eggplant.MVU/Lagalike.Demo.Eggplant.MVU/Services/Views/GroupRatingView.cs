namespace Lagalike.Demo.Eggplant.MVU.Services.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Telegram.Bot.Types;

    using Lagalike.Demo.Eggplant.MVU.Commands;
    using Lagalike.Demo.Eggplant.MVU.Services.Domain;
    using Lagalike.Telegram.Shared.Contracts;
    using Lagalike.Telegram.Shared.Services;

    using Microsoft.Extensions.Caching.Memory;

    using PatrickStar.MVU;

    public record GroupRatingView : BaseMenuView<CommandTypes>
    {
        public GroupRatingView(CommandsFactory commandsFactory, MenuBuilder<CommandTypes> menuBuilder)
        {
            InitialMenu = menuBuilder
                           .Row()
                           .Build("It's no a group or members to size their cocks.");
            Menu = InitialMenu;
        }

        public override Menu<CommandTypes> InitialMenu { get; }

        /// <inheritdoc />
        public override IView<CommandTypes> Update(IElement sourceMenu)
        {
            var view = this with
            {
                Menu = sourceMenu
            };

            return view;
        }
    }

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

        public async Task<GroupRatingInfo> GetRatingAsync(string groupId)
        {
            const byte TopUsersAmount = 3;
            var checkedUsersInGroup = (await GetUsersInGroupAsync(groupId))
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
                                      .Where(x => x.ChatMember.IsMember == true)
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
            return $"{chatMember.User.FirstName} {chatMember.User.Username}".Trim();
        }
    }

    public class GroupRatingStore : BaseTelegramBotCache<GroupRatingInfo>
    {
        private const string DEMO_NAME = "group-rating-cock-size";

        public GroupRatingStore(IMemoryCache telegramCache)
            : base(telegramCache, DEMO_NAME)
        {
        }
    }

    public record GroupRatingInfo
    {
        public IReadOnlyCollection<GroupUserInfo> TopUsers { get; init; } = Array.Empty<GroupUserInfo>();
    }

    public record GroupUserInfo
    {
        public string FullName { get; init; } = null!;

        public CockSize CockSize { get; init; } = null!;
    }
}