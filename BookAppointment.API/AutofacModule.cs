using Autofac;
using BookAppointment.Core.Interfaces.Common;
using BookAppointment.Core.Interfaces.Repository;
using BookAppointment.Core.Interfaces.Services;
using BookAppointment.Core.Utilities;
using BookAppointment.Infrastructure.Persistence;
using BookAppointment.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace BookAppointment.API
{
    public class AutofacModule : Module
    {
        private readonly IConfiguration _configuration;

        public AutofacModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Scan the BookAppointment.Core assembly
            builder.RegisterAssemblyTypes(typeof(ICustomerService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            // Scan the BookAppointment.Infrastructure assembly
            builder.RegisterAssemblyTypes(typeof(ICustomerRepository).Assembly, typeof(CustomerRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<PasswordHash>().As<IPasswordHash>().InstancePerDependency();

            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ConnectionApp"));
                return new DataContext(optionsBuilder.Options);
            }).InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork<DataContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();

        }
    }
}
