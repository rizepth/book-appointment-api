using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;
using BookAppointment.API.Common.TypeConverters;

namespace BookAppointment.API.DTOs.Request
{
    public class DayoffRequest
    {
        [Required]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly DayOffDate { get; set; }
        public string DayOffNotes { get; set; }
    }
}
