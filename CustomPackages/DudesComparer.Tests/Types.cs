﻿namespace DudesComparer.Tests
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
                                              CockSize = new CockSize
                                              {
                                                  Size = (byte) (MaxCockSize - (byte) i)
                                              },
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
        internal static class TestChats
        {
            internal static readonly ChatId TestChatId = new(nameof(TestChatId));

            internal static readonly ChatId TestFakeChatId = new(nameof(TestFakeChatId));
        }

        /// <summary>
        ///     The key is a chat id, the value is a user info.
        /// </summary>
        private static readonly IDictionary<string, IReadOnlyCollection<ChatMember>> ChatUsers;

        static FakeDudesStore()
        {
            ChatUsers = new Dictionary<string, IReadOnlyCollection<ChatMember>>
            {
                {
                    TestChats.TestChatId.Value,
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
        }

        public Task<ChatMember> GetChatMemberAsync(ChatId chatId, string userName)
        {
            if (!ChatUsers.TryGetValue(chatId.Value, out var chatMemmbers))
                throw new ArgumentOutOfRangeException(nameof(chatId));

            var foundUser = chatMemmbers.SingleOrDefault(x => x.User.Username == userName);
            if (foundUser is not null)
                return Task.FromResult(foundUser);
            
            var emptyUser = new ChatMember
            {
                IsMember = false,
                User = new UserInfo
                {
                    UserId = 0,
                    FirstName = null!,
                    LastName = null!,
                    Username = userName
                }
            };
                
            return Task.FromResult(emptyUser);
        }

        internal static IEnumerable<string> GetChatUsernames(ChatId chatId)
        {
            return ChatUsers[chatId.Value]
                .Select(x => x.User.Username);
        }

        internal static UserInfo GetUserInfoTestFakeChatBy(long userId)
        {
            return ChatUsers[TestChats.TestFakeChatId.Value]
                   .Single(x => x.User.UserId == userId)
                   .User;
        }

        internal static UserInfo GetUserInfoTestChatBy(long userId)
        {
            return ChatUsers[TestChats.TestChatId.Value]
                   .Single(x => x.User.UserId == userId)
                   .User;
        }
    }
}