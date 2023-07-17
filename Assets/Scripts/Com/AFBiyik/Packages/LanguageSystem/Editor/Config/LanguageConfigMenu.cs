using Com.AFBiyik.AssetSystem.Editor;
using UnityEditor;
using UnityEngine;

namespace Com.AFBiyik.LanguageSystem.Editor
{
    public class LanguageConfigMenu
    {
        [MenuItem("AFBiyik/LanguageSystem/LanguageConfig", false, 0)]
        public static void Settings()
        {
            LanguageConfig languageConfig = AssetDatabase.LoadAssetAtPath<LanguageConfig>("Assets/Language/LanguageConfig.asset");

            if (languageConfig == null)
            {
                if (!AssetDatabase.IsValidFolder("Assets/Language"))
                {
                    AssetDatabase.CreateFolder("Assets", "Language");
                }

                languageConfig = ScriptableObject.CreateInstance<LanguageConfig>();
                AssetDatabase.CreateAsset(languageConfig, "Assets/Language/LanguageConfig.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                var addressable = languageConfig.AddAssetToAddressables();
                addressable.SetAddress("LanguageConfig");
            }

            Selection.activeObject = languageConfig;
        }
    }
}