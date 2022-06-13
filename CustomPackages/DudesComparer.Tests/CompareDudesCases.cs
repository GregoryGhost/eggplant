namespace DudesComparer.Tests
{
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

        public IEnumerator GetEnumerator()
        {
            return TestCases.Select(x => new object[] { x.TestCaseName, x.InputData, x.Expected })
                            .GetEnumerator();
        }

        private static CompareDudeTestCase GetErrorCompareOnEmptyChatMembersTestCase()
        {
            return new CompareDudeTestCase
            {
                TestCaseName = nameof(GetErrorCompareOnEmptyChatMembersTestCase),
                InputData = new ComparingDudes
                {
                    ChatId = FakeDudesStore.TestChats.TestChatId,
                    DudesUserNames = new[] { "322", "228", "888" }
                },
                Expected = ComparedDudesErrors.EmptyChatDudes
            };
        }

        private static CompareDudeTestCase GetErrorCompareOnUnknownDudesTestCase()
        {
            const string UnknownDudeUserName = "dude11";

            return new CompareDudeTestCase
            {
                TestCaseName = nameof(GetErrorCompareOnUnknownDudesTestCase),
                InputData = new ComparingDudes
                {
                    ChatId = FakeDudesStore.TestChats.TestChatId,
                    DudesUserNames = new[] { "dude1", UnknownDudeUserName }
                },
                Expected = ComparedDudesErrors.GetUnknownChatDudes(new[] { UnknownDudeUserName })
            };
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
                                CheckedDude = FakeDudesStore.GetUserInfoTestChatBy(1)
                            },
                        }.Concat(
                             FakeCockSizerCache.GetUserCockSizes()
                                               .Skip(1)
                                               .Select(
                                                   x => new DudeInfo
                                                   {
                                                       DudeType = DudeTypes.Loser,
                                                       CockSize = x.CockSize,
                                                       CheckedDude = FakeDudesStore.GetUserInfoTestChatBy(x.UserId)
                                                   }))
                         .ToList()
                }
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
                            CheckedDude = new CheckedDude
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
                            CheckedDude = new CheckedDude
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