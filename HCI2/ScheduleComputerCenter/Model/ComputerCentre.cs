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
        public static DayRepository DayRepository = new DayRepository(context);

        public static void AddDummyData()
        {

            List<Software> softwares = new List<Software>()
            {
                new Software("Matlab", OsType.Windows, "Matlab doo", @"http://www.matlab.org", 2005, 200, "Software for science"),
                new Software("Eclipse", OsType.Any, "Oracle", @"http://www.eclipse.com", 2001, 0, "Software for java app development"),
                new Software("PyCharm", OsType.Linux, "Python", @"http://www.pycharm.com", 2008, 300, "Software for python app development")
            };

            SoftwareRepository.AddRange(softwares);
            */

            List<Day> days = new List<Day>()
            {
                new Day("PONEDELJAK"),
                new Day("UTORAK"),
                new Day("SREDA"),
                new Day("CETVRTAK"),
                new Day("PETAK"),
                new Day("SUBOTA")
            };
            DayRepository.AddRange(days);
            /*
            List<Classroom> classrooms = new List<Classroom>()
            {
                new Classroom("L1", "Description 1", 16, true, true, false, OsType.Any, new List<Software>(){ softwares[1]}),
                new Classroom("L2", "Description 2", 32, false, true, false, OsType.Windows, new List<Software>(){ softwares[0]}),
                new Classroom("L3", "Description 3", 16, true, false, false, OsType.Linux, new List<Software>(){ softwares[2]}),
                new Classroom("L4", "Description 4", 32, true, true, false, OsType.Any, new List<Software>(){ softwares[0], softwares[1], softwares[2]}),
                new Classroom("L5", "Description 5", 64, false, true, false, OsType.Any, new List<Software>(){ softwares[0], softwares[1]}),
                new Classroom("L6", "Description 6", 32, true, true, true, OsType.Any, new List<Software>(){ softwares[1], softwares[2]})
            };

            ClassroomRepository.AddRange(classrooms);

            List<Course> courses = new List<Course>()
            {
                new Course("Racunarstvo i Automatika", "05-9-2000", "Smer za obucavanje inzenjera racunarstva"),
                new Course("Softversko inzenjerstvo i informacione tehnologije", "12-09-2013", "Najbolji softverski smer FTN-a")
            };

            CourseRepository.AddRange(courses);

            List<Subject> subjects = new List<Subject>()
            {
                new Subject("HCI","2", courses[0], "Najbolji predmet na svetu xD", 16, 2, 2, true, false, false, OsType.Windows, softwares[0]),
                new Subject("Internet softverske arhitekture","2", courses[1], "Spring", 32, 2, 2, false, true, false, OsType.Any, softwares[1]),
                new Subject("Android", "2",courses[0], "Android programiranje", 16, 2, 2, true, true, false, OsType.Linux, softwares[2]),
                new Subject("PIGKUT","2", courses[1], "Izrada seminarskog rada i jos svasta nesto", 32, 2, 2, true, false, true, OsType.Windows, softwares[0])
            };

            SubjectRepository.AddRange(subjects);

            List<Day> days = new List<Day>()
            {
                new Day("PONEDELJAK"),
                new Day("UTORAK"),
                new Day("SREDA"),
                new Day("ČETVRTAK"),
                new Day("PETAK"),
                new Day("SUBOTA")
            };

            DayRepository.AddRange(days);

            context.SaveChanges();
        }
        */
        }
    }
}
