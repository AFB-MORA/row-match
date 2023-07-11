using Com.AFBiyik.MatchRow.GameScene.Enumeration;

namespace Com.AFBiyik.MatchRow.GameScene.Util
{
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts item code into <see cref="ItemType"/>.
        /// <br/>
        /// "r" => <see cref="ItemType.Red"/>
        /// <br/>
        /// "g" => <see cref="ItemType.Green"/>
        /// <br/>
        /// "b" => <see cref="ItemType.Blue"/>
        /// <br/>
        /// "y" => <see cref="ItemType.Yellow"/>
        /// <br/>
        /// fallback => Yellow
        /// </summary>
        /// <param name="item">Item code "r", "g", "b", or "y"</param>
        /// <returns>Item Type</returns>
        public static ItemType ToItemType(this string item)
        {
            switch (item)
            {
                case "r":
                    return ItemType.Red;
                case "g":
                    return ItemType.Green;
                case "b":
                    return ItemType.Blue;
                case "y":
                default:
                    return ItemType.Yellow;
            }
        }
    }
}