using System;
using System.IO;
using UnityEngine;

namespace Com.AFBiyik.JsonHelper
{
    /// <summary>
    /// Json load functions.
    /// <example>
    /// 
    /// <code>
    /// FooObject foo = JsonLoader.Load&lt;FooObject&gt;(Application.persistentDataPath + "/foojson.json");
    ///
    /// FooObject foo = JsonLoader.LoadFromResources&lt;FooObject&gt;("foojson");
    /// </code>
    ///
    /// </example>
    /// </summary>
    public static class JsonLoader
    {
        /// <summary>
        /// Deserializes json file into T.
        /// </summary>
        /// <typeparam name="T">Serializable object</typeparam>
        /// <param name="path">File path</param>
        /// <returns>Deserialized object</returns>
        public static T Load<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Invalid path value: " + path);
            }

            string json = File.ReadAllText(path);
            return json.ParseJson<T>();
        }

        /// <summary>
        /// Deserializes json file into T from resources.
        /// </summary>
        /// <typeparam name="T">Serializable object</typeparam>
        /// <param name="path">Text asset path</param>
        /// <returns>Deserialized object</returns>
        public static T LoadFromResources<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Invalid path value: " + path);
            }

            TextAsset text = Resources.Load<TextAsset>(path);
            return text.ParseJson<T>();
        }
    }
}
