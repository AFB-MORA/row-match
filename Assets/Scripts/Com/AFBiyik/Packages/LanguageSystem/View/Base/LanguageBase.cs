using UniRx;
using UnityEngine;
using Zenject;

namespace Com.AFBiyik.LanguageSystem
{
    public abstract class LanguageBase : MonoBehaviour
    {
        /// Serialize Fields
        [SerializeField]
        protected string languageTag = "";
        [SerializeField]
        protected string[] values;

        // Dependencies
        [Inject]
        protected ILanguageController languageController;

        // Unity Methods
        protected virtual void OnEnable()
        {
            CheckLanguageReady();
        }

        // Private Methods
        private void CheckLanguageReady()
        {
            if (languageController.IsLanguageReady.Value)
            {
                SubscribeLanguage();
            }
            else
            {
                SubscribeIsLanguageReady();
            }
        }

        private void SubscribeIsLanguageReady()
        {
            languageController.IsLanguageReady
                .TakeUntilDisable(gameObject)
                .Subscribe(OnLanguageReady);
        }

        private void SubscribeLanguage()
        {
            languageController.Language
                .TakeUntilDisable(gameObject)
                .Subscribe(SetLanguage);
        }

        private void OnLanguageReady(bool ready)
        {
            if (ready)
            {
                SubscribeLanguage();
            }
        }

        // Protected Methods
        protected virtual void SetLanguage(SystemLanguage language)
        {
            UpdateText();
        }

        protected virtual void UpdateText()
        {
            string resultText = languageTag;
            resultText = languageController.GetText(languageTag, values);
            SetText(resultText);
        }

        // Protected Abstract Methods
        protected abstract void SetText(string text);

        // Public Methods
        public virtual void SetTag(string tag)
        {
            this.languageTag = tag;

            UpdateText();
        }

        public virtual void SetValues(params string[] values)
        {
            this.values = values;

            UpdateText();
        }

        public virtual void SetTagAndValues(string tag, params string[] values)
        {
            this.languageTag = tag;
            this.values = values;

            UpdateText();
        }
    }
}