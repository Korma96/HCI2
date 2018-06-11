using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleComputerCenter.Model
{
    public class Software
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public OsType OsType { get; set; }
        public String Manufacturer { get; set; }
        public String Website { get; set; }
        public int YearOfFounding { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }

        public Software()
        {
        }

        public Software(string name,string code, OsType osType, string manufacturer, string website, int yearOfFounding, int price, string description)
        {
            Name = name;
            Code = code;
            OsType = osType;
            Manufacturer = manufacturer;
            Website = website;
            YearOfFounding = yearOfFounding;
            Price = price;
            Description = description;
        }
    
    public override string ToString()
    {
        return Name;
    }
}

}
