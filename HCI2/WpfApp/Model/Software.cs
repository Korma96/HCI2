using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Model
{
    class Software
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public OsType OsType { get; set; }
        public String Manufacturer { get; set; }
        public String Website { get; set; }
        public int YearOfFounding { get; set; }
        public int price { get; set; }
        public string Description { get; set; }

        public Software()
        {
        }

        public Software(int id, string name, OsType osType, string manufacturer, string website, int yearOfFounding, int price, string description)
        {
            Id = id;
            Name = name;
            OsType = osType;
            Manufacturer = manufacturer;
            Website = website;
            YearOfFounding = yearOfFounding;
            this.price = price;
            Description = description;
        }
    }
}
