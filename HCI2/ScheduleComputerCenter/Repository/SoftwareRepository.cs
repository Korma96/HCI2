using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleComputerCenter.Model;
using System.Data.Entity;

namespace ScheduleComputerCenter.Repository
{
    public class SoftwareRepository : Repository<Software>
    {
        public SoftwareRepository(DbContext context) : base(context)
        {
            
        }
    }
}

