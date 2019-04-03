﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {

        }

        public DbSet<User> Users { get; set; }

        public static void Seed(AppDbContext context)
        {
            // добавляем пользователя по умолчанию
            var defaultUser = new User { Login = "admin" };
            context.Users.Add(defaultUser);
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