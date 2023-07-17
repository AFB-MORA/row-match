using UnityEngine;
using Zenject;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Pool to spawn <see cref="IFxSource"/>.
    /// </summary>
    public class SoundFxPool : MemoryPool<IFxSource> {
        /// <summary>
        /// Original parent for despawned sources.
        /// </summary>
        protected Transform originalParent;

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public SoundFxPool() {
        }

        /// <summary>
        /// Called when source created.
        /// </summary>
        /// <param name="item">Newly created source.</param>
        protected override void OnCreated(IFxSource item) {
            item.Initialize(this);
            item.gameObject.SetActive(false);
            originalParent = item.transform.parent;
        }

        /// <summary>
        /// Called when source needed to be destroyed.
        /// </summary>
        /// <param name="item">Source to destroy.</param>
        protected override void OnDestroyed(IFxSource item) {
            GameObject.Destroy(item.gameObject);
        }

        /// <summary>
        /// Called when source despawned.
        /// </summary>
        /// <param name="item">Despawned source.</param>
        protected override void OnDespawned(IFxSource item) {
            item.OnDespawned();
            item.gameObject.SetActive(false);

            if (item.transform.parent != originalParent) {
                item.transform.SetParent(originalParent, false);
            }
        }

        /// <summary>
        /// Called when source spawned.
        /// </summary>
        /// <param name="item">Spawned source.</param>
        protected override void Reinitialize(IFxSource item) {
            item.gameObject.SetActive(true);
            item.OnSpawned();
        }
    }
}
