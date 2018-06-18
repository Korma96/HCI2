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
    /// Interaction logic for MoveTermControl.xaml
    /// </summary>
    public partial class MoveTermControl : UserControl
    {
        public MoveTermControl()
        {
            InitializeComponent();
            MediaElement.LoadedBehavior = MediaState.Manual;
            MediaElement.Play();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MediaElement.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MediaElement.Pause();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MediaElement.Stop();
        }
    }
}
