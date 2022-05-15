namespace DudesComparer.Tests
{
    using System.Threading.Tasks;

    using DudesComparer.Models;
    using DudesComparer.Services;

    using FluentAssertions;

    using NUnit.Framework;

    public class Tests
    {
        [Test]
        public async Task CompareTwoDudesTestAsync()
        {
            var dudesHandler = DudesHandlerFactory.GetInitializedDudesHandler();
            var dudesToCompare = new ComparingDudes
            {
                ChatId = new ChatId("testChatId"),
                DudesUserNames = new[] {"dude1", "dude2"}
            };
            var actualComparedDudes = await dudesHandler.CompareDudesAsync(dudesToCompare);
            var expectedComparedDudes = new ComparedDudes
            {
                DudeInfos = new DudeInfo[]
                {
                    new()
                    {
                        DudeType = DudeTypes.Winner,
                        CockSize = new CockSize(10),
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
                        CockSize = new CockSize(5),
                        UserInfo = new UserInfo
                        {
                            UserId = 2,
                            FirstName = "dude2FN",
                            LastName = "dude2LN",
                            Username = "dude2"
                        }
                    }
                }
            };

            expectedComparedDudes.Should().BeEquivalentTo(actualComparedDudes);
        }

        [SetUp]
        public void Setup()
        {
        }
    }
}