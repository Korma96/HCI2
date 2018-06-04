﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleComputerCenter.Model
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumOfSeats { get; set; }
        public Boolean Projector { get; set; }
        public Boolean Table { get; set; }
        public Boolean SmartTable { get; set; }
        public OsType OsType { get; set; }
        public Software Software { get; set; }

        public Classroom()
        {
        }

        public Classroom(string description,string name, int numOfSeats, bool projector, bool table, bool smartTable, OsType osType, Software software)
        {
            Name = name;
            Description = description;
            NumOfSeats = numOfSeats;
            Projector = projector;
            Table = table;
            SmartTable = smartTable;
            OsType = osType;
            Software = software;
        }

    }
}