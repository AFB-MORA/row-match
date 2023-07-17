using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Com.AFBiyik.AssetSystem.Editor
{
    /// <summary>
    /// Editor helper for asset system.
    /// </summary>
    public static class AddressableExtensions
    {
        /// <summary>
        /// Extension for UnityEngine.Object.
        /// <br/>
        /// Adds asset to default addressable group.
        /// </summary>
        /// <param name="asset">Asset to add</param>
        /// <returns></returns>
        public static AddressableAssetEntry AddAssetToAddressables(this Object asset)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
            settings.CreateAssetReference(assetGUID);
            return settings.FindAssetEntry(assetGUID);
        }
    }
}
