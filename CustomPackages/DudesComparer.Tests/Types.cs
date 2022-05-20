namespace DudesComparer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DudesComparer.Models;
    using DudesComparer.Services;

    internal static class DudesHandlerFactory
    {
        public static IDudesHandler GetInitializedDudesHandler()
        {
            var store = new FakeDudesStore();
            var cache = new FakeCockSizerCache();
            var dudesHandler = new DudesHandler(store, cache);

            return dudesHandler;
        }
    }

    internal sealed class FakeCockSizerCache : ICockSizerCache
    {
        /// <summary>
        ///     The key is a userId, the value is a user cock size info.
        /// </summary>
        private static readonly IDictionary<long, UserCockSize> UserCockSizes;

        static FakeCockSizerCache()
        {
            const byte MaxCockSize = 10;
            UserCockSizes = Enumerable.Range(1, 10)
                                      .Select(
                                          (userId, i) => new UserCockSize
                                          {
                                              CockSize = new CockSize((byte) (MaxCockSize - (byte) i)),
                                              UserId = userId
                                          })
                                      .ToDictionary(x => x.UserId, x => x);
        }

        public UserCockSize? GetCheckedUser(long userId)
        {
            UserCockSizes.TryGetValue(userId, out var userCockSize);

            return userCockSize;
        }

        public static IEnumerable<UserCockSize> GetUserCockSizes()
        {
            return UserCockSizes.Values;
        }
    }

    internal sealed class FakeDudesStore : IDudesComparerStore
    {
        internal static readonly ChatId TestChatId = new("testChatId");

        /// <summary>
        ///     The key is a chat id, the value is a user info.
        /// </summary>
        private static readonly IDictionary<string, IReadOnlyCollection<ChatMember>> ChatUsers =
            new Dictionary<string, IReadOnlyCollection<ChatMember>>
            {
                {
                    TestChatId.Value,
                    Enumerable.Range(1, 10)
                              .Select(
                                  userId => new ChatMember
                                  {
                                      IsMember = true,
                                      User = new UserInfo
                                      {
                                          UserId = userId,
                                          FirstName = $"dude{userId}FN",
                                          LastName = $"dude{userId}LN",
                                          Username = $"dude{userId}"
                                      }
                                  })
                              .ToArray()
                },
            };

        public Task<ChatMember> GetChatMemberAsync(ChatId chatId, string userName)
        {
            if (!ChatUsers.TryGetValue(chatId.Value, out var chatMemmbers))
                throw new ArgumentOutOfRangeException(nameof(chatId));

            var foundUser = chatMemmbers.Single(x => x.User.Username == userName);

            return Task.FromResult(foundUser);
        }

        internal static IEnumerable<string> GetChatUsernames(ChatId chatId)
        {
            return ChatUsers[chatId.Value]
                .Select(x => x.User.Username);
        }

        internal static UserInfo GetUserInfoById(long userId)
        {
            return ChatUsers[TestChatId.Value].Single(x => x.User.UserId == userId).User;
        }
    }
}