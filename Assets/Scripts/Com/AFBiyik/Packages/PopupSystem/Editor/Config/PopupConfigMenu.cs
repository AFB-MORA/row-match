using Com.AFBiyik.AssetSystem.Editor;
using UnityEditor;
using UnityEngine;

namespace Com.AFBiyik.PopupSystem.Editor
{
    public class PopupConfigMenu
    {
        [MenuItem("AFBiyik/PopupSystem/PopupConfig", false, 0)]
        public static void Settings()
        {
            PopupConfig popupConfig = AssetDatabase.LoadAssetAtPath<PopupConfig>("Assets/PopupSystem/PopupConfig.asset");

            if (popupConfig == null)
            {
                if (!AssetDatabase.IsValidFolder("Assets/PopupSystem"))
                {
                    AssetDatabase.CreateFolder("Assets", "PopupSystem");
                }

                popupConfig = ScriptableObject.CreateInstance<PopupConfig>();
                AssetDatabase.CreateAsset(popupConfig, "Assets/PopupSystem/PopupConfig.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                var addressable = popupConfig.AddAssetToAddressables();
                addressable.SetAddress("PopupConfig");
            }

            Selection.activeObject = popupConfig;
        }
    }
}