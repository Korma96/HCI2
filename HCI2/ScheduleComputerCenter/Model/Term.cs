using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleComputerCenter.Model
{
    public class Term
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int DayId { get; set; }
        public Day Day { get; set; }
        public Subject Subject { get; set; }
        public int RowSpan { get; set; }

        public Term()
        {

        }

        public Term(string startTimeStr, string endTimeStr, Subject subject, Day day)
        {
            this.StartTime = TimeSpan.Parse(startTimeStr);
            this.EndTime = TimeSpan.Parse(endTimeStr);
            this.Subject = subject;
            this.Day = day;
            this.RowSpan = 1;
        }
    }
}
