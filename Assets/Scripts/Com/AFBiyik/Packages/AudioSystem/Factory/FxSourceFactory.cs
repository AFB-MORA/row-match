using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Factory to create <see cref="IFxSource"/>.
    /// </summary>
    public class FxSourceFactory : IFactory<IFxSource> {
        /// <summary>
        /// Parent for instantiated prefab.
        /// </summary>
        protected readonly Transform parent;

        /// <summary>
        /// 
        /// </summary>
        protected readonly DiContainer container;

        /// <summary>
        /// Prefab to instantiate.
        /// </summary>
        protected readonly IFxSource prefab;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="parent">Parent for instantiated prefab.</param>
        /// <param name="prefab">Prefab to instantiate.</param>
        public FxSourceFactory(DiContainer container, Transform parent, IFxSource prefab) {
            this.container = container;
            this.parent = parent;
            this.prefab = prefab;
        }

        /// <summary>
        /// Create new <see cref="IFxSource"/> in the <see cref="parent"/>.
        /// </summary>
        /// <returns>Instantiated prefab</returns>
        public virtual IFxSource Create() {
            var source = container.InstantiatePrefabForComponent<IFxSource>(prefab.gameObject, parent);
            return source;
        }
    }
}