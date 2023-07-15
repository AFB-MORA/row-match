using System.Linq;
using UnityEngine;

namespace Com.AFBiyik.PopupSystem
{
    public static class AnimatorExtensions
    {
        /// <summary>
        /// Gets PopupAnimationBehaivour with state name filter.
        /// </summary>
        /// <typeparam name="T">PopupAnimationBehaivour</typeparam>
        /// <param name="animator"></param>
        /// <param name="name"><see cref="behaviour.StateName"/></param>
        /// <returns></returns>
        public static T GetBehaviourAtState<T>(this Animator animator, string name) where T : PopupAnimationBehaviour
        {
            return animator.GetBehaviours<T>().FirstOrDefault(behaviour => behaviour.StateName == name);
        }
    }
}
