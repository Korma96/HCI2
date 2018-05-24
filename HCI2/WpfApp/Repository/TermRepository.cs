using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;
using System.Data.Entity;

namespace WpfApp.Repository
{
    public class TermRepository : Repository<Classroom>
    {
        public TermRepository(DbContext context) : base(context)
        {
        }
    }
}
