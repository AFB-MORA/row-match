using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.PopupSystem
{
    /// <summary>
    /// Base animated popup class for open/close animations
    /// </summary>
    public abstract class BaseAnimatedPopup : BasePopup, IAnimatedPopup
    {
        // Serialize fileds

        [SerializeField]
        protected Animator animator;

        // Private Properties

        private PopupAnimationBehaviour openTrigger;
        private PopupAnimationBehaviour closeTrigger;

        // Protected Properties

        /// <summary>
        /// Open animation state name
        /// </summary>
        protected virtual string OpenState => "Open";

        /// <summary>
        /// Close animation state name
        /// </summary>
        protected virtual string CloseState => "Close";

        /// <summary>
        /// Open state trigger
        /// </summary>
        protected virtual PopupAnimationBehaviour OpenTrigger
        {
            get
            {
                if (openTrigger == null)
                {
                    openTrigger = animator.GetBehaviourAtState<PopupAnimationBehaviour>(OpenState);
                }
                return openTrigger;
            }
        }

        /// <summary>
        /// Close state trigger
        /// </summary>
        protected virtual PopupAnimationBehaviour CloseTrigger
        {
            get
            {
                if (closeTrigger == null)
                {
                    closeTrigger = animator.GetBehaviourAtState<PopupAnimationBehaviour>(CloseState);
                }
                return closeTrigger;
            }
        }

        // Public Methods

        /// <inheritdoc/>
        public virtual async UniTask PlayOpenAnimation()
        {
            try
            {
                animator.Play(OpenState);
                await OpenTrigger.OnStateExitAsObservable().First().ToUniTask();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        /// <inheritdoc/>
        public virtual async UniTask PlayCloseAnimation()
        {
            try
            {
                animator.Play(CloseState);
                await CloseTrigger.OnStateExitAsObservable().First().ToUniTask();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}
