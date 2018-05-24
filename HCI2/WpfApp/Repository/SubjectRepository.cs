using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;
using System.Data.Entity;

namespace WpfApp.Repository
{
    public class SubjectRepository : Repository<Classroom>
    {
        public SubjectRepository(DbContext context) : base(context)
        {
        }
    }
}
