using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Com.AFBiyik.AssetSystem;
using UnityEngine;

namespace Com.AFBiyik.LanguageSystem
{
    public class LanguageConfig : ScriptableObject
    {
        [SerializeField]
        protected SystemLanguage defaultLanguage = SystemLanguage.English;
        [SerializeField]
        protected List<SystemLanguage> languages = new List<SystemLanguage> { SystemLanguage.English };

        public IReadOnlyList<SystemLanguage> Languages => languages;
        public SystemLanguage DefaultLanguage => defaultLanguage;

        public static async UniTask<LanguageConfig> Load(IAssetLoader assetLoader)
        {
            var tmp = await assetLoader.LoadAsset<LanguageConfig>("LanguageConfig");
            return Instantiate(tmp);
        }
    }
}
