using UnityEngine;

namespace Com.AFBiyik.MatchRow.LevelScene.Input
{
    /// <summary>
    /// Swipe model
    /// </summary>
    public class SwipeModel
    {
        /// <summary>
        /// Swipe direction
        /// </summary>
        public SwipeDirection Direction { get; private set; }

        /// <summary>
        /// Swipe start position
        /// </summary>
        public Vector3 StartPosition { get; private set; }

        /// <summary>
        /// Creates swipe model
        /// </summary>
        /// <param name="direction">Swipe direction</param>
        /// <param name="startPosition">Swipe start position</param>
        public SwipeModel(SwipeDirection direction, Vector3 startPosition)
        {
            Direction = direction;
            StartPosition = startPosition;
        }
    }
}
