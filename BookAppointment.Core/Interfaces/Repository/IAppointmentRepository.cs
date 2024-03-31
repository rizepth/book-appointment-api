using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Interfaces.Repository
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        Task<int> GetMaximumLimit();
        bool CheckExistingToken(string token);
        IQueryable<Appointment> GetAppointments();
    }
}
