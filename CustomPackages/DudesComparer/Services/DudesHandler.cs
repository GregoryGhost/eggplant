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

    public abstract class ComparedDudesErrors : SmartEnum<ComparedDudesErrors>
    {
        public static readonly ComparedDudesErrors EmptyChatDudes = new EmptyChatDudesError();

        public static readonly ComparedDudesErrors UnknownChatDudes = new UnknownChatDudeError(Array.Empty<string>());

        protected ComparedDudesErrors(string name, int value)
            : base(name, value)
        {
        }

        public IReadOnlyCollection<string> DudeUserNames { get; init; } = Array.Empty<string>();

        public static ComparedDudesErrors GetUnknownChatDudes(IReadOnlyCollection<string> dudeUserNames)
        {
            return new UnknownChatDudeError(dudeUserNames);
        }

        private sealed class EmptyChatDudesError : ComparedDudesErrors
        {
            public EmptyChatDudesError()
                : base(nameof(EmptyChatDudesError), 0)
            {
            }
        }

        private sealed class UnknownChatDudeError : ComparedDudesErrors
        {
            public UnknownChatDudeError(IReadOnlyCollection<string> dudeUserNames)
                : base(nameof(UnknownChatDudeError), 1)
            {
                DudeUserNames = dudeUserNames;
            }
        }
    }

    public interface IDudesHandler
    {
        Task<ComparedDudesResult> CompareDudesAsync(ComparingDudes? comparingDudes);
    }

    public sealed class DudesHandler : IDudesHandler
    {
        private readonly ICockSizerCache _cockSizerCache;

        private readonly IDudesComparerStore _store;

        public DudesHandler(IDudesComparerStore store, ICockSizerCache cockSizerCache)
        {
            _store = store;
            _cockSizerCache = cockSizerCache;
        }

        public async Task<ComparedDudesResult> CompareDudesAsync(ComparingDudes? comparingDudes)
        {
            var chatDudes = await GetChatDudesAsync(comparingDudes);
            if (chatDudes.IsFailure)
                return chatDudes.Error;

            var comparedDudes = GetComparedDudes(chatDudes.Value);

            return comparedDudes;
        }

        private async Task<Result<IReadOnlyCollection<ChatMember>, ComparedDudesErrors>> GetChatDudesAsync(ComparingDudes? comparingDudes)
        {
            if (comparingDudes is null)
            {
                return ComparedDudesErrors.UnknownChatDudes;
            }
            var preparedChatMembers = comparingDudes.DudesUserNames
                                                    .Select(async userName => await _store.GetChatMemberAsync(comparingDudes.ChatId, userName));
            var foundChatMembers = await Task.WhenAll(preparedChatMembers);
            var notChatMembers = foundChatMembers.Where(x => !x.IsMember).ToArray();
            var areThereNotChatMembers = notChatMembers.Any();

            var haveNoChatMembers = notChatMembers.Length == foundChatMembers.Length;
            if (haveNoChatMembers)
            {
                return ComparedDudesErrors.EmptyChatDudes;
            }

            if (!areThereNotChatMembers)
                return foundChatMembers;
            
            var unknownChatDudes = notChatMembers.Select(x => x.User.Username).ToArray();

            return ComparedDudesErrors.GetUnknownChatDudes(unknownChatDudes);
        }

        private ComparedDudes GetComparedDudes(IEnumerable<ChatMember> chatDudes)
        {
            var dudesCocks = chatDudes.Select(x => (CockSizeInfo: _cockSizerCache.GetCheckedUser(x.User.UserId), ChatMember: x))
                                      .Where(x => x.CockSizeInfo != null);
            var comparedDudes = dudesCocks.OrderByDescending(
                                              x =>
                                              {
                                                  Debug.Assert(x.CockSizeInfo != null, "x.CockSizeInfo != null");
                                                  return x.CockSizeInfo.CockSize.Size;
                                              })
                                          .Select(
                                              (x, i) =>
                                              {
                                                  Debug.Assert(x.CockSizeInfo != null, "x.CockSizeInfo != null");
                                                  return new DudeInfo
                                                  {
                                                      DudeType = GetDudeType(i),
                                                      CockSize = x.CockSizeInfo.CockSize,
                                                      CheckedDude = x.ChatMember.User
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
        public CockSize CockSize { get; init; } = null!;

        public DudeTypes DudeType { get; init; }

        public CheckedDude CheckedDude { get; init; } = null!;
    }

    public enum DudeTypes
    {
        Loser,

        Winner
    }
}