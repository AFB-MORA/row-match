using System;
using UnityEngine.ResourceManagement.ResourceLocations;
using Zenject;

namespace Com.AFBiyik.AssetSystem
{
    /// <summary>
    /// Installs asset system.
    /// <br/>
    /// <example>
    /// Install by:
    /// <code>
    /// #if UNITY_ANDROID
    /// private const string url = &quot;https://android.cdn.com/?path=&quot;;
    /// #elif UNITY_IOS          
    /// private const string url = &quot;https://ios.cdn.com/?path=&quot;;
    /// #endif
    /// 
    /// public override void InstallBindings() {
    ///     // TransformFuntion: Addressable id transform function
    ///     // Use Cases: Change remote url depends on platform (i.e. iOS / Adnroid).
    ///     // Change remote url depends on environment (i.e. Production / Test)
    ///     AssetSystemInstaller.Install(Container, TransformFuntion);
    /// }
    /// 
    /// private string TransformFuntion(IResourceLocation location) {
    ///     if (location.InternalId.StartsWith(&quot;replace_cdn&quot;)) {
    ///         if (url != null) {
    ///              return location.InternalId.Replace(&quot;replace_cdn&quot;, url);
    ///         }
    ///     }
    ///     return location.InternalId;
    /// }
    /// </code>
    /// Or
    /// <code>
    /// public override void InstallBindings() {
    ///     AssetSystemInstaller.Install(Container, null);
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class AssetSystemInstaller : Installer<Func<IResourceLocation, string>, AssetSystemInstaller>
    {
        /// <summary>
        /// To change addressable id.
        /// </summary>
        protected readonly Func<IResourceLocation, string> transformAddressableId;

        /// <summary>
        /// Do not use constructor directly. Instead use Install function.
        /// </summary>
        /// <param name="transformAddressableId">Optional. To change addressable id.</param>
        public AssetSystemInstaller(Func<IResourceLocation, string> transformAddressableId = null)
        {
            this.transformAddressableId = transformAddressableId;
        }

        /// <summary>
        /// Installs asset system.
        /// </summary>
        public override void InstallBindings()
        {
            if (transformAddressableId != null)
            {
                Container.BindInterfacesTo<AssetLoader>()
                    .AsSingle()
                    .WithArguments(transformAddressableId)
                    .NonLazy();
            }
            else
            {
                Container.BindInterfacesTo<AssetLoader>()
                    .AsSingle()
                    .NonLazy();
            }

            Container.BindInterfacesTo<AtlasLoader>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<AssetManager>()
                .AsSingle()
                .NonLazy();
        }
    }
}