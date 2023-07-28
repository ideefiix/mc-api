using Newtonsoft.Json;

namespace Api.Models;

public static class JsonConverter
{
    public static string SerializeObject(object value)
        {
            try
            {
                var serializerSettings = GetSerializerSettings();

                return JsonConvert.SerializeObject(value, serializerSettings);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to serialize object.", ex);
            }
        }

    public static T DeserializeObject<T>(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value, GetSerializerSettings());
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to deserialize(generic) message data to object.", ex);
            }
        }

        public static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateParseHandling = DateParseHandling.DateTimeOffset,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
        }
}