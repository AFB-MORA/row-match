using Com.AFBiyik.MatchRow.GameScene.Presenter;
using Com.AFBiyik.MatchRow.Global.Util;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.GameScene.View
{
    /// <summary>
    /// Displays score
    /// </summary>
    public abstract class BaseValueView : MonoBehaviour
    {
        // Constants
        protected float TWEEN_TIME = 0.5f;

        // Serialize Fields
        [SerializeField]
        protected TMP_Text valueText;
        [SerializeField]
        private bool tweenValueUpdates = true;

        // Dependencies
        [Inject]
        protected IGamePresenter gamePresenter;

        // Private Fields
        protected bool isInitial;
        protected int value;

        protected virtual void Awake()
        {
            isInitial = true;
            SubscribeValue();
        }

        /// <summary>
        /// Subscrive value changes.
        /// Call <see cref="OnValueChange"/> method for value changes.
        /// </summary>
        protected abstract void SubscribeValue();

        /// <summary>
        /// Called when value changes.
        /// </summary>
        /// <param name="newValue"></param>
        protected virtual async void OnValueChange(int newValue)
        {
            await OnValueChangeAsync(newValue);
        }

        /// <summary>
        /// Called when value changes.
        /// </summary>
        /// <param name="newValue"></param>
        protected virtual async UniTask OnValueChangeAsync(int newValue)
        {
            if (isInitial || !tweenValueUpdates)
            {
                isInitial = false;
                value = newValue;
                // Set score
                valueText.text = newValue.ToString();
                return;
            }

            await NumberTween.TweenInt(value, newValue, TWEEN_TIME, (value) =>
            {
                valueText.text = value.ToString();
            });

            value = newValue;
            valueText.text = value.ToString();
        }
    }
}