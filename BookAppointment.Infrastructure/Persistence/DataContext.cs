using BookAppointment.Core.Entities;
using BookAppointment.Core.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookAppointment.Infrastructure.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions) {
            try
            {
                var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (dbCreator != null)
                {
                    if (!dbCreator.CanConnect()) dbCreator.Create();
                    if (!dbCreator.HasTables()) dbCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<AgencyUser> AgencyUsers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DayOff> DayOffs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            const string currentUser = "admin";

            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableEntity)entityEntry.Entity).CreatedAt = System.DateTime.Now;
                    ((AuditableEntity)entityEntry.Entity).CreatedBy = currentUser;
                }
                else
                {
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }
                ((AuditableEntity)entityEntry.Entity).UpdatedAt = System.DateTime.Now;
                ((AuditableEntity)entityEntry.Entity).UpdatedBy = "admin";
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
