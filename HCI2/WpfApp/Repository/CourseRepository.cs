using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WpfApp.Model;

namespace WpfApp.Repository
{
    public class CourseRepository : Repository<Course>
    {
        public CourseRepository(DbContext context) : base(context)
        {

        }
    }
}
