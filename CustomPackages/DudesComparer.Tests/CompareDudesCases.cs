namespace DudesComparer.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using DudesComparer.Models;
    using DudesComparer.Services;

    internal class CompareDudesCases : IEnumerable
    {
        private static readonly IReadOnlyCollection<CompareDudeTestCase> TestCases = new[]
        {
            GetOkCompareTwoDudesCompareTestCase(),
            GetOkCompareManyDudesCompareTestCase(),
            GetErrorCompareOnEmptyChatMembersTestCase(),
            GetErrorCompareOnUnknownDudesTestCase()
        };

        private static CompareDudeTestCase GetErrorCompareOnUnknownDudesTestCase()
        {
            const string UnknownDudeUserName = "dude11";
            
            return new CompareDudeTestCase
            {
                TestCaseName = nameof(GetErrorCompareOnUnknownDudesTestCase),
                InputData = new ComparingDudes
                {
                    ChatId = FakeDudesStore.TestChats.TestChatId,
                    DudesUserNames = new [] { "dude1", UnknownDudeUserName }
                },
                Expected = ComparedDudesErrors.GetUnknownChatDudes(new []{UnknownDudeUserName})
            };
        }

        public IEnumerator GetEnumerator()
        {
            return TestCases.Select(x => new object[] {x.TestCaseName, x.InputData, x.Expected})
                            .GetEnumerator();
        }

        private static CompareDudeTestCase GetOkCompareManyDudesCompareTestCase()
        {
            return new CompareDudeTestCase
            {
                TestCaseName = nameof(GetOkCompareManyDudesCompareTestCase),
                InputData = new ComparingDudes
                {
                    ChatId = FakeDudesStore.TestChats.TestChatId,
                    DudesUserNames = FakeDudesStore.GetChatUsernames(FakeDudesStore.TestChats.TestChatId)
                                                   .ToArray()
                },
                Expected = new ComparedDudes
                {
                    DudeInfos = new DudeInfo[]
                        {
                            new()
                            {
                                DudeType = DudeTypes.Winner,
                                CockSize = new CockSize
                                {
                                    Size = 10
                                },
                                UserInfo = FakeDudesStore.GetUserInfoTestChatBy(1)
                            },
                        }.Concat(
                             FakeCockSizerCache.GetUserCockSizes()
                                               .Skip(1)
                                               .Select(
                                                   x => new DudeInfo
                                                   {
                                                       DudeType = DudeTypes.Loser,
                                                       CockSize = x.CockSize,
                                                       UserInfo = FakeDudesStore.GetUserInfoTestChatBy(x.UserId)
                                                   }))
                         .ToList()
                }
            };
        }

        private static CompareDudeTestCase GetErrorCompareOnEmptyChatMembersTestCase()
        {
            return new CompareDudeTestCase
            {
                TestCaseName = nameof(GetErrorCompareOnEmptyChatMembersTestCase),
                InputData = new ComparingDudes
                {
                    ChatId = FakeDudesStore.TestChats.TestChatId,
                    DudesUserNames = new [] { "322", "228", "888" }
                },
                Expected = ComparedDudesErrors.EmptyChatDudes
            };
        }

        private static CompareDudeTestCase GetOkCompareTwoDudesCompareTestCase()
        {
            return new CompareDudeTestCase
            {
                TestCaseName = nameof(GetOkCompareTwoDudesCompareTestCase),
                InputData = new ComparingDudes
                {
                    ChatId = FakeDudesStore.TestChats.TestChatId,
                    DudesUserNames = FakeDudesStore.GetChatUsernames(FakeDudesStore.TestChats.TestChatId)
                                                   .Take(2)
                                                   .ToArray()
                },
                Expected = new ComparedDudes
                {
                    DudeInfos = new DudeInfo[]
                    {
                        new()
                        {
                            DudeType = DudeTypes.Winner,
                            CockSize = new CockSize
                            {
                                Size = 10
                            },
                            UserInfo = new UserInfo
                            {
                                UserId = 1,
                                FirstName = "dude1FN",
                                LastName = "dude1LN",
                                Username = "dude1"
                            }
                        },
                        new()
                        {
                            DudeType = DudeTypes.Loser,
                            CockSize = new CockSize
                            {
                                Size = 9
                            },
                            UserInfo = new UserInfo
                            {
                                UserId = 2,
                                FirstName = "dude2FN",
                                LastName = "dude2LN",
                                Username = "dude2"
                            }
                        }
                    }
                }
            };
        }
    }
}