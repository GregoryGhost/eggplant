namespace CockSizer.Services
{
    using CockSizer.Models;

    public class EmotionBotReactionsHandler
    {
        private const string ZERO_COCK_SIZE_EMOUTION =
            ".∧＿∧" +
            "( ･ω･｡)つ━☆・*。" +
            "⊂. ノ ...・゜+. " +
            "しーＪ...°。+ *´¨)" +
            "..........· ´¸.·*´¨) ¸.·*¨)" +
            "..........(¸.·´ (¸.·'* ☆ WOW AND YOU ARE A FAG ☆";

        private readonly string[] _cockSizeEmoutions =
        {
            ZERO_COCK_SIZE_EMOUTION, "🚷", "😭", "🤣", "😂", "🥲", "🙃", "😔", "🤏", "😟", "😕", "🙁", "😣", "😖", "🙂", "🙃",
            "👍", "🙌",
            "👏", "🎉",
            "👄", "😘", "🥰", "😍", "😳", "😅", "😬", "😥", "😰", "😨", "😱", "🍆", "໒( 0◡0)っ✂╰⋃╯", "🦍", "🐎", "🐘", "🦣", "🏆",
            "(●≧ﻬ≦)(˘ ε ˘ʃƪ)",
            "🚀", "(╯°□°）╯︵ ┻━┻", "(ó ì_í)=óò=(ì_í ò)",
            "/╲/\\╭( ͡° ͡° ͜ʖ ͡° ͡°)╮/]\\╱\\", "ಠ╭╮ಠ", "╾━╤デ╦︻( ▀̿ Ĺ̯ ▀̿├┬┴┬", "щ(ಠ益ಠщ)", "You need to ໒( 0◡0)っ✂╰⋃╯",
            "つ ◕_◕༽つ", "(づ´༎ຶU´༎ຶ)づᕙ(ᴗ ͟لᴗ)ᕗ", "┌(  ಠ_ಠ )┘"
        };

        private readonly CockSizeFactory _cockSizeFactory;

        private readonly IReadOnlyDictionary<CockSize, EmoutionInfo> _dictCockSizeReactions;

        public EmotionBotReactionsHandler(CockSizeFactory cockSizeFactory)
        {
            _cockSizeFactory = cockSizeFactory;
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
            var cockSizes = _cockSizeFactory.GetCockSizeRanges();
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