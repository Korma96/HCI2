using ScheduleComputerCenter.Model;
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

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for courses.xaml
    /// </summary>
    public partial class courses : Window
    {
        DataTable dt;
        List<Course> coursesList = new List<Course>();

        public courses()
        {
            InitializeComponent();
            view();
        }
        public void view()
        {
            coursesList = ComputerCentre.CourseRepository.GetAll().ToList();
            dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("DateOfFounding");
            dt.Columns.Add("Description");
            foreach (Course c in coursesList)
            {
                DataRow dr = dt.NewRow();
                dr["Description"] = c.Description;
                dr["Name"] = c.Name;
                dr["DateOfFounding"] = c.DateOfFounding;
                dt.Rows.Add(dr);
            }
            gvData.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                noCourses.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                noCourses.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (btnAdd.Content.Equals("Add"))
            {
                
                string Name = nameCourse.Text;
                string Description = desc.Text;
                string DateOfFounding = yearOfFounding.Text;
                Course course = new Course(Name,Description,DateOfFounding);
                ComputerCentre.CourseRepository.Add(course);
                ComputerCentre.CourseRepository.Context.SaveChanges();
                
                MessageBox.Show("Successfully added course");
                btnAdd.Content = "Add";
                view();
            }
            else
            {

            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                desc.Text = dataRowView["Description"].ToString();
                nameCourse.Text = dataRowView["Table"].ToString();
                yearOfFounding.Text = dataRowView["YearOfFounding"].ToString();
                btnAdd.Content = "Update";
            }
            else
            {
                MessageBox.Show("Please select any course from the list..");
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                String name = dataRowView["name"].ToString();
                foreach (Course c in coursesList)
                {
                    if (name.Equals(c.Name))
                    {
                        ComputerCentre.CourseRepository.Remove(c);
                        ComputerCentre.CourseRepository.Context.SaveChanges();
                        MessageBox.Show("Successfully deleted course");
                        view();
                        break;
                    }
                }
            }
            else
                MessageBox.Show("Please Select Any Software From the list...");
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
        
}