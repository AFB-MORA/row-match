using Com.AFBiyik.AssetSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Com.AFBiyik.LanguageSystem
{
    [RequireComponent(typeof(Image))]
    public class LanguageImage : LanguageComponentBase<Image>
    {
        // Serialize Fields
        [SerializeField]
        protected string atlas;

        // Dependencies
        [Inject]
        protected IAssetLoader assetLoader;

        // Protected Methods
        protected override void UpdateText()
        {
            string lng = languageController.Language.Value.ToIsoString();
            SetText(lng);
        }

        protected override async void SetText(string lng)
        {
            var sprite = await assetLoader.LoadSprite(atlas, languageTag + "_" + lng);
            LanguageComponent.sprite = sprite;
        }
    }
}
