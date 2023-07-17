using System;
using UnityEngine;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Music configurations.
    /// </summary>
    public class MusicConfig : ScriptableObject {
        /// <summary>
        /// Silent gap between musics.
        /// </summary>
        [SerializeField]
        protected float musicGap;

        /// <summary>
        /// Silent gap between musics.
        /// </summary>
        public float MusicGap => musicGap;
    }
}
