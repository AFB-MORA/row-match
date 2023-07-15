using UnityEngine;
using Zenject;

namespace Com.AFBiyik.Factory
{
    /// <summary>
    /// Generic factory for prefabs.
    ///
    /// <example>
    /// <h3>Installer:</h3>
    ///
    /// <code>
    /// public class ExampleInstaller : MonoInstaller {
    ///     public override void InstallBindings() {
    ///         Container.BindInterfacesTo&lt;PrefabFactory&gt;()
    ///             .AsSingle();
    ///     }
    /// }
    /// </code>
    ///
    /// <h3>Create prefabs:</h3>
    ///
    /// <code>
    /// [Inject]
    /// private IFactory&lt;GameObject, GameObject&gt; prefabFactory;
    /// 
    /// [SerializeField]
    /// private GameObject prefab
    /// 
    /// public void Spawn() {
    ///     GameObject instance = prefabFactory.Create(prefab);
    /// }
    /// </code>
    /// 
    /// Or;
    /// 
    /// <code>
    /// [Inject]
    /// private IFactory&lt;GameObject, Transform, GameObject&gt; prefabFactoryWithParent;
    /// 
    /// [SerializeField]
    /// private Transform parent
    /// [SerializeField]
    /// private GameObject prefab
    /// 
    /// public void Spawn() {
    ///     GameObject instance = prefabFactoryWithParent.Create(prefab, parent);
    /// }
    /// </code>
    /// 
    /// </example>
    ///
    /// </summary>
    public class PrefabFactory : IFactory<GameObject, GameObject>, IFactory<GameObject, Transform, GameObject>
    {
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
        public virtual GameObject Create(GameObject prefab)
        {
            return container.InstantiatePrefab(prefab);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public virtual GameObject Create(GameObject prefab, Transform parent)
        {
            return container.InstantiatePrefab(prefab, parent);
        }
    }
}