using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //declare a new table
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<CapWeek> CapWeeks { get; set; }
        public DbSet<CapDay> CapDays { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassDetail> ClassDetails { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public ApplicationDbContext(string connectionString) : base(connectionString)
        { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<Semester>().HasKey(r => r.Id);
            modelBuilder.Entity<UserProfile>().Property(t => t.StaffCode).HasMaxLength(50);
            base.OnModelCreating(modelBuilder);
        }

        public DbQuery<T> Query<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }

    }
}
