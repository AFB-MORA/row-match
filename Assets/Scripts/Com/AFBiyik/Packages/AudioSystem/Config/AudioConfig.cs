using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Audio configurations. 
    /// </summary>
    public class AudioConfig : ScriptableObject {
        /// <summary>
        /// 2D <see cref="IFxSource"/>
        /// </summary>
        [SerializeField]
        protected Object soundSource2d; // Hack: unity cannot serialize interfaces. So we use Object.

        /// <summary>
        /// 3D <see cref="IFxSource"/>
        /// </summary>
        [SerializeField]
        protected Object soundSource3d; // Hack: unity cannot serialize interfaces. So we use Object.

        /// <summary>
        /// Music <see cref="IFxSource"/>
        /// </summary>
        [SerializeField]
        protected Object musicSource; // Hack: unity cannot serialize interfaces. So we use Object.

        /// <summary>
        /// Music configurations.
        /// </summary>
        [SerializeField]
        protected MusicConfig musicConfig;

        /// <summary>
        /// 2D source.
        /// </summary>
        public IFxSource SoundSource2d => soundSource2d as IFxSource;

        /// <summary>
        /// 3D source.
        /// </summary>
        public IFxSource SoundSource3d => soundSource3d as IFxSource;

        /// <summary>
        /// Music source.
        /// </summary>
        public IFxSource MusicSource => musicSource as IFxSource;

        /// <summary>
        /// Music configurations.
        /// </summary>
        public MusicConfig MusicConfig => musicConfig;

        /// <summary>
        /// Validates SerializeFields.
        /// </summary>
        protected virtual void OnValidate() {
            ValidateObject(ref soundSource2d);
            ValidateObject(ref soundSource3d);
            ValidateObject(ref musicSource);
        }

        /// <summary>
        /// Validates is object is IFxSource or not.
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void ValidateObject(ref Object obj) {
            if (obj == null) {
                return;
            }

            if (typeof(IFxSource).IsAssignableFrom(obj.GetType())) {
                return;
            }

            if (obj is GameObject go) {
                var comp = go.GetComponent(typeof(IFxSource));

                if (comp == null) {
                    obj = null;
                    Debug.LogWarning("GameObject does not contain IFxSource");
                }
                else {
                    obj = comp;
                }
                return;
            }

            Debug.LogWarning("Object does not contain IFxSource");
            obj = null;
        }
    }
}