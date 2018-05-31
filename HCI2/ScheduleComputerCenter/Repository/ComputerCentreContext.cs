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

        DbSet<Classroom> Classrooms { get; set; }
        DbSet<Course> Courses {get; set;}
        DbSet<Software> Softwares {get; set;}
        DbSet<Subject> Subjects {get; set;}
        DbSet<Term> Terms {get; set;}
    }
}
