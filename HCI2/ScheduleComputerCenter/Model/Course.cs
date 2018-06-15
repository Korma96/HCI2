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
        public string Mark { get; set; }

        public Course()
        {
        }

        public Course(string name,string code, string dateOfFounding, string description)
        {
            Name = name;
            Code = code;
            DateOfFounding = dateOfFounding;
            Description = description;
            Mark = makeMark(Name); ;
        }

        private string makeMark(string courseName)
        {
            string[] tokens = System.Text.RegularExpressions.Regex.Split(courseName, @"\s{2,}");
            switch (tokens.Length)
            {
                case 0: return "***";
                case 1: return tokens[0].Substring(0, 3);
                case 2: return tokens[0][0] + tokens[1].Substring(0, 2);
                case 3: return "" + tokens[0][0] + tokens[1][0] + tokens[2][0];
                default: return "" + tokens[0][0] + tokens[1][0] + tokens[2][0];
            }
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
