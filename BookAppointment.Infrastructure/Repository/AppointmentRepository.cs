using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Repository;
using BookAppointment.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Infrastructure.Repository
{
    public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DataContext context) : base(context)
        {

        }

        public IQueryable<Appointment> GetAppointments()
        {
            return GetAll().Include(x => x.Customer);
        }

        public bool CheckExistingToken(string token)
        {
            return _context.Appointments.Any(x => x.Token ==  token);
        }

        public async Task<int> GetMaximumLimit()
        {
            return await Task.FromResult(_context.Agencies.FirstAsync().Result.MaxAppointmentLimit);
        }
    }
}
