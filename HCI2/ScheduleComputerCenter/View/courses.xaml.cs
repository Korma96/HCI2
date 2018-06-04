using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for courses.xaml
    /// </summary>
    public partial class courses : Window
    {
        public courses()
        {
            InitializeComponent();
        }
        private void TextBoxYear(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("(1|2)[^0-9]{3}").IsMatch(e.Text);
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please Select Any Classroom From List...");
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please Select Any Classroom From List...");
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please Select Any Classroom From List...");
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please Select Any Classroom From List...");
        }
    }
}