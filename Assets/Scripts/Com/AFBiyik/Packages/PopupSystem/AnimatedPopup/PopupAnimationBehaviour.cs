using UniRx.Triggers;
using UnityEngine;

namespace Com.AFBiyik.PopupSystem
{
    public class PopupAnimationBehaviour : ObservableStateMachineTrigger
    {
        [SerializeField]
        protected string stateName;

        /// <summary>
        /// State name to filter
        /// </summary>
        public string StateName => stateName;
    }
}
