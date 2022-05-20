namespace DudesComparer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Ardalis.SmartEnum;

    using CSharpFunctionalExtensions;

    using DudesComparer.Models;

    using ComparedDudesResult = CSharpFunctionalExtensions.Result<ComparedDudes, ComparedDudesErrors>;

    public abstract class ComparedDudesErrors: SmartEnum<ComparedDudesErrors>
    {
        public static readonly ComparedDudesErrors UnknownChatDude = new UnknownChatDudeError(string.Empty);
        public static readonly ComparedDudesErrors EmptyChatDudes = new EmptyChatDudesError();

        public static ComparedDudesErrors GetUnknownChatDude(string dudeUserName)
        {
            return new UnknownChatDudeError(dudeUserName);
        }

        public string DudeUserName { get; init; } = null!;

        protected ComparedDudesErrors(string name, int value)
            : base(name, value)
        {
        }
        
        private sealed class EmptyChatDudesError : ComparedDudesErrors
        {
            public EmptyChatDudesError(): base(nameof(EmptyChatDudesError), 0) {}
        }

        private sealed class UnknownChatDudeError : ComparedDudesErrors
        {
            public UnknownChatDudeError(string dudeUserName)
                : base(nameof(UnknownChatDudeError), 1)
            {
                DudeUserName = dudeUserName;
            }
        }
    }

    public interface IDudesHandler
    {
        Task<Result<ComparedDudes, ComparedDudesErrors>> CompareDudesAsync(ComparingDudes comparingDudes);
    }
    
    public sealed class DudesHandler: IDudesHandler
    {
        private readonly IDudesComparerStore _store;

        private readonly ICockSizerCache _cockSizerCache;

        public DudesHandler(IDudesComparerStore store, ICockSizerCache cockSizerCache)
        {
            _store = store;
            _cockSizerCache = cockSizerCache;
        }

        public async Task<ComparedDudesResult> CompareDudesAsync(ComparingDudes comparingDudes)
        {
            var chatDudes = await ChatMembers(comparingDudes);
            if (chatDudes is null)
                return ComparedDudesErrors.EmptyChatDudes;

            var comparedDudes = GetComparedDudes(chatDudes);

            return comparedDudes;
        }

        private async Task<IReadOnlyCollection<ChatMember>?> ChatMembers(ComparingDudes comparingDudes)
        {
            var foundChatDudes = comparingDudes.DudesUserNames
                                               .Select(x => _store.GetChatMemberAsync(comparingDudes.ChatId, x));
            var chatDudes = (await Task.WhenAll(foundChatDudes))
                .Where(x => x.IsMember)
                .ToArray();

            var hasMissedDudes = chatDudes.Length != comparingDudes.DudesUserNames.Count;
            if (hasMissedDudes)
            {
                return null;
            }

            return chatDudes;
        }

        private ComparedDudes GetComparedDudes(IEnumerable<ChatMember> chatDudes)
        {
            var dudesCocks = chatDudes.Select(x => (CockSizeInfo: _cockSizerCache.GetCheckedUser(x.User.UserId), ChatMember: x))
                                      .Where(x => x.CockSizeInfo != null);
            var comparedDudes = dudesCocks.OrderByDescending(x =>
                                          {
                                              Debug.Assert(x.CockSizeInfo != null, "x.CockSizeInfo != null");
                                              return x.CockSizeInfo.CockSize.Size;
                                          })
                                          .Select((x, i) =>
                                          {
                                              Debug.Assert(x.CockSizeInfo != null, "x.CockSizeInfo != null");
                                              return new DudeInfo
                                              {
                                                  DudeType = GetDudeType(i),
                                                  CockSize = x.CockSizeInfo.CockSize,
                                                  UserInfo = x.ChatMember.User
                                              };
                                          })
                                          .ToArray();
            var result = new ComparedDudes
            {
                DudeInfos = comparedDudes
            };

            return result;
        }

        private static DudeTypes GetDudeType(int dudeIndex)
        {
            const byte DudeNumberOne = 0;

            return dudeIndex == DudeNumberOne
                ? DudeTypes.Winner
                : DudeTypes.Loser;
        }
    }

    public sealed record ComparingDudes
    {
        public ChatId ChatId { get; init; } = null!;

        public IReadOnlyCollection<string> DudesUserNames { get; init; } = Array.Empty<string>();
    }

    public sealed record ComparedDudes
    {
        public IReadOnlyCollection<DudeInfo> DudeInfos { get; init; } = Array.Empty<DudeInfo>();
    }

    public sealed record DudeInfo
    {
        public DudeTypes DudeType { get; init; }

        public CockSize CockSize { get; init; } = null!;

        public UserInfo UserInfo { get; init; } = null!;
    }

    public enum DudeTypes
    {
        Loser,
        Winner
    }
}