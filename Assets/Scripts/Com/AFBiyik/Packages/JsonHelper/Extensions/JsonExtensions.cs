using Newtonsoft.Json;

namespace Com.AFBiyik.JsonHelper
{
    /// <summary>
    /// Json extension methods.
    ///
    /// <example>
    ///
    /// <h4>Parse</h4>
    /// <code>
    /// string json = "{\"foo\":\"foo\",\"bar\":\"bar\"}";
    /// FooObject foo = json.ParseJson<FooObject>();
    /// </code>
    /// 
    /// <h4>Stringify</h4>
    ///
    /// <code>
    /// FooObject foo = new FooObject;
    /// string json1 = foo.ToJson();
    /// // {"foo":"foo","bar":"bar"}
    /// string json2 = foo.ToJson(true);
    /// // {
    /// //      "foo":"foo",
    /// //      "bar":"bar"
    /// // }
    /// </code>
    ///
    /// </example>
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Deserialize JSON string to T.
        /// </summary>
        /// <typeparam name="T">Serializable object</typeparam>
        /// <param name="data">JSON string</param>
        /// <returns>Deserialized object</returns>
        public static T ParseJson<T>(this string data)
        {
            return JsonConvert.DeserializeObject<T>(data, Settings.Get());
        }

        /// <summary>
        /// Serialize T to JSON string
        /// </summary>
        /// <typeparam name="T">Serializable object</typeparam>
        /// <param name="value">Object to serialize</param>
        /// <param name="beutify">Optional. Beautifies json string. Default false.</param>
        /// <returns>JSON string</returns>
        public static string ToJson<T>(this T value, bool beutify = false)
        {
            return JsonConvert.SerializeObject(value, beutify ? Formatting.Indented : Formatting.None, Settings.Get());
        }
    }
}
