using BookAppointment.Core.Entities;
using BookAppointment.Core.Interfaces.Common;
using BookAppointment.Core.Interfaces.Repository;
using BookAppointment.Core.Interfaces.Services;

namespace BookAppointment.Core.Services
{
    public class AgencyUserService : IAgencyUserService
    {
        private readonly IAgencyUserRepository _agencyRepo;
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHash _passwordHash;

        public AgencyUserService(IAgencyUserRepository agencyRepo, IPasswordHash passwordHash, IUnitOfWork uow)
        {
            _agencyRepo=agencyRepo;
            _passwordHash=passwordHash;
            _uow=uow;
        }

        public async Task<AgencyUser> Authenticate(string username, string password)
        {
            var hashPassword = _passwordHash.Hash(password);
            var user = _agencyRepo.GetAll().FirstOrDefault(x => x.UserName == username && x.Password == hashPassword);

            return await Task.FromResult(user);
        }

        public async Task Register(AgencyUser entity)
        {
            if (entity is not null)
            {
                entity.Password = _passwordHash.Hash(entity.Password);
            }

            _agencyRepo.Insert(entity);
            await _uow.CompleteAsync();
        }
    }
}
