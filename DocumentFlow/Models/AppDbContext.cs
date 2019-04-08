using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<RoleType> RoleTypes { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<MyFile> MyFiles { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentState> DocumentStates { get; set; }

        public static void Seed(AppDbContext context)
        {
            // добавляем пользователя по умолчанию
            var defaultUser = new User { Login = "admin" };
            context.Users.Add(defaultUser);
            // RodeTypes
            context.RoleTypes.Add(new RoleType { RoleTypeName = "Full"});
            context.RoleTypes.Add(new RoleType { RoleTypeName = "AlmostFull" });
            context.RoleTypes.Add(new RoleType { RoleTypeName = "ReadOnly" });
            // Document states
            context.DocumentStates.Add(new DocumentState { DocStateName = "New" });


            context.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<CityList>()
            //   .HasOptional(j => j.Trip)
            //   .WithMany(x => x.CityList)
            //   .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Ticket>()
            //   .HasOptional(j => j.Trip)
            //   .WithMany(x => x.Tickets)
            //   .WillCascadeOnDelete(true);

            //modelBuilder.Entity<CheckItem>()
            //   .HasOptional(j => j.Trip)
            //   .WithMany(x => x.CheckItems)
            //   .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }

        
    }
}