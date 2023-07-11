
using System;
using Com.AFBiyik.MatchRow.Util;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.AFBiyik.MatchRow.GameScene.Input
{
    /// <summary>
    /// Detects and controls swipe input
    /// </summary>
    public class SwipeManager : ISwipeEvent, IDisposable
    {
        // Constants
        private const float MIN_DISTANCE = 0.4f;

        // Private Readonly Fields
        private readonly GameControls gameControls;
        private readonly Subject<SwipeModel> onSwipe;

        // Private Fields
        private Vector3 startPosition;
        private IDisposable swipeDetection;

        // Public Properties
        /// <inheritdoc/>
        public IObservable<SwipeModel> OnSwipe => onSwipe;

        /// <summary>
        /// Creates swipe manager.
        /// </summary>
        public SwipeManager()
        {
            // Create subject
            onSwipe = new Subject<SwipeModel>();
            // Create controls
            gameControls = new GameControls();
            gameControls.Enable();

            // Subscribe input events
            gameControls.Swipe.PrimaryContact.started += StartTouch;
            gameControls.Swipe.PrimaryContact.canceled += EndTouch;
        }

        public void Dispose()
        {
            gameControls.Dispose();
            DisposeDetection();
        }

        /// <summary>
        /// Called when touch stars
        /// </summary>
        /// <param name="ctx"></param>
        private async void StartTouch(InputAction.CallbackContext ctx)
        {
            // Hack: To get correct position wait 1 frame.
            // Unity new input system doesn't give correct screen position.
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate); 

            // Get position
            startPosition = GetWorldPosition();

            // Create detection
            swipeDetection?.Dispose();
            swipeDetection = Observable
                .EveryGameObjectUpdate()
                .Subscribe(_ => DetectSwipe());

        }

        /// <summary>
        /// Called when touch ends
        /// </summary>
        /// <param name="ctx"></param>
        private void EndTouch(InputAction.CallbackContext ctx)
        {
            // Dispose detection
            DisposeDetection();
        }

        /// <summary>
        /// Disposes detection.
        /// </summary>
        private void DisposeDetection()
        {
            // Dispose swipe detection
            swipeDetection?.Dispose();
            swipeDetection = null;
        }

        /// <summary>
        /// Called during touch. Checks swipe.
        /// </summary>
        private void DetectSwipe()
        {
            // Get last position
            var lastPosition = GetWorldPosition();

            // Check if the last position is more than or equal to min distance
            if (Vector3.Distance(startPosition, lastPosition) >= MIN_DISTANCE)
            {
                // Dispose detection
                DisposeDetection();
                // Find direction
                Vector2 direction = (lastPosition - startPosition).normalized;
                FindDirection(direction);
            }
        }

        /// <summary>
        /// Finds swipe direction.
        /// </summary>
        /// <param name="direction">Vector from start to end</param>
        private void FindDirection(Vector3 direction)
        {
            // Check if horizontal or not
            // Horizontal
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Right
                if (direction.x > 0)
                {
                    onSwipe.OnNext(new SwipeModel(SwipeDirection.Right, startPosition));
                }
                // Left
                else
                {
                    onSwipe.OnNext(new SwipeModel(SwipeDirection.Left, startPosition));
                }
            }
            // Vertical
            else
            {
                // Up
                if (direction.y > 0)
                {
                    onSwipe.OnNext(new SwipeModel(SwipeDirection.Up, startPosition));
                }
                // Down
                else
                {
                    onSwipe.OnNext(new SwipeModel(SwipeDirection.Down, startPosition));
                }
            }
        }

        /// <summary>
        /// Gets touch position in world space.
        /// </summary>
        /// <returns>Touch position in world space</returns>
        private Vector3 GetWorldPosition()
        {
            var screenPos = gameControls.Swipe.PrimaryPosition.ReadValue<Vector2>();
            var worldPos = screenPos.ScreenToWorldPosition();
            worldPos.z = 0;
            return worldPos;
        }
    }
}