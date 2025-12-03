using System;
using Newtonsoft.Json;

namespace Meteorological.Models
{
    public class WeatherData
    {
        [JsonConverter(typeof(UnixEpochLongToDateTimeConverter))]
        public DateTime Date { get; set; }

        public double Value { get; set; }
        public string Quality { get; set; }
    }

    internal class UnixEpochLongToDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return default(DateTime);
            }

            long epoch;
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    epoch = Convert.ToInt64(reader.Value);
                    break;
                case JsonToken.String:
                    {
                        var s = (string)reader.Value;
                        if (!long.TryParse(s, out epoch))
                        {
                            return default(DateTime);
                        }
                        break;
                    }
                default:
                    return default(DateTime);
            }

            // Heuristic: values greater than 9999999999 are milliseconds, otherwise seconds
            try
            {
                if (Math.Abs(epoch) > 9999999999L)
                {
                    return DateTimeOffset.FromUnixTimeMilliseconds(epoch).UtcDateTime;
                }

                return DateTimeOffset.FromUnixTimeSeconds(epoch).UtcDateTime;
            }
            catch
            {
                return default(DateTime);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is DateTime dt))
            {
                writer.WriteNull();
                return;
            }

            var utc = dt.Kind == DateTimeKind.Utc ? dt : dt.ToUniversalTime();
            long milliseconds = new DateTimeOffset(utc).ToUnixTimeMilliseconds();
            writer.WriteValue(milliseconds);
        }
    }
}
