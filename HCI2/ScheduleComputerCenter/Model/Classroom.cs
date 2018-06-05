using System;
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
        public bool Projector { get; set; }
        public bool Table { get; set; }
        public bool SmartTable { get; set; }
        public OsType OsType { get; set; }
        public List<Software> Softwares { get; set; }

        public Classroom()
        {
        }

        public Classroom(string name, string description, int numOfSeats, bool projector, bool table, bool smartTable, OsType osType, List<Software> softwares)
        {
            Name = name;
            Description = description;
            NumOfSeats = numOfSeats;
            Projector = projector;
            Table = table;
            SmartTable = smartTable;
            OsType = osType;
            Softwares = softwares;
        }


    }
}