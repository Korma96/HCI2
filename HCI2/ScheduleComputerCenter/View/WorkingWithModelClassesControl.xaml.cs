using System;
using System.Collections.Generic;
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

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for WorkingWithModelClassesControl.xaml
    /// </summary>
    public partial class WorkingWithModelClassesControl : UserControl
    {
        public WorkingWithModelClassesControl()
        {
            InitializeComponent();
        }

        private void ImageLoaded(object sender, RoutedEventArgs e)
        {
            // ... Create a new BitmapImage.
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri("/ScheduleComputerCenter;component/Images/model.jpg", UriKind.RelativeOrAbsolute);
            b.EndInit();

            // ... Get Image reference from sender.
            var image = sender as Image;
            // ... Assign Source.
            image.Source = b;
        }

        private void ImageLoaded2(object sender, RoutedEventArgs e)
        {
            // ... Create a new BitmapImage.
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri("/ScheduleComputerCenter;component/Images/model2.jpg", UriKind.RelativeOrAbsolute);
            b.EndInit();

            // ... Get Image reference from sender.
            var image = sender as Image;
            // ... Assign Source.
            image.Source = b;
        }
    }
}
