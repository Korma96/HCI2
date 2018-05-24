using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Repository;
using System.Data.Entity;

namespace WpfApp.Model
{
    public class ComputerCentre
    {
        public static DbContext context = new ComputerCentreContext();
        public static ClassroomRepository ClassroomRepository = new ClassroomRepository(context);
        public static CourseRepository CourseRepository = new CourseRepository(context);
        public static SoftwareRepository SoftwareRepository = new SoftwareRepository(context);
        public static SubjectRepository SubjectRepository = new SubjectRepository(context);
        public static TermRepository TermRepository = new TermRepository(context);
    }
}
