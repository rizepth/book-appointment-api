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
    public class AgencyUserRepository : BaseRepository<AgencyUser>, IAgencyUserRepository
    {
        public AgencyUserRepository(DataContext context) : base(context)
        {
        }
    }
}
