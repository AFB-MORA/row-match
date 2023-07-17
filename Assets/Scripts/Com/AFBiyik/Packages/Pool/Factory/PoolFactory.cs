using UnityEngine;
using Zenject;

namespace Com.AFBiyik.Pool
{
    /// <summary>
    /// Factory for pooling.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PoolFactory<T> : IFactory<T> where T : Component
    {
        /// <summary>
        /// DI container
        /// </summary>
        protected readonly DiContainer container;

        /// <summary>
        /// Prefab to create
        /// </summary>
        protected readonly T prefab;

        /// <summary>
        /// Parent for created objects
        /// </summary>
        protected readonly Transform poolParent;

        /// <summary>
        /// Do not use constructor directly.
        /// <br/>
        /// <example>
        /// Bind with prefab and parent.
        /// <code>
        /// Container.Bind&lt;IFactory&lt;TPrefab&gt;&gt;()
        ///     .To&lt;PoolFactory&lt;TPrefab&gt;&gt;()
        ///     .FromNew()
        ///     .AsSingle()
        ///     .WithArguments(prefab, poolParent);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="prefab">Prefab to create</param>
        /// <param name="poolParent">Parent for created objects</param>
        public PoolFactory(DiContainer container, T prefab, Transform poolParent)
        {
            this.container = container;
            this.prefab = prefab;
            this.poolParent = poolParent;
        }

        /// <summary>
        /// Creates new instance from prefab.
        /// </summary>
        /// <returns>Instantiated prefab</returns>
        public virtual T Create()
        {
            return container.InstantiatePrefabForComponent<T>(prefab, poolParent);
        }
    }
}
