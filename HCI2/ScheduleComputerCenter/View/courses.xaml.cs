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
        string courseCode = "";

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
            dt.Columns.Add("Code");
            dt.Columns.Add("DateOfFounding");
            dt.Columns.Add("Description");
            foreach (Course c in coursesList)
            {
                DataRow dr = dt.NewRow();
                dr["Description"] = c.Description;
                dr["Name"] = c.Name;
                dr["Code"] = c.Code;
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
                if (code.Text.Equals("") || nameCourse.Text.Equals("") || yearOfFounding.Text.Equals(""))
                {
                    
                    MessageBox.Show("Some obligatory fields are empty");

                }
                else
                {
                    if (UniqueCode(code.Text))
                    {
                        string Name = nameCourse.Text;
                        string Description = desc.Text;
                        string DateOfFounding = yearOfFounding.Text;
                        string Code = code.Text;
                        Course course = new Course(Name, Code, DateOfFounding, Description);
                        ComputerCentre.CourseRepository.Add(course);
                        ComputerCentre.CourseRepository.Context.SaveChanges();
                        MessageBox.Show("Successfully added course");
                        btnAdd.Content = "Add";
                        view();
                    }

                    else
                    {
                        MessageBox.Show("Course code has to be unique");
                    }

                }
            }
            else
            {
                
                if (!(code.Text).Equals(courseCode) & !UniqueCode(code.Text))
                {
                    MessageBox.Show("Course code has to be unique");
                }
                else
                {
                    if (code.Text.Equals("") || nameCourse.Text.Equals("") || yearOfFounding.Text.Equals(""))
                    {
                        MessageBox.Show("Some obligatory fields are empty");

                    }
                    else
                    {

                    }
                }

            }
        }

        public int FindID(string code)
        {
            foreach (Course c in coursesList)
            {
                if (code.Equals(c.Code))
                {
                    return c.Id;
                }
            }
            return 0;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                desc.Text = dataRowView["Description"].ToString();
                code.Text = dataRowView["Code"].ToString();
                nameCourse.Text = dataRowView["Name"].ToString();
                yearOfFounding.Text = dataRowView["DateOfFounding"].ToString();
                btnAdd.Content = "Update";
                courseCode = code.Text;
            }
            else
            {
                MessageBox.Show("Please select any course from the list..");
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            btnAdd.Content = "Add";
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                String code = dataRowView["Code"].ToString();
                foreach (Course c in coursesList)
                {
                    if (code.Equals(c.Code))
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
            {
                MessageBox.Show("Please Select Any Course From the list...");
            }
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            desc.Text = "";
            code.Text = "";
            nameCourse.Text = "";
            yearOfFounding.Text = "";
            btnAdd.Content = "Add";
        }
        public Boolean UniqueCode(String code)
        {
            foreach (Course c in coursesList)
            {
                if (code.Equals(c.Code)) return false;
            }
            return true;
        }
        
    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}
        
}