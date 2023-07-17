using UnityEngine;

namespace Com.AFBiyik.LanguageSystem
{
    public abstract class LanguageComponentBase<T> : LanguageBase where T : MonoBehaviour
    {
        // Private Properties
        private T languageComponent;

        // Public Properties
        public virtual T LanguageComponent
        {
            get
            {
                if (languageComponent == null)
                {
                    languageComponent = GetComponent<T>();
                }

                return languageComponent;
            }
        }
    }
}
