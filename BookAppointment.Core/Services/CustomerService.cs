using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Common;
using BookAppointment.Core.Interfaces.Repository;
using BookAppointment.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHash _passwordHash;
        public CustomerService(ICustomerRepository customerRepo, IUnitOfWork uow, IPasswordHash passwordHash)
        {
            _customerRepo=customerRepo;
            _uow=uow;
            _passwordHash=passwordHash;
        }

        public async Task<Customer> Authenticate(string username, string password)
        {
            var hashPassword = _passwordHash.Hash(password);
            var customer = _customerRepo.GetAll().FirstOrDefault(x => x.UserName == username && x.Password == hashPassword);

            return await Task.FromResult(customer);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepo.GetAll();
        }

        public async Task Register(Customer entity)
        {
            if(entity is not null)
            {
                entity.Password = _passwordHash.Hash(entity.Password);
            }

            _customerRepo.Insert(entity);
            await _uow.CompleteAsync();
        }
    }
}
