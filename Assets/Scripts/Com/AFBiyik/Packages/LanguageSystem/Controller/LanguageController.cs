using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Com.AFBiyik.AssetSystem;
using Com.AFBiyik.JsonHelper;
using UniRx;
using UnityEngine;

namespace Com.AFBiyik.LanguageSystem
{
    public class LanguageController : ILanguageController, IDisposable
    {
        // Protected Constants
        protected const string LANGUAGE_PREFS = "selectedLanguage";

        // Protected Readonly properties
        protected readonly IAssetManager assetManager;
        protected readonly IAssetLoader assetLoader;
        protected readonly CompositeDisposable disposables = new CompositeDisposable();
        protected readonly ReactiveProperty<bool> isLanguageReady = new ReactiveProperty<bool>(false);
        protected readonly ReactiveProperty<SystemLanguage> language = new ReactiveProperty<SystemLanguage>(SystemLanguage.English);

        // Protected Properties
        protected LanguageConfig languageConfig;
        protected Dictionary<string, string> languageDictionary = null;

        // Public Properties
        public IReadOnlyReactiveProperty<bool> IsLanguageReady => isLanguageReady;
        public IReadOnlyReactiveProperty<SystemLanguage> Language => language;
        public LanguageConfig LanguageConfig => languageConfig;

        // Constructor
        public LanguageController(IAssetManager assetManager, IAssetLoader assetLoader)
        {
            this.assetManager = assetManager;
            this.assetLoader = assetLoader;

            assetManager.IsAssetInitialized.Subscribe(OnAssetsReady).AddTo(disposables);
        }

        // Public Methods
        public void Dispose()
        {
            disposables.Dispose();
        }

        public virtual string GetText(string tag, params string[] values)
        {
            if (string.IsNullOrEmpty(tag)) return "";

            if (!isLanguageReady.Value)
            {
                return tag;
            }

            if (!languageDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Language does not contain tag: " + tag);
                return tag;
            }

            string returnText = languageDictionary[tag];

            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    returnText = returnText.Replace("[" + i + "]", values[i]);
                }
            }
            return returnText;
        }

        public virtual bool HasTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return false;
            }

            return languageDictionary.ContainsKey(tag);
        }

        public virtual async void SetLanguage(SystemLanguage systemLanguage)
        {
            if (!languageConfig.Languages.Contains(systemLanguage))
            {
                return;
            }

            if (systemLanguage == language.Value)
            {
                return;
            }

            PlayerPrefs.SetInt(LANGUAGE_PREFS, (int)systemLanguage);
            languageDictionary = await GetLanguageDictionaryAsync(systemLanguage);

            language.SetValueAndForceNotify(systemLanguage);
        }

        // Private Methods
        private void OnAssetsReady(bool ready)
        {
            if (ready && !isLanguageReady.Value)
            {
                InitializeAsync();
            }
        }

        // Protected Methods
        protected virtual async void InitializeAsync()
        {
            await UniTask.SwitchToMainThread();
            languageConfig = await LanguageConfig.Load(assetLoader);
            var systemLanguage = GetLanguage();
            languageDictionary = await GetLanguageDictionaryAsync(systemLanguage);
            language.SetValueAndForceNotify(systemLanguage);
            isLanguageReady.SetValueAndForceNotify(true);
        }

        protected virtual SystemLanguage GetLanguage()
        {
            int temp = PlayerPrefs.GetInt(LANGUAGE_PREFS, -1);

            if (temp < 0)
            {
                temp = (int)languageConfig.DefaultLanguage; // Default language

                var sysLang = Application.systemLanguage;
                if (languageConfig.Languages.Contains(sysLang))
                {
                    temp = (int)sysLang;
                }

                PlayerPrefs.SetInt(LANGUAGE_PREFS, temp);
            }

            return (SystemLanguage)temp;
        }

        protected virtual async UniTask<Dictionary<string, string>> GetLanguageDictionaryAsync(SystemLanguage language)
        {
            await UniTask.SwitchToMainThread();
            var lng = await assetLoader.LoadAsset<TextAsset>("Language_" + language.ToIsoString());
            return lng.ParseJson<Dictionary<string, string>>();
        }
    }
}

