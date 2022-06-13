namespace DudesComparer.Tests
{
    using DudesComparer.Models;

    public static class DataFaker
    {
        private static readonly Faker<ChatMember> ChatMemberFaker;

        private static readonly IList<ChatMember> ChatMembers;

        private static readonly Faker<CockSize> CockSizeFaker;

        private static readonly IList<CockSize> CockSizes;

        private static readonly IList<ChatMember> NoChatMembers;

        private static readonly IList<UserCockSize> UserCockSizes;

        private static readonly Faker<UserCockSize> UserCockSizesFaker;

        private static readonly Faker<CheckedDude> UserInfoFaker;

        private static readonly IList<CheckedDude> Users;

        static DataFaker()
        {
            const int NumToSeed = 322;
            Randomizer.Seed = new Random(NumToSeed);

            UserInfoFaker = new Faker<CheckedDude>()
                            .RuleFor(x => x.UserId, x => x.IndexVariable++)
                            .RuleFor(x => x.Username, x => x.Person.UserName)
                            .RuleFor(x => x.FirstName, x => x.Person.FirstName)
                            .RuleFor(x => x.LastName, x => x.Person.LastName);
            Users = UserInfoFaker.Generate(NumToSeed);

            ChatMemberFaker = new Faker<ChatMember>()
                              .RuleFor(x => x.IsMember, f => f.Random.Bool())
                              .RuleFor(x => x.User, f => f.PickRandom(Users));
            var chatMembers = ChatMemberFaker.Generate(NumToSeed);
            ChatMembers = chatMembers.Where(x => x.IsMember).ToList();
            NoChatMembers = chatMembers.Where(x => !x.IsMember).ToList();

            CockSizeFaker = new Faker<CockSize>()
                .RuleFor(x => x.Size, f => f.Random.Byte());
            CockSizes = CockSizeFaker.Generate(NumToSeed);

            UserCockSizesFaker = new Faker<UserCockSize>()
                                 .RuleFor(x => x.UserId, f => f.PickRandom(Users).UserId)
                                 .RuleFor(x => x.CockSize, f => f.PickRandom(CockSizes));

            UserCockSizes = UserCockSizesFaker.Generate(NumToSeed);
        }

        public static ChatMember GetChatMember()
        {
            return ChatMemberFaker.Generate();
        }

        public static IEnumerable<ChatMember> GetChatMembers()
        {
            return ChatMembers;
        }

        public static IEnumerable<ChatMember> GetNoChatMembers()
        {
            return NoChatMembers;
        }

        public static UserCockSize GetUserCockSize()
        {
            return UserCockSizesFaker;
        }

        public static IEnumerable<UserCockSize> GetUserCockSizes()
        {
            return UserCockSizes;
        }

        public static CheckedDude GetUserInfo()
        {
            return UserInfoFaker.Generate();
        }
    }
}