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
        private readonly IDictionary<long, UserCockSize> _userCockSizes = new Dictionary<long, UserCockSize>
        {
            {
                1, new UserCockSize
                {
                    CockSize = new CockSize(10),
                    UserId = 1
                }
            },
            {
                2, new UserCockSize
                {
                    CockSize = new CockSize(5),
                    UserId = 2
                }
            }
        };

        public UserCockSize? GetCheckedUser(long userId)
        {
            _userCockSizes.TryGetValue(userId, out var userCockSize);

            return userCockSize;
        }
    }

    internal sealed class FakeDudesStore : IDudesComparerStore
    {
        /// <summary>
        ///     The key is a chat id, the value is a user info.
        /// </summary>
        private readonly IDictionary<string, IReadOnlyCollection<ChatMember>> _chatUsers = new Dictionary<string, IReadOnlyCollection<ChatMember>>
        {
            {
                new ChatId("testChatId").Value,
                new ChatMember[]
                {
                    new()
                    {
                        IsMember = true,
                        User = new UserInfo
                        {
                            UserId = 1,
                            FirstName = "dude1FN",
                            LastName = "dude1LN",
                            Username = "dude1"
                        }
                    },
                    new()
                    {
                        IsMember = true,
                        User = new UserInfo
                        {
                            UserId = 2,
                            FirstName = "dude2FN",
                            LastName = "dude2LN",
                            Username = "dude2"
                        }
                    }
                }
            },
        };   
        public Task<ChatMember> GetChatMemberAsync(ChatId chatId, string userName)
        {
            if (!_chatUsers.TryGetValue(chatId.Value, out var chatMemmbers))
                throw new ArgumentOutOfRangeException(nameof(chatId));
            
            var foundUser = chatMemmbers.Single(x => x.User.Username == userName);
                
            return Task.FromResult(foundUser);

        }
    }
}