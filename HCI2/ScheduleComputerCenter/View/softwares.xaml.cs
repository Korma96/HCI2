using System;
using System.Collections.Generic;
using System.Data;
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
using ScheduleComputerCenter.Model;

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for softwares.xaml
    /// </summary>
    public partial class softwares : Window
    {
        DataTable dt;
        List<Software> softwaresList = new List<Software>();

        public softwares()
        {
            InitializeComponent();
            view();
        }
        public void view()
        {
            dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("OsType");
            dt.Columns.Add("Manufacturer");
            dt.Columns.Add("Website");
            dt.Columns.Add("YearOfFounding");
            dt.Columns.Add("Price");
            dt.Columns.Add("Description");
            foreach (Software s in softwaresList)
            {
                DataRow dr = dt.NewRow();
                dr["Description"] = s.Description;
                dr["Name"] = s.Name;
                dr["Manufacturer"] = s.Manufacturer;
                dr["Website"] = s.Website;
                dr["YearOfFounding"] = s.YearOfFounding;
                dr["Price"] = s.Price;
                dr["OsType"] = s.OsType;
                dt.Rows.Add(dr);
            }
            gvData.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                noSoftwares.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                noSoftwares.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void textBoxYear(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("(1|2)[^0-9]{3}").IsMatch(e.Text);
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (btnAdd.Content.Equals("Add"))
            {
                if (!osType.Text.Equals("OS"))
                {
                    Software software = new Software();
                    software.Name = nameSoftware.Text;
                    software.Description = desc.Text;
                    software.Manufacturer = manufacturer.Text;
                    software.OsType = getOsType(osType.Text);
                    software.Id = 1;

                    if (UniqueName(nameSoftware.Text))
                    {
                        softwaresList.Add(software);
                        MessageBox.Show("Successfully added software");
                        btnAdd.Content = "Add";
                        view();
                    }
                    else
                    {
                        MessageBox.Show("Name of software has to be unique");
                    }

                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
            else
            {

            }
        }
        public OsType getOsType(string ostype)
        {
            if (ostype.Equals("LINUX")) return OsType.Linux;
            else if (ostype.Equals("WINDOWS")) return OsType.Windows;
            else return OsType.Any;
        }

        public Boolean UniqueName(String name)
        {
            foreach (Software s in softwaresList)
            {
                if (name.Equals(s.Name)) return false;
            }
            return true;
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
