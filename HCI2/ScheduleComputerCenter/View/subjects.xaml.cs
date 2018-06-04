using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using ScheduleComputerCenter.Model;

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for Subject.xaml
    /// </summary>
    public partial class SubjectsWindow : Window
    {
        DataTable dt;
        List<Subject> subjectsList = new List<Subject>();


        public SubjectsWindow()
        {
            InitializeComponent();
            view();
        }
        public void view()
        {
            dt = new DataTable();
            dt.Columns.Add("Description");
            dt.Columns.Add("Name");
            dt.Columns.Add("NumOfStudents");
            dt.Columns.Add("Course");
            dt.Columns.Add("MinNumOfClassesPerTerm");
            dt.Columns.Add("NumOfClasses");
            dt.Columns.Add("Table");
            dt.Columns.Add("SmartTable");
            dt.Columns.Add("OsType");
            dt.Columns.Add("Software");
            foreach (Subject s in subjectsList)
            {
                DataRow dr = dt.NewRow();
                dr["Description"] = s.Description;
                dr["Name"] = s.Name;
                dr["NumOfStudents"] = s.NumOfStudents;
                dr["Course"] = s.Course;
                dr["MinNumOfClassesPerTerm"] = s.Course;
                dr["NumOfClasses"] = s.Course;
                dr["Projector"] = s.MinNumOfClassesPerTerm;
                dr["Table"] = s.Table;
                dr["OsType"] = s.OsType;
                dr["SmartTable"] = s.SmartTable;
                dr["Software"] = s.Software;
                dt.Rows.Add(dr);
            }
            gvData.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                noSubjects.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                noSubjects.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        public void loadCourses(object sender, RoutedEventArgs e)
        {
            List<Course> courses = new List<Course>();
            courses.Add(new Course("pe", "he","1996"));
            courses.Add(new Course("ss", "he","1996"));
            courses.Add(new Course("sx", "he","1996"));
            var combo = sender as ComboBox;
            combo.ItemsSource = courses;
            combo.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            {
                if (btnAdd.Content.Equals("Add"))
                {
                    if (!projector.Text.Equals("Projector") || !smartTable.Text.Equals("Smart table") || !table.Text.Equals("Table") || !osType.Text.Equals("OS"))
                    {
                        Subject subject = new Subject();
                        subject.Name = name.Text;
                        subject.Description = desc.Text;
                        subject.NumOfStudents = Int32.Parse(numOfStudents.Text);
                        if (smartTable.Text.Equals("YES")) subject.SmartTable = true;
                        else subject.SmartTable = false;
                        if (table.Text.Equals("YES")) subject.Table = true;
                        else
                            subject.Table = false;
                        if (projector.Text.Equals("YES")) subject.Projector = true;
                        else subject.Projector = false;
                        subject.OsType = getOsType(osType.Text);
                        subject.Software = new Software("1", OsType.Any, "1", "mm", 18, 200, "14");
                        subject.Id = 1;
                        subjectsList.Add(subject);
                        MessageBox.Show("Successfully added subject");
                        btnAdd.Content = "Add";
                        view();
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
        }
        public OsType getOsType(string ostype)
        {
            if (ostype.Equals("LINUX")) return OsType.Linux;
            else if (ostype.Equals("WINDOWS")) return OsType.Windows;
            else return OsType.Any;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                desc.Text = dataRowView["Description"].ToString();
                table.Text = dataRowView["Table"].ToString();
                smartTable.Text = dataRowView["SmartTable"].ToString();
                projector.Text = dataRowView["Projector"].ToString();
                osType.Text = dataRowView["OsType"].ToString();
                software.Text = dataRowView["Software"].ToString();
                numOfStudents.Text = dataRowView["NumOfSeats"].ToString();
                btnAdd.Content = "Update";
            }
            else
            {
                MessageBox.Show("Please select any classroom from the list..");
            }
        }
        public void loadSoftwares(object sender, RoutedEventArgs e)
        {
            List<Software> softwares = new List<Software>();
            softwares.Add(new Software("1", OsType.Any, "1", "mm", 18, 200, "14"));
            softwares.Add(new Software("2", OsType.Any, "1", "mm", 18, 200, "14"));
            softwares.Add(new Software("3", OsType.Any, "1", "mm", 18, 200, "14"));
            var combo = sender as ComboBox;
            combo.ItemsSource = softwares;
            combo.SelectedIndex = 0;

        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                String name = dataRowView["name"].ToString();
                foreach (Subject s in subjectsList)
                {
                    if (name.Equals(s.Name))
                    {
                        subjectsList.Remove(s);
                        view();
                        break;
                    }
                }
            }
            else
                MessageBox.Show("Please Select Any Subject From The list...");
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
