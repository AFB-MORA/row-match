using UnityEngine;
using Zenject;

namespace Com.AFBiyik.Pool
{
    #region No Param
    /// <summary>
    /// Installs pool for prefab.
    /// <br/>
    /// Install by:
    /// <code>
    /// MonoPoolableMemoryPoolInstaller&lt;PrefabView&gt;.Install(Container, prefab, poolParent, settings)
    /// </code>
    /// <br/>
    /// Inject by:
    /// <code>
    /// [Inject]
    /// private IMemoryPool&lt;PrefabView&gt; prefabViewPool;
    ///
    /// PrefabView instance = prefabViewPool.Spawn();
    /// </code>
    /// </summary>
    /// <typeparam name="TPrefab">Prefab type</typeparam>
    public class MonoPoolableMemoryPoolInstaller<TPrefab> : MonoPoolableMemoryPoolInstallerBase<TPrefab, MonoPoolableMemoryPoolInstaller<TPrefab>> where TPrefab : Component, IPoolable
    {
        /// <inheritdoc/>
        public MonoPoolableMemoryPoolInstaller(TPrefab prefab, Transform poolParent, MemoryPoolSettings memoryPoolSettings) : base(prefab, poolParent, memoryPoolSettings)
        {
        }


        /// <inheritdoc/>
        public override void InstallBindings()
        {
            Container.Bind<IMemoryPool<TPrefab>>()
                .FromSubContainerResolve()
                .ByMethod(InstallPool)
                .AsCached();
        }

        /// <inheritdoc/>
        protected override void InstallPool(DiContainer subContainer)
        {
            InstallFactory(subContainer);

            subContainer.Bind<IMemoryPool<TPrefab>>()
                .To<MonoPoolableMemoryPool<TPrefab>>()
                .FromNew()
                .AsSingle()
                .WithArguments(memoryPoolSettings);
        }
    }
    #endregion

    #region 1 Param
    /// <summary>
    /// Installs pool for prefab.
    /// <br/>
    /// Install by:
    /// <code>
    /// MonoPoolableMemoryPoolInstaller&lt;Param1, PrefabView&gt;.Install(Container, prefab, poolParent, settings)
    /// </code>
    /// <br/>
    /// Inject by:
    /// <code>
    /// [Inject]
    /// private IMemoryPool&lt;Param1, PrefabView&gt; prefabViewPool;
    ///
    /// PrefabView instance = prefabViewPool.Spawn(parameter1);
    /// </code>
    /// </summary>
    /// <typeparam name="TParam1">Parameter 1</typeparam>
    /// <typeparam name="TPrefab">Prefab type</typeparam>
    public class MonoPoolableMemoryPoolInstaller<TParam1, TPrefab> : MonoPoolableMemoryPoolInstallerBase<TPrefab, MonoPoolableMemoryPoolInstaller<TParam1, TPrefab>> where TPrefab : Component, IPoolable<TParam1>
    {
        /// <inheritdoc/>
        public MonoPoolableMemoryPoolInstaller(TPrefab prefab, Transform poolParent, MemoryPoolSettings memoryPoolSettings) : base(prefab, poolParent, memoryPoolSettings)
        {
        }

        /// <inheritdoc/>
        public override void InstallBindings()
        {
            Container.Bind<IMemoryPool<TParam1, TPrefab>>()
                .FromSubContainerResolve()
                .ByMethod(InstallPool)
                .AsCached();
        }

        /// <inheritdoc/>
        protected override void InstallPool(DiContainer subContainer)
        {
            InstallFactory(subContainer);

            subContainer.Bind<IMemoryPool<TParam1, TPrefab>>()
                .To<MonoPoolableMemoryPool<TParam1, TPrefab>>()
                .FromNew()
                .AsSingle()
                .WithArguments(memoryPoolSettings);
        }
    }
    #endregion

    #region 2 Param
    /// <summary>
    /// Installs pool for prefab.
    /// <br/>
    /// Install by:
    /// <code>
    /// MonoPoolableMemoryPoolInstaller&lt;Param1, Param2, PrefabView&gt;.Install(Container, prefab, poolParent, settings)
    /// </code>
    /// <br/>
    /// Inject by:
    /// <code>
    /// [Inject]
    /// private IMemoryPool&lt;Param1, Param2, PrefabView&gt; prefabViewPool;
    ///
    /// PrefabView instance = prefabViewPool.Spawn(parameter1, parameter2);
    /// </code>
    /// </summary>
    /// <typeparam name="TParam1">Parameter 1</typeparam>
    /// <typeparam name="TParam2">Parameter 2</typeparam>
    /// <typeparam name="TPrefab">Prefab type</typeparam>
    public class MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TPrefab> : MonoPoolableMemoryPoolInstallerBase<TPrefab, MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TPrefab>> where TPrefab : Component, IPoolable<TParam1, TParam2>
    {
        /// <inheritdoc/>
        public MonoPoolableMemoryPoolInstaller(TPrefab prefab, Transform poolParent, MemoryPoolSettings memoryPoolSettings) : base(prefab, poolParent, memoryPoolSettings)
        {
        }

        /// <inheritdoc/>
        public override void InstallBindings()
        {
            Container.Bind<IMemoryPool<TParam1, TParam2, TPrefab>>()
                .FromSubContainerResolve()
                .ByMethod(InstallPool)
                .AsCached();
        }

        /// <inheritdoc/>
        protected override void InstallPool(DiContainer subContainer)
        {
            InstallFactory(subContainer);

            subContainer.Bind<IMemoryPool<TParam1, TParam2, TPrefab>>()
                .To<MonoPoolableMemoryPool<TParam1, TParam2, TPrefab>>()
                .FromNew()
                .AsSingle()
                .WithArguments(memoryPoolSettings);
        }
    }
    #endregion

    #region 3 Param
    /// <summary>
    /// Installs pool for prefab.
    /// <br/>
    /// Install by:
    /// <code>
    /// MonoPoolableMemoryPoolInstaller&lt;Param1, Param2, Param3, PrefabView&gt;.Install(Container, prefab, poolParent, settings)
    /// </code>
    /// <br/>
    /// Inject by:
    /// <code>
    /// [Inject]
    /// private IMemoryPool&lt;Param1, Param2, Param3, PrefabView&gt; prefabViewPool;
    ///
    /// PrefabView instance = prefabViewPool.Spawn(parameter1, parameter2, parameter3);
    /// </code>
    /// </summary>
    /// <typeparam name="TParam1">Parameter 1</typeparam>
    /// <typeparam name="TParam2">Parameter 2</typeparam>
    /// <typeparam name="TParam3">Parameter 3</typeparam>
    /// <typeparam name="TPrefab">Prefab type</typeparam>
    public class MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TPrefab> : MonoPoolableMemoryPoolInstallerBase<TPrefab, MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TPrefab>> where TPrefab : Component, IPoolable<TParam1, TParam2, TParam3>
    {
        /// <inheritdoc/>
        public MonoPoolableMemoryPoolInstaller(TPrefab prefab, Transform poolParent, MemoryPoolSettings memoryPoolSettings) : base(prefab, poolParent, memoryPoolSettings)
        {
        }

        /// <inheritdoc/>
        public override void InstallBindings()
        {
            Container.Bind<IMemoryPool<TParam1, TParam2, TParam3, TPrefab>>()
                .FromSubContainerResolve()
                .ByMethod(InstallPool)
                .AsCached();
        }

        /// <inheritdoc/>
        protected override void InstallPool(DiContainer subContainer)
        {
            InstallFactory(subContainer);

            subContainer.Bind<IMemoryPool<TParam1, TParam2, TParam3, TPrefab>>()
                .To<MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TPrefab>>()
                .FromNew()
                .AsSingle()
                .WithArguments(memoryPoolSettings);
        }
    }
    #endregion

    #region 4 Param
    /// <summary>
    /// Installs pool for prefab.
    /// <br/>
    /// Install by:
    /// <code>
    /// MonoPoolableMemoryPoolInstaller&lt;Param1, Param2, Param3, Param4, PrefabView&gt;.Install(Container, prefab, poolParent, settings)
    /// </code>
    /// <br/>
    /// Inject by:
    /// <code>
    /// [Inject]
    /// private IMemoryPool&lt;Param1, Param2, Param3, Param4, PrefabView&gt; prefabViewPool;
    ///
    /// PrefabView instance = prefabViewPool.Spawn(parameter1, parameter2, parameter3, parameter4);
    /// </code>
    /// </summary>
    /// <typeparam name="TParam1">Parameter 1</typeparam>
    /// <typeparam name="TParam2">Parameter 2</typeparam>
    /// <typeparam name="TParam3">Parameter 3</typeparam>
    /// <typeparam name="TParam4">Parameter 4</typeparam>
    /// <typeparam name="TPrefab">Prefab type</typeparam>
    public class MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TParam4, TPrefab> : MonoPoolableMemoryPoolInstallerBase<TPrefab, MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TParam4, TPrefab>> where TPrefab : Component, IPoolable<TParam1, TParam2, TParam3, TParam4>
    {
        /// <inheritdoc/>
        public MonoPoolableMemoryPoolInstaller(TPrefab prefab, Transform poolParent, MemoryPoolSettings memoryPoolSettings) : base(prefab, poolParent, memoryPoolSettings)
        {
        }

        /// <inheritdoc/>
        public override void InstallBindings()
        {
            Container.Bind<IMemoryPool<TParam1, TParam2, TParam3, TParam4, TPrefab>>()
                .FromSubContainerResolve()
                .ByMethod(InstallPool)
                .AsCached();
        }

        /// <inheritdoc/>
        protected override void InstallPool(DiContainer subContainer)
        {
            InstallFactory(subContainer);

            subContainer.Bind<IMemoryPool<TParam1, TParam2, TParam3, TParam4, TPrefab>>()
                .To<MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TPrefab>>()
                .FromNew()
                .AsSingle()
                .WithArguments(memoryPoolSettings);
        }
    }
    #endregion

    #region 5 Param
    /// <summary>
    /// Installs pool for prefab.
    /// <br/>
    /// Install by:
    /// <code>
    /// MonoPoolableMemoryPoolInstaller&lt;Param1, Param2, Param3, Param4, Param5, PrefabView&gt;.Install(Container, prefab, poolParent, settings)
    /// </code>
    /// <br/>
    /// Inject by:
    /// <code>
    /// [Inject]
    /// private IMemoryPool&lt;Param1, Param2, Param3, Param4, Param5, PrefabView&gt; prefabViewPool;
    ///
    /// PrefabView instance = prefabViewPool.Spawn(parameter1, parameter2, parameter3, parameter4, parameter5);
    /// </code>
    /// </summary>
    /// <typeparam name="TParam1">Parameter 1</typeparam>
    /// <typeparam name="TParam2">Parameter 2</typeparam>
    /// <typeparam name="TParam3">Parameter 3</typeparam>
    /// <typeparam name="TParam4">Parameter 4</typeparam>
    /// <typeparam name="TParam5">Parameter 5</typeparam>
    /// <typeparam name="TPrefab">Prefab type</typeparam>
    public class MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TPrefab> : MonoPoolableMemoryPoolInstallerBase<TPrefab, MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TPrefab>> where TPrefab : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5>
    {
        /// <inheritdoc/>
        public MonoPoolableMemoryPoolInstaller(TPrefab prefab, Transform poolParent, MemoryPoolSettings memoryPoolSettings) : base(prefab, poolParent, memoryPoolSettings)
        {
        }

        /// <inheritdoc/>
        public override void InstallBindings()
        {
            Container.Bind<IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TPrefab>>()
                .FromSubContainerResolve()
                .ByMethod(InstallPool)
                .AsCached();
        }

        /// <inheritdoc/>
        protected override void InstallPool(DiContainer subContainer)
        {
            InstallFactory(subContainer);

            subContainer.Bind<IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TPrefab>>()
                .To<MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TPrefab>>()
                .FromNew()
                .AsSingle()
                .WithArguments(memoryPoolSettings);
        }
    }
    #endregion

    #region 6 Param
    /// <summary>
    /// Installs pool for prefab.
    /// <br/>
    /// Install by:
    /// <code>
    /// MonoPoolableMemoryPoolInstaller&lt;Param1, Param2, Param3, Param4, Param5, Param6, PrefabView&gt;.Install(Container, prefab, poolParent, settings)
    /// </code>
    /// <br/>
    /// Inject by:
    /// <code>
    /// [Inject]
    /// private IMemoryPool&lt;Param1, Param2, Param3, Param4, Param5, Param6, PrefabView&gt; prefabViewPool;
    ///
    /// PrefabView instance = prefabViewPool.Spawn(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6);
    /// </code>
    /// </summary>
    /// <typeparam name="TParam1">Parameter 1</typeparam>
    /// <typeparam name="TParam2">Parameter 2</typeparam>
    /// <typeparam name="TParam3">Parameter 3</typeparam>
    /// <typeparam name="TParam4">Parameter 4</typeparam>
    /// <typeparam name="TParam5">Parameter 5</typeparam>
    /// <typeparam name="TParam6">Parameter 6</typeparam>
    /// <typeparam name="TPrefab">Prefab type</typeparam>
    public class MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TPrefab> : MonoPoolableMemoryPoolInstallerBase<TPrefab, MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TPrefab>> where TPrefab : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
    {
        /// <inheritdoc/>
        public MonoPoolableMemoryPoolInstaller(TPrefab prefab, Transform poolParent, MemoryPoolSettings memoryPoolSettings) : base(prefab, poolParent, memoryPoolSettings)
        {
        }

        /// <inheritdoc/>
        public override void InstallBindings()
        {
            Container.Bind<IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TPrefab>>()
                .FromSubContainerResolve()
                .ByMethod(InstallPool)
                .AsCached();
        }

        /// <inheritdoc/>
        protected override void InstallPool(DiContainer subContainer)
        {
            InstallFactory(subContainer);

            subContainer.Bind<IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TPrefab>>()
                .To<MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TPrefab>>()
                .FromNew()
                .AsSingle()
                .WithArguments(memoryPoolSettings);
        }
    }
    #endregion

    #region 7 Param
    /// <summary>
    /// Installs pool for prefab.
    /// <br/>
    /// Install by:
    /// <code>
    /// MonoPoolableMemoryPoolInstaller&lt;Param1, Param2, Param3, Param4, Param5, Param6, Param7, PrefabView&gt;.Install(Container, prefab, poolParent, settings)
    /// </code>
    /// <br/>
    /// Inject by:
    /// <code>
    /// [Inject]
    /// private IMemoryPool&lt;Param1, Param2, Param3, Param4, Param5, Param6, Param7, PrefabView&gt; prefabViewPool;
    ///
    /// PrefabView instance = prefabViewPool.Spawn(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7);
    /// </code>
    /// </summary>
    /// <typeparam name="TParam1">Parameter 1</typeparam>
    /// <typeparam name="TParam2">Parameter 2</typeparam>
    /// <typeparam name="TParam3">Parameter 3</typeparam>
    /// <typeparam name="TParam4">Parameter 4</typeparam>
    /// <typeparam name="TParam5">Parameter 5</typeparam>
    /// <typeparam name="TParam6">Parameter 6</typeparam>
    /// <typeparam name="TParam7">Parameter 7</typeparam>
    /// <typeparam name="TPrefab">Prefab type</typeparam>
    public class MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TPrefab> : MonoPoolableMemoryPoolInstallerBase<TPrefab, MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TPrefab>> where TPrefab : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>
    {
        /// <inheritdoc/>
        public MonoPoolableMemoryPoolInstaller(TPrefab prefab, Transform poolParent, MemoryPoolSettings memoryPoolSettings) : base(prefab, poolParent, memoryPoolSettings)
        {
        }

        /// <inheritdoc/>
        public override void InstallBindings()
        {
            Container.Bind<IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TPrefab>>()
                .FromSubContainerResolve()
                .ByMethod(InstallPool)
                .AsCached();
        }

        /// <inheritdoc/>
        protected override void InstallPool(DiContainer subContainer)
        {
            InstallFactory(subContainer);

            subContainer.Bind<IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TPrefab>>()
                .To<MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TPrefab>>()
                .FromNew()
                .AsSingle()
                .WithArguments(memoryPoolSettings);
        }
    }
    #endregion

    #region 8 Param
    /// <summary>
    /// Installs pool for prefab.
    /// <br/>
    /// Install by:
    /// <code>
    /// MonoPoolableMemoryPoolInstaller&lt;Param1, Param2, Param3, Param4, Param5, Param6, Param7, Param8, PrefabView&gt;.Install(Container, prefab, poolParent, settings)
    /// </code>
    /// <br/>
    /// Inject by:
    /// <code>
    /// [Inject]
    /// private IMemoryPool&lt;Param1, Param2, Param3, Param4, Param5, Param6, Param7, Param8, PrefabView&gt; prefabViewPool;
    ///
    /// PrefabView instance = prefabViewPool.Spawn(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8);
    /// </code>
    /// </summary>
    /// <typeparam name="TParam1">Parameter 1</typeparam>
    /// <typeparam name="TParam2">Parameter 2</typeparam>
    /// <typeparam name="TParam3">Parameter 3</typeparam>
    /// <typeparam name="TParam4">Parameter 4</typeparam>
    /// <typeparam name="TParam5">Parameter 5</typeparam>
    /// <typeparam name="TParam6">Parameter 6</typeparam>
    /// <typeparam name="TParam7">Parameter 7</typeparam>
    /// <typeparam name="TParam8">Parameter 8</typeparam>
    /// <typeparam name="TPrefab">Prefab type</typeparam>
    public class MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TPrefab> : MonoPoolableMemoryPoolInstallerBase<TPrefab, MonoPoolableMemoryPoolInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TPrefab>> where TPrefab : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8>
    {
        /// <inheritdoc/>
        public MonoPoolableMemoryPoolInstaller(TPrefab prefab, Transform poolParent, MemoryPoolSettings memoryPoolSettings) : base(prefab, poolParent, memoryPoolSettings)
        {
        }

        /// <inheritdoc/>
        public override void InstallBindings()
        {
            Container.Bind<IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TPrefab>>()
                .FromSubContainerResolve()
                .ByMethod(InstallPool)
                .AsCached();
        }

        /// <inheritdoc/>
        protected override void InstallPool(DiContainer subContainer)
        {
            InstallFactory(subContainer);

            subContainer.Bind<IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TPrefab>>()
                .To<MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TPrefab>>()
                .FromNew()
                .AsSingle()
                .WithArguments(memoryPoolSettings);
        }
    }
    #endregion
}