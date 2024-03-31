using BookAppointment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.DTOs
{
    public class AppointmentResultDto
    {
        public Appointment Appointment { get; set; }
        public string Notes { get; set; }
    }
}
