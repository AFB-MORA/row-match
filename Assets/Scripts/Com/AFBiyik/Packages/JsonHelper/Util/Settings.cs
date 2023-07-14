using Newtonsoft.Json;

namespace Com.AFBiyik.JsonHelper
{
    public static class Settings
    {
        public static JsonSerializerSettings Get()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return settings;
        }
    }
}
