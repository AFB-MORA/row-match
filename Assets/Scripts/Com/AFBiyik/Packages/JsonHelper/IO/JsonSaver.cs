using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Com.AFBiyik.JsonHelper
{
    /// <summary>
    /// Json save functions.
    /// <example>
    ///
    /// <code>
    /// FooObject foo = new FooObject;
    /// JsonSaver.Save(foo, Application.persistentDataPath + "/foojson.json");
    /// // {"foo":"foo","bar\":"bar"}
    /// JsonSaver.Save(foo, Application.persistentDataPath + "/foojson.json", true);
    /// // {
    /// //      "foo":"foo",
    /// //      "bar":"bar"
    /// // }
    /// </code>
    /// 
    /// </example>
    /// </summary>
    public static class JsonSaver
    {
        /// <summary>
        /// Serialize T to JSON string and saves to path.
        /// </summary>
        /// <typeparam name="T">Serializable object</typeparam>
        /// <param name="value">Object to serialize</param>
        /// <param name="path">Save path</param>
        /// <param name="beutify">Optional. Beautifies json string. Default false.</param>
        public static void Save<T>(T value, string path, bool beutify = false)
        {
            var json = JsonConvert.SerializeObject(value, beutify ? Formatting.Indented : Formatting.None, Settings.Get());

            // Create a new file     
            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                // Add some text to file    
                Byte[] text = new UTF8Encoding(true).GetBytes(json);
                fs.Write(text, 0, text.Length);
            }
        }
    }
}
