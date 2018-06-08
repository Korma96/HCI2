using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleComputerCenter.Model;

namespace ScheduleComputerCenter.Repository
{
    public class ComputerCentreContext : DbContext
    {

        public ComputerCentreContext()
        {

        }

        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Software> Softwares {get; set;}
        public DbSet<Subject> Subjects {get; set;}
        public DbSet<Term> Terms {get; set;}
        public DbSet<Day> Days { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            // configures one-to-many relationship
            modelBuilder.Entity<Term>()
                .HasRequired<Day>(t => t.Day)
                .WithMany(d => d.Terms)
                .HasForeignKey<int>(t => t.DayId);
        }
    }
}
