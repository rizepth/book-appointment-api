using BookAppointment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Interfaces.Services
{
    public interface ICustomerService
    {
        Task Register(Customer entity);
        Task<Customer> Authenticate(string username, string password);
        IEnumerable<Customer> GetAll();
    }
}
