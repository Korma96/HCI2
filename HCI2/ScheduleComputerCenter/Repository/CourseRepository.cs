using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ScheduleComputerCenter.Model;

namespace ScheduleComputerCenter.Repository
{
    public class CourseRepository : Repository<Course>
    {
        public CourseRepository(DbContext context) : base(context)
        {

        }
    }
}
