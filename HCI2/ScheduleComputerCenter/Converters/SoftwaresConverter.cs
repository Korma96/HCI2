using ScheduleComputerCenter.Model;
using ScheduleComputerCenter.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ScheduleComputerCenter.Converters
{
    public class SoftwaresConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Software> softwares = value as List<Software>;
            return SubjectsWindow.SoftwaresToString(softwares);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}
