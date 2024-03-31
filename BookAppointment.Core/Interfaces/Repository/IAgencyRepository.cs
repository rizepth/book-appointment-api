using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Interfaces.Repository
{
    public interface IAgencyRepository : IBaseRepository<Agency>
    {
        Task SetLimit(int limit);
    }
}
