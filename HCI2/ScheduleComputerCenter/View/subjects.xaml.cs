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
using ScheduleComputerCenter.Repository;

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for Subject.xaml
    /// </summary>
    public partial class SubjectsWindow : Window
    {
        DataTable dt;
        List<Subject> subjectsList = new List<Subject>();
        string subjectCode = "";


        public SubjectsWindow()
        {
            InitializeComponent();
            view();
        }
        public void view()
        {
            subjectsList = ComputerCentre.SubjectRepository.GetAll().ToList();
            dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Code");
            dt.Columns.Add("Description");
            dt.Columns.Add("NumOfStudents");
            dt.Columns.Add("Course");
            dt.Columns.Add("MinNumOfClassesPerTerm");
            dt.Columns.Add("NumOfClasses");
            dt.Columns.Add("Table");
            dt.Columns.Add("SmartTable");
            dt.Columns.Add("Projector");
            dt.Columns.Add("OsType");
            dt.Columns.Add("Softwares");

            foreach (Subject s in subjectsList)
            {
                DataRow dr = dt.NewRow();
                dr["Description"] = s.Description;
                dr["Code"] = s.Code;
                dr["Name"] = s.Name;
                dr["NumOfStudents"] = s.NumOfStudents;
                dr["Course"] = s.Course;
                dr["MinNumOfClassesPerTerm"] = s.MinNumOfClassesPerTerm;
                dr["NumOfClasses"] = s.NumOfClasses;
                if (s.Projector) dr["Projector"] = "YES";
                else dr["Projector"] = "NO";
                if (s.Table) dr["Table"] = "YES";
                else dr["Table"] = "NO";
                dr["OsType"] = s.OsType;
                if (s.SmartTable) dr["SmartTable"] = "YES";
                else dr["SmartTable"] = "NO";
                dr["Softwares"] = s.Software;
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
            List<Course> courses = ComputerCentre.CourseRepository.GetAll().ToList();
            var combo = sender as ComboBox;
            combo.ItemsSource = courses;
            combo.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            {
                if (btnAdd.Content.Equals("Add"))
                {
                    if (numOfStudents.Text.Equals("") || numOF.Text.Equals("") || Course.Text.Equals("") || software.Text.Equals("") || minNumOfClasses.Text.Equals("") || osType.Text.Equals("") || name.Text.Equals("") || code.Text.Equals(""))
                    {
                        MessageBox.Show("Some obligatory fields are empty");
                    }
                    else
                    {
                        Subject subject = new Subject();
                        subject.Name = name.Text;
                        subject.Code = code.Text;
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
                        subject.Software = (Software)software.SelectedItem;
                        subject.Course = (Course)Course.SelectedItem;
                        subject.MinNumOfClassesPerTerm = Int32.Parse(minNumOfClasses.Text);
                        subject.NumOfClasses = Int32.Parse(numOF.Text);
                        if (UniqueCode(code.Text))
                        {
                            ComputerCentre.SubjectRepository.Add(subject);
                            ComputerCentre.SubjectRepository.Context.SaveChanges();
                            MessageBox.Show("Successfully added subject");
                            btnAdd.Content = "Add";
                            view();
                        }
                        else
                        {
                            MessageBox.Show("Subject code has to be unique");
                        }
                    }
                }
                else
                {
                    int id = FindID(subjectCode);
                    if (numOfStudents.Text.Equals("") || numOF.Text.Equals("") || Course.Text.Equals("") || software.Text.Equals("") || minNumOfClasses.Text.Equals("") || osType.Text.Equals("") || name.Text.Equals("") || code.Text.Equals(""))
                    {
                        MessageBox.Show("Some obligatory fields are empty");
                    }
                    else
                    {
                        if (!code.Text.Equals(subjectCode) && !UniqueCode(code.Text))
                        {
                            MessageBox.Show("Subject code has to be unique");
                        }
                        else
                        {
                            ComputerCentre.SubjectRepository.Get(id).Name = name.Text;
                            ComputerCentre.SubjectRepository.Get(id).Code = code.Text;
                            ComputerCentre.SubjectRepository.Get(id).Description = desc.Text;
                            ComputerCentre.SubjectRepository.Get(id).NumOfStudents = Int32.Parse(numOfStudents.Text);
                            if (smartTable.Text.Equals("YES")) ComputerCentre.SubjectRepository.Get(id).SmartTable = true;
                            else ComputerCentre.SubjectRepository.Get(id).SmartTable = false;
                            if (table.Text.Equals("YES")) ComputerCentre.SubjectRepository.Get(id).Table = true;
                            else
                                ComputerCentre.SubjectRepository.Get(id).Table = false;
                            if (projector.Text.Equals("YES")) ComputerCentre.SubjectRepository.Get(id).Projector = true;
                            else ComputerCentre.SubjectRepository.Get(id).Projector = false;
                            ComputerCentre.SubjectRepository.Get(id).OsType = getOsType(osType.Text);
                            ComputerCentre.SubjectRepository.Get(id).Software = (Software)software.SelectedItem;
                            ComputerCentre.SubjectRepository.Get(id).Course = (Course)Course.SelectedItem;
                            ComputerCentre.SubjectRepository.Get(id).MinNumOfClassesPerTerm = Int32.Parse(minNumOfClasses.Text);
                            ComputerCentre.SubjectRepository.Get(id).NumOfClasses = Int32.Parse(numOF.Text);
                            ComputerCentre.SubjectRepository.Context.SaveChanges();
                            MessageBox.Show("Successfully edited subject");
                            btnAdd.Content = "Add";
                            view();
                        }
                    }

                    }
                }
        }
        public OsType getOsType(string ostype)
        {
            if (ostype.Equals("LINUX")) return OsType.LINUX;
            else if (ostype.Equals("WINDOWS")) return OsType.WINDOWS;
            else return OsType.ANY;
        }

        public Boolean UniqueCode(String code)
        {
            foreach (Subject s in subjectsList)
            {
                if (code.Equals(s.Code)) return false;
            }
            return true;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                desc.Text = dataRowView["Description"].ToString();
                name.Text = dataRowView["Name"].ToString();
                code.Text = dataRowView["Code"].ToString();
                table.Text = dataRowView["Table"].ToString();
                smartTable.Text = dataRowView["SmartTable"].ToString();
                projector.Text = dataRowView["Projector"].ToString();
                osType.Text = dataRowView["OsType"].ToString();
                software.Text = dataRowView["Softwares"].ToString();
                numOfStudents.Text = dataRowView["NumOfStudents"].ToString();
                minNumOfClasses.Text = dataRowView["MinNumOfClassesPerTerm"].ToString();
                numOF.Text = dataRowView["NumOfClasses"].ToString();
                Course.Text = dataRowView["Course"].ToString();
                btnAdd.Content = "Update";
                subjectCode = code.Text;
            }
            else
            {
                MessageBox.Show("Please select any classroom from the list..");
            }
        }

        public int FindID(string code)
        {
            foreach (Subject s in subjectsList)
            {
                if (code.Equals(s.Code))
                {
                    return s.Id;
                }
            }
            return 0;
        }

        public void loadSoftwares(object sender, RoutedEventArgs e)
        {
            List<Software> softwares = ComputerCentre.SoftwareRepository.GetAll().ToList();
            var combo = sender as ComboBox;
            combo.ItemsSource = softwares;
            combo.SelectedIndex = 0;

        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                String code = dataRowView["Code"].ToString();
                foreach (Subject s in subjectsList)
                {
                    if (code.Equals(s.Code))
                    {
                        ComputerCentre.SubjectRepository.Remove(s);
                        ComputerCentre.SubjectRepository.Context.SaveChanges();
                        MessageBox.Show("Successfully deleted subject");
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
            desc.Text = "";
            name.Text = "";
            code.Text = "";
            table.Text = "";
            smartTable.Text = "";
            projector.Text = "";
            osType.Text = "";
            software.Text = "";
            numOfStudents.Text = "";
            minNumOfClasses.Text = "";
            numOF.Text = "";
            Course.Text = "";
            btnAdd.Content = "Add";
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
