using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<MappedAppointment> MappedAppointments { get; set; }
        public ObservableCollection<DateTime> VisibleDays { get; set; } 

        public MainWindow()
        {
            InitializeComponent();

            MappedAppointments = new ObservableCollection<MappedAppointment>
            {
            new MappedAppointment{MappedSubject = "Meeting", MappedStartTime = DateTime.Now.Date.AddHours(10),
                             MappedEndTime = DateTime.Now.Date.AddHours(11)},

            new MappedAppointment{MappedSubject = "Conference", MappedStartTime = DateTime.Now.Date.AddHours(15),
                             MappedEndTime = DateTime.Now.Date.AddHours(16)},
            };

            VisibleDays = new ObservableCollection<DateTime>
            {
                new DateTime(2018, 5, 28),
                new DateTime(2018, 5, 29),
                new DateTime(2018, 5, 30),
                new DateTime(2018, 5, 31),
                new DateTime(2018, 6, 1),
                new DateTime(2018, 6, 2)
            };
            
            this.DataContext = this;
            //this.schedule.VisibleDatesChanging -= ;
        }

    }

    public class MappedAppointment
    {
        public string MappedSubject { get; set; }

        public DateTime MappedStartTime { get; set; }

        public DateTime MappedEndTime { get; set; }
    }
}
