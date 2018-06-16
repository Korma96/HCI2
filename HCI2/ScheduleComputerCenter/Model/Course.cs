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
        public string Code { get; set; }
        public string DateOfFounding { get; set; }
        public string Description { get; set; }
        public Course()
        {
        }

        public Course(string name,string code, string dateOfFounding, string description)
        {
            Name = name;
            Code = code;
            DateOfFounding = dateOfFounding;
            Description = description;
            
        }

        
        public override string ToString()
        {
            return Code;
        }
    }
}
