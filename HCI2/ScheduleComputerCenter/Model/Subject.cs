using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleComputerCenter.Model
{
    public class Subject
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public Course Course { get; set; }
        public String Description { get; set; }
        public int NumOfStudents { get; set; }
        public int MinNumOfClassesPerTerm { get; set; }
        public int NumOfClasses { get; set; }
        public Boolean Projector { get; set; }
        public Boolean Table { get; set; }
        public Boolean SmartTable { get; set; }
        public OsType OsType { get; set; }
        public Software Software { get; set; }

        public Subject()
        {
        }

        public Subject(int id, string name, Course course, string description, int numOfStudents, int minNumOfClassesPerTerm, int numOfClasses, bool projector, bool table, bool smartTable, OsType osType, Software software)
        {
            Id = id;
            Name = name;
            Course = course;
            Description = description;
            NumOfStudents = numOfStudents;
            MinNumOfClassesPerTerm = minNumOfClassesPerTerm;
            NumOfClasses = numOfClasses;
            Projector = projector;
            Table = table;
            SmartTable = smartTable;
            OsType = osType;
            Software = software;
        }
    }
}
