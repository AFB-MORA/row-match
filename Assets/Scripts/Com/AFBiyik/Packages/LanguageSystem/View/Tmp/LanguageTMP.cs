#if LANGUAGE_SYSTEM_TMP
using TMPro;
using UnityEngine;

namespace Com.AFBiyik.LanguageSystem
{
    [RequireComponent(typeof(TMP_Text))]
    public class LanguageTMP : LanguageComponentBase<TMP_Text>
    {
        protected override void SetText(string text)
        {
            LanguageComponent.text = text;
        }
    }
}
#endif