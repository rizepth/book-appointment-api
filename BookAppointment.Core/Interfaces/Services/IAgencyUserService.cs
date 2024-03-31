using BookAppointment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Interfaces.Services
{
    public interface IAgencyUserService
    {
        Task Register(AgencyUser entity);
        Task<AgencyUser> Authenticate(string username, string password);
    }
}
