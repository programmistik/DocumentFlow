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

        static AppDbContext()
        {
            Database.SetInitializer<AppDbContext>(new MyContextInitializer());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<MyFile> MyFiles { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentState> DocumentStates { get; set; }
        public DbSet<ContactInfoType> ContactInfoTypes { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<NewsPost> NewsPosts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Employee> Employees { get; set; }



        //protected override void Seed(AppDbContext context)
        //{
        //    // добавляем пользователя по умолчанию
        //    var defaultUser = new User { Login = "admin" };
        //    context.Users.Add(defaultUser);

        //    // Document states
        //    context.DocumentStates.Add(new DocumentState { DocStateName = "New" });
        //    // ContactInfoTypes
        //    context.ContactInfoTypes.Add(new ContactInfoType { InfoType = "Phone" });
        //    context.ContactInfoTypes.Add(new ContactInfoType { InfoType = "Mobile" });
        //    context.ContactInfoTypes.Add(new ContactInfoType { InfoType = "Address" });
        //    context.ContactInfoTypes.Add(new ContactInfoType { InfoType = "e-mail" });
        //    context.ContactInfoTypes.Add(new ContactInfoType { InfoType = "Skype" });
        //    context.ContactInfoTypes.Add(new ContactInfoType { InfoType = "Facebook" });

        //    //Positions
        //    context.Positions.Add(new Position { PositionName = "Head of department" });


        //    context.SaveChanges();
        //}

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

    class MyContextInitializer : CreateDatabaseIfNotExists<AppDbContext>
    {
        protected override void Seed(AppDbContext db)
        {
            // добавляем пользователя по умолчанию
            var defaultUser = new User { Login = "admin", IsActive = true };
            db.Users.Add(defaultUser);

            // Document states
            db.DocumentStates.Add(new DocumentState { DocStateName = "New" });
            //db.DocumentStates.Add(new DocumentState { DocStateName = "Prepared" });
            db.DocumentStates.Add(new DocumentState { DocStateName = "In progress" });
            db.DocumentStates.Add(new DocumentState { DocStateName = "Done" });
            db.DocumentStates.Add(new DocumentState { DocStateName = "Rejected" });
            // ContactInfoTypes
            db.ContactInfoTypes.Add(new ContactInfoType { InfoType = "Phone" });
            db.ContactInfoTypes.Add(new ContactInfoType { InfoType = "Mobile" });
            db.ContactInfoTypes.Add(new ContactInfoType { InfoType = "Address" });
            db.ContactInfoTypes.Add(new ContactInfoType { InfoType = "e-mail" });
            db.ContactInfoTypes.Add(new ContactInfoType { InfoType = "Skype" });
            db.ContactInfoTypes.Add(new ContactInfoType { InfoType = "Facebook" });

            //Positions
            db.Positions.Add(new Position { PositionName = "Head of department" });


            db.SaveChanges();

        }
    }
}