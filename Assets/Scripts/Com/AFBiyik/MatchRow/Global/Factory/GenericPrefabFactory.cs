using UnityEngine;
using Zenject;

namespace Com.AFBiyik.MatchRow.Factory {
    /// <summary>
    /// Generic factory for prefabs.
    ///
    /// <example>
    /// <h3>Installer:</h3>
    ///
    /// <code>
    /// using Morautils.Factory.Prefab;
    /// using Zenject;
    /// 
    /// public class ExampleInstaller : MonoInstaller {
    ///     public override void InstallBindings() {
    ///         Container.BindInterfacesTo&lt;GenericPrefabFactory&lt;Foo&gt;&gt;()
    ///         .AsSingle();
    ///     }
    /// }
    /// </code>
    ///
    /// <h3>Create prefabs:</h3>
    ///
    /// <code>
    /// [Inject]
    /// private IFactory&lt;Foo, Foo&gt; fooFactory;
    /// 
    /// [SerializeField]
    /// private Foo prefab
    /// 
    /// public void Spawn() {
    ///     Foo instance = fooFactory.Create(prefab);
    /// }
    /// </code>
    /// 
    /// Or;
    /// 
    /// <code>
    /// [Inject]
    /// private IFactory&lt;Foo, Transform, Foo&gt; fooFactoryWithParent;
    /// 
    /// [SerializeField]
    /// private Transform parent
    /// [SerializeField]
    /// private Foo prefab
    /// 
    /// public void Spawn() {
    ///     Foo instance = fooFactoryWithParent.Create(prefab, parent);
    /// }
    /// </code>
    /// 
    /// </example>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericPrefabFactory<T> : IFactory<T, T>, IFactory<T, Transform, T> where T : Behaviour {
        // Dependencies
        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected DiContainer container;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public virtual T Create(T prefab) {
            T view = container.InstantiatePrefabForComponent<T>(prefab.gameObject);
            return view;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public virtual T Create(T prefab, Transform parent) {
            T view = container.InstantiatePrefabForComponent<T>(prefab.gameObject, parent);
            return view;
        }
    }
}
