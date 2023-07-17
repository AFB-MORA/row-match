using UnityEngine;
using UnityEngine.UI;

namespace Com.AFBiyik.LanguageSystem
{
    [RequireComponent(typeof(Text))]
    public class LanguageText : LanguageComponentBase<Text>
    {
        protected override void SetText(string text)
        {
            LanguageComponent.text = text;
        }
    }
}