using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleComputerCenter.Model
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public String DateOfFounding { get; set; }
        public String Description { get; set; }
        public String Mark { get; set; }

        public Course()
        {
        }

        public Course(string name, string dateOfFounding, string description)
        {
            Name = name;
            DateOfFounding = dateOfFounding;
            Description = description;
            Mark = mark;
        }
    }
}
