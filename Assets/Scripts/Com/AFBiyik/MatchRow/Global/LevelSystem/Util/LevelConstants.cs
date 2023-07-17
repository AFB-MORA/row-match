using System.IO;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.Global.LevelSystem
{
    internal static class LevelConstants
    {
        public const int NUMBER_OF_LEVELS = 25;
        public const int PRELOADED_LEVELS = 10;
        public const int NUMBER_OF_A_LEVELS = 15;
        public const int NUMBER_OF_B_LEVELS = 10;
        public const string BASE_URL = "https://row-match.s3.amazonaws.com/levels/RM_";

        public static string GetLevelPath(int level)
        {
            return Path.Join(Application.persistentDataPath,
                "Levels",
                $"Level_{level}");
        }

        public static string GetLevelUrl(int level)
        {
            var url = BASE_URL;

            if (level <= NUMBER_OF_A_LEVELS)
            {
                url += "A" + level;
            }
            else
            {
                url += "B" + (level - NUMBER_OF_A_LEVELS);
            }

            return url;
        }
    }
}
