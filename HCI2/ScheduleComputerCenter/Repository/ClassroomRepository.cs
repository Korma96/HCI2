using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleComputerCenter.Model;
using System.Data.Entity;

namespace ScheduleComputerCenter.Repository
{
    public class ClassroomRepository : Repository<Classroom>
    {
        public ClassroomRepository(DbContext context) : base(context)
        {

        }
    }
}
