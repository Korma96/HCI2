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
        public string Code { get; set; }
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
        public List<Software> Softwares { get; set; }

        public Subject()
        {
        }

        public Subject(string name,string code, Course course, string description, int numOfStudents, int minNumOfClassesPerTerm, int numOfClasses, bool projector, bool table, bool smartTable, OsType osType, List<Software> softwares)
        {
            Code = code;
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
            Softwares = softwares;
        }

        
    }
}
