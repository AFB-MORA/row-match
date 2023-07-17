using System;

namespace Com.AFBiyik.MatchRow.LevelScene.Input
{
    /// <summary>
    /// Observable swipe event
    /// </summary>
    public interface ISwipeEvent
    {
        /// <summary>
        /// Invoked when swipe detected.
        /// </summary>
        IObservable<SwipeModel> OnSwipe { get; }
    }
}