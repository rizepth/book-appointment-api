using BookAppointment.Core.Entities;

namespace BookAppointment.API.DTOs.Response
{
    public class QueueAppointmentResponse
    {
        public int TotalAppointments { get; set; }
        public List<AppointmentView> Appointments { get; set; }
    }

    public class AppointmentView
    {
        public string CustomerName { get; set; }
        public string Token { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
    }
}
