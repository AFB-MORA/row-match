using System;
using UnityEngine.U2D;

namespace Com.AFBiyik.AssetSystem
{
    /// <summary>
    /// Loads addressable system atlases.
    /// </summary>
    public class AtlasLoader : IAtlasLoader
    {
        /// <summary>
        /// Asset loader to load atlases.
        /// </summary>
        protected readonly IAssetLoader assetLoader;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetLoader">Asset loader to load atlases.</param>
        public AtlasLoader(IAssetLoader assetLoader)
        {
            this.assetLoader = assetLoader;
            SpriteAtlasManager.atlasRegistered += (spriteAtlas =>
            {
                // this gets called anytime a sprite sheet is loaded explictly
                //UnityEngine.Debug.Log("spriteAtlas  loaded ???: " + spriteAtlas.name);
            });
            SpriteAtlasManager.atlasRequested += LoadAtlas;
        }

        /// <summary>
        /// Loads atlas if atlas not found in build.
        /// <br/>
        /// Loads atlas from addressable system.
        /// </summary>
        /// <param name="name">Name of the atlas</param>
        /// <param name="bindAction">Action to call when loaded</param>
        protected virtual async void LoadAtlas(string name, Action<SpriteAtlas> bindAction)
        {
            var atlas = await assetLoader.LoadAsset<SpriteAtlas>(name);
            bindAction?.Invoke(atlas);
        }
    }
}