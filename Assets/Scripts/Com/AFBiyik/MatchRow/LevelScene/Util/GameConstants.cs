using Com.AFBiyik.MatchRow.LevelScene.Enumeration;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.LevelScene.Util
{
    public static class GameConstants
    {
        public const int RED_SCORE = 100;
        public const int GREEN_SCORE = 150;
        public const int BLUE_SCORE = 200;
        public const int YELLOW_SCORE = 250;

        public static readonly Color RED_FROM_COLOR = new Color(0.6705883f, 0.1372549f, 0.1490196f);
        public static readonly Color RED_TO_COLOR = new Color(0.9137255f, 0.254902f, 0.2117647f);
        public static readonly Color GREEN_FROM_COLOR = new Color(0.2705882f, 0.7176471f, 0.254902f);
        public static readonly Color GREEN_TO_COLOR = new Color(0.3411765f, 0.8901961f, 0.254902f);
        public static readonly Color BLUE_FROM_COLOR = new Color(0.2039216f, 0.5176471f, 0.9843137f);
        public static readonly Color BLUE_TO_COLOR = new Color(0.3058824f, 0.6941177f, 0.9921569f);
        public static readonly Color YELLOW_FROM_COLOR = new Color(0.9882353f, 0.9176471f, 0.3647059f);
        public static readonly Color YELLOW_TO_COLOR = new Color(0.9764706f, 0.7019608f, 0.2078431f);

        /// <summary>
        /// Get score for item type
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns>Item score</returns>
        public static int GetScore(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Red:
                    return RED_SCORE;
                case ItemType.Green:
                    return GREEN_SCORE;
                case ItemType.Blue:
                    return BLUE_SCORE;
                case ItemType.Yellow:
                    return YELLOW_SCORE;
            }

            return 0;
        }

        /// <summary>
        /// Get colors for item type particle system
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns>Particle system colors</returns>
        public static (Color from, Color to) GetColors(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Red:
                    return (RED_FROM_COLOR, RED_TO_COLOR);
                case ItemType.Green:
                    return (GREEN_FROM_COLOR, GREEN_TO_COLOR);
                case ItemType.Blue:
                    return (BLUE_FROM_COLOR, BLUE_TO_COLOR);
                case ItemType.Yellow:
                    return (YELLOW_FROM_COLOR, YELLOW_TO_COLOR);
            }
            return (Color.white, Color.white);
        }
    }
}
