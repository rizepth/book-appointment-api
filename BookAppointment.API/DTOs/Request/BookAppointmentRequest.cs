using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BookAppointment.API.Common.TypeConverters;

namespace BookAppointment.API.DTOs.Request
{
    public class BookAppointmentRequest
    {
        [Required]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly AppointmentDate { get; set; }

        [Required]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan AppointmentTime { get; set; }
    }
}
