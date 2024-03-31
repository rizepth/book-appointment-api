using BookAppointment.Core.DTOs;
using BookAppointment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task SetBookingLimit(int limit);
        Task<AppointmentResultDto> CreateAppointment(Appointment appointment);
        List<Appointment> GetAll(DateTime? dateFrom, DateTime? dateTo);
    }
}
