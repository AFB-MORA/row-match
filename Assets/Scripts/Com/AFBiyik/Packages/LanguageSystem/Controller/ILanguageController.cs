using UniRx;
using UnityEngine;

namespace Com.AFBiyik.LanguageSystem
{
    public interface ILanguageController {
        IReadOnlyReactiveProperty<bool> IsLanguageReady { get; }
        IReadOnlyReactiveProperty<SystemLanguage> Language { get; }
        LanguageConfig LanguageConfig { get; }
        string GetText(string tag, params string[] values);
        bool HasTag(string tag);
        void SetLanguage(SystemLanguage systemLanguage);
    }
}
