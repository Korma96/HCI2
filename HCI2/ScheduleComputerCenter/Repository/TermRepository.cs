using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleComputerCenter.Model;
using System.Data.Entity;

namespace ScheduleComputerCenter.Repository
{
    public class TermRepository : Repository<Term>
    {
        public TermRepository(DbContext context) : base(context)
        {
        }
    }
}
