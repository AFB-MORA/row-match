using UnityEngine;

namespace Com.AFBiyik.JsonHelper
{
    /// <summary>
    /// <see cref="TextAsset"/> extensions.
    /// <example>
    ///
    /// <code>
    /// TextAsset text = Resources.Load<TextAsset>("foojson");
    /// FooObject foo = text.ParseJson<T>();
    /// </code>
    /// 
    /// </example>
    /// </summary>
    public static class JsonTextAssetExtensions
    {
        /// <summary>
        /// Deserializes json TextAsset file into T.
        /// </summary>
        /// <typeparam name="T">Serializable object</typeparam>
        /// <param name="text">Text asset</param>
        /// <returns>Deserialized object</returns>
        public static T ParseJson<T>(this TextAsset text)
        {
            return text.text.ParseJson<T>();
        }
    }
}
