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
            this.Configuration.AutoDetectChangesEnabled = true;
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
        public DbSet<ExternalContact> ExternalContacts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<ColorScheme> ColorSchemes { get; set; }
        public DbSet<Constant> Constants { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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

            db.Languages.Add(new Language { LangCode = "EN", LangCultureCode = "en-US", LangName = "English" });
            db.Languages.Add(new Language { LangCode = "RU", LangCultureCode = "ru-RU", LangName = "Русский" });

            db.ColorSchemes.Add(new ColorScheme { Name = "Violet"});

            db.Constants.Add(new Constant {DocPath = Environment.CurrentDirectory, FilePath = Environment.CurrentDirectory });

            db.SaveChanges();

        }
    }
}