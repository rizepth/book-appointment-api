using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BookAppointment.API.Common.TypeConverters
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        private const string Format = "hh\\:mm";
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeSpan.ParseExact(reader.GetString(), Format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}
