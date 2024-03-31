namespace BookAppointment.API.DTOs.Response
{
    public class BookAppointmentResponse
    {
        public string CustomerId { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string Token { get; set; }
        public string CreatedAt { get; set; }
    }
}
