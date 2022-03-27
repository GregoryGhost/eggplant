namespace Lagalike.Demo.Eggplant.MVU.Services.Views.Staff
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Lagalike.Demo.Eggplant.MVU.Models;

    public record EmoutionInfo
    {
        public string Reaction { get; init; } = null!;
    }

    public class EmotionBotReactionsHandler
    {
        private readonly string[] _cockSizeEmoutions =
        {
            "໒( 0◡0)っ✂╰⋃╯", "🚷", "😭", "🤣", "😂", "🥲", "🙃", "😔", "🤏", "😟", "😕", "🙁", "😣", "😖", "🙂", "🙃", "👍", "🙌",
            "👏", "🎉",
            "👄", "😘", "🥰", "😍", "😳", "😅", "😬", "😥", "😰", "😨", "😱", "🍆", "໒( 0◡0)っ✂╰⋃╯", "🦍", "🐎", "🐘", "🦣", "🏆",
            "(●≧ﻬ≦)(˘ ε ˘ʃƪ)",
            "🚀", "(╯°□°）╯︵ ┻━┻", "(ó ì_í)=óò=(ì_í ò)",
            "/╲/\\╭( ͡° ͡° ͜ʖ ͡° ͡°)╮/]\\╱\\", "ಠ╭╮ಠ", "╾━╤デ╦︻( ▀̿ Ĺ̯ ▀̿├┬┴┬", "щ(ಠ益ಠщ)", "You need to ໒( 0◡0)っ✂╰⋃╯",
            "つ ◕_◕༽つ", "(づ´༎ຶU´༎ຶ)づᕙ(ᴗ ͟لᴗ)ᕗ", "┌(  ಠ_ಠ )┘"
        };

        private readonly IReadOnlyDictionary<CockSize, EmoutionInfo> _dictCockSizeReactions;

        public EmotionBotReactionsHandler()
        {
            var cockSizes = GetCockSizes();

            _dictCockSizeReactions = cockSizes.Zip(
                                                  _cockSizeEmoutions,
                                                  (cockSize, emoution) =>
                                                  {
                                                      var reaction = new EmoutionInfo
                                                      {
                                                          Reaction = emoution
                                                      };
                                                      return (CockSize: cockSize, Reaction: reaction);
                                                  })
                                              .ToDictionary(x => x.CockSize, x => x.Reaction);
        }

        public EmoutionInfo GetBotEmoution(CockSize cockSize)
        {
            return _dictCockSizeReactions[cockSize];
        }

        private IReadOnlyList<CockSize> GetCockSizes()
        {
            var cockSizes = CockSize.Range();
            var availableEmoutionsCount = _cockSizeEmoutions.Length;
            var needEmoutionsCount = cockSizes.Count;
            var hasEnough = availableEmoutionsCount >= needEmoutionsCount;
            if (!hasEnough)
            {
                var exceptionMsg =
                    "Doesn't matched cock sizes to available emoutions. Need to get more emoutions." +
                    $" Available({availableEmoutionsCount})/Need({needEmoutionsCount})";
                throw new IndexOutOfRangeException(exceptionMsg);
            }

            return cockSizes;
        }
    }
}