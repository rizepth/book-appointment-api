using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Repository;
using BookAppointment.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Infrastructure.Repository
{
    public class AgencyRepository : BaseRepository<Agency>, IAgencyRepository
    {
        public AgencyRepository(DataContext context) : base(context)
        {
        }

        public async Task SetLimit(int limit)
        {
            var data = _context.Agencies.First();
            data.MaxAppointmentLimit = limit;
            Update(data);

            await _context.SaveChangesAsync();
        }
    }
}
