using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleComputerCenter.Repository;
using System.Data.Entity;

namespace ScheduleComputerCenter.Model
{
    public class ComputerCentre
    {
        public static DbContext context = new ComputerCentreContext();
        public static ClassroomRepository ClassroomRepository = new ClassroomRepository(context);
        public static CourseRepository CourseRepository = new CourseRepository(context);
        public static SoftwareRepository SoftwareRepository = new SoftwareRepository(context);
        public static SubjectRepository SubjectRepository = new SubjectRepository(context);
        public static TermRepository TermRepository = new TermRepository(context);

        public static void AddDummyData()
        {
            List<Software> softwares = new List<Software>()
            {
                new Software(1, "Matlab", OsType.Windows, "Matlab doo", @"http://www.matlab.org", 2005, 200, "Software for science"),
                new Software(2, "Eclipse", OsType.Any, "Oracle", @"http://www.eclipse.com", 2001, 0, "Software for java app development"),
                new Software(3, "PyCharm", OsType.Linux, "Python", @"http://www.pycharm.com", 2008, 300, "Software for python app development")
            };
            SoftwareRepository.AddRange(softwares);

            List<Classroom> classrooms = new List<Classroom>()
            {
                new Classroom(1, "L1", 16, true, true, false, OsType.Any, new List<Software>(){ softwares[1]}),
                new Classroom(2, "L2", 32, false, true, false, OsType.Windows, new List<Software>(){ softwares[0]}),
                new Classroom(3, "L3", 16, true, false, false, OsType.Linux, new List<Software>(){ softwares[2]}),
                new Classroom(4, "L4", 32, true, true, false, OsType.Any, new List<Software>(){ softwares[0], softwares[1], softwares[2]}),
                new Classroom(5, "L5", 64, false, true, false, OsType.Any, new List<Software>(){ softwares[0], softwares[1]}),
                new Classroom(6, "L6", 32, true, true, true, OsType.Any, new List<Software>(){ softwares[1], softwares[2]})
            };
            ClassroomRepository.AddRange(classrooms);

            List<Course> courses = new List<Course>()
            {
                new Course(1, "Racunarstvo i Automatika", "05-9-2000", "Smer za obucavanje inzenjera racunarstva", "RA"),
                new Course(2, "Softversko inzenjerstvo i informacione tehnologije", "12-09-2013", "Najbolji softverski smer FTN-a", "SW")
            };
            CourseRepository.AddRange(courses);

            List<Subject> subjects = new List<Subject>()
            {
                new Subject(1, "HCI", courses[0], "Najbolji predmet na svetu xD", 16, 2, 2, true, false, false, OsType.Windows, softwares[0]),
                new Subject(2, "Internet softverske arhitekture", courses[1], "Spring", 32, 2, 2, false, true, false, OsType.Any, softwares[1]),
                new Subject(3, "Android", courses[0], "Android programiranje", 16, 2, 2, true, true, false, OsType.Linux, softwares[2]),
                new Subject(4, "PIGKUT", courses[1], "Izrada seminarskog rada i jos svasta nesto", 32, 2, 2, true, false, true, OsType.Windows, softwares[0])
            };
            SubjectRepository.AddRange(subjects);

            context.SaveChanges();
        }

    }
}
