using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;
using System.Data.Entity;

namespace WpfApp.Repository
{
    public class ClassroomRepository : Repository<Classroom>
    {
        public ClassroomRepository(DbContext context) : base(context)
        {

        }
    }
}
