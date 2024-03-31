using BookAppointment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Infrastructure.Persistence
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agency>().HasData(
                new Agency
                {
                    Id = 1,
                    CompanyName = "Company ABC",
                    EmailAddress = "company@gmail.com",
                    PhoneNumber = "02188273723",
                    Address="Indonesia",
                    MaxAppointmentLimit = 10,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "admin",
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = "admin",
                }
            );
            modelBuilder.Entity<AgencyUser>().HasData(
                new AgencyUser
                {
                    Id = 1,
                    UserName = "admin",
                    FullName = "administrator",
                    Password = "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=",
                    CreatedAt = DateTime.Now,
                    CreatedBy = "admin",
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = "admin",
                }
            );
        }
    }
}
