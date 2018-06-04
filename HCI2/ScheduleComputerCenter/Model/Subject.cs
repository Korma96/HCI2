﻿using System;
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
        public string Mark { get; set; }
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

        public Subject(string name, Course course, string description, int numOfStudents, int minNumOfClassesPerTerm, int numOfClasses, bool projector, bool table, bool smartTable, OsType osType, Software software)
        {
            Name = name;
            Course = course;
            Mark = makeMark(course.Name);
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

        private string makeMark(string courseName)
        {
            string[] tokens = System.Text.RegularExpressions.Regex.Split(courseName, @"\s{2,}");
           switch(tokens.Length)
            {
                case 0: return "***";
                case 1: return tokens[0].Substring(0, 3);
                case 2: return tokens[0][0] + tokens[1].Substring(0, 2);
                case 3: return "" + tokens[0][0] + tokens[1][0] + tokens[2][0];
                default: return "" + tokens[0][0] + tokens[1][0] + tokens[2][0];
            }
        }
    }
}
