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
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for Subject.xaml
    /// </summary>
    public partial class SubjectsWindow : Window
    {
        DataTable dt;
        public List<Subject> SubjectsList { get; set; }
        public ObservableCollection<Subject> ObservableSubjectsList { get; set; }
        public List<Software> Softwares { get; set; }
        string subjectCode = "";


        public SubjectsWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            SubjectsList = ComputerCentre.context.Subjects.Include(s => s.Softwares).ToList();
            ObservableSubjectsList = new ObservableCollection<Subject>(SubjectsList);
            gvData.ItemsSource = ObservableSubjectsList;

            view();
        }
        public void view()
        {

            if (gvData.Items.Count > 0)
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

        public static string SoftwaresToString(List<Software> soft)
        {
       
            if (soft != null)
            {

                if (soft.Count > 0)
                {
                    string value = "";

                    foreach (Software s in soft)
                    {
                        value += ", " + s.Code;
                    }

                    return value.Substring(2);
                }
            }
            return "No softwares";
            
        }

        public void loadCourses(object sender, RoutedEventArgs e)
        {
            List<Course> courses = ComputerCentre.CourseRepository.GetAll().ToList();
            var combo = sender as ComboBox;
            combo.ItemsSource = courses;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            {
                if (btnAdd.Content.Equals("Add"))
                {
                    if (numOfStudents.Text.Equals("") || numOF.Text.Equals("") || Course.Text.Equals("") || softwareCombo.Text.Equals("") || minNumOfClasses.Text.Equals("") || osType.Text.Equals("") || name.Text.Equals("") || code.Text.Equals(""))
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
                        subject.Course = (Course)Course.SelectedItem;
                        subject.MinNumOfClassesPerTerm = Int32.Parse(minNumOfClasses.Text);
                        subject.NumOfClasses = Int32.Parse(numOF.Text);
                        List<Software> softwares = new List<Software>();
                        CheckBox checkBox;
                        StringBuilder sb = new StringBuilder();

                        for (int i = 0; i < softwareCombo.Items.Count; i++)
                        {
                            checkBox = (softwareCombo.Items[i] as ComboBoxItem).Content as CheckBox;
                            if (checkBox != null)
                            {
                                if (checkBox.IsChecked.Value)
                                {
                                    softwares.Add(Softwares[i - 1]);
                                }
                            }

                        }
                        subject.Softwares = softwares;
                        if (UniqueCode(code.Text))
                        {
                            ComputerCentre.SubjectRepository.Add(subject);
                            ComputerCentre.SubjectRepository.Context.SaveChanges();
                            MessageBox.Show("Successfully added subject");
                            btnAdd.Content = "Add";
                            ObservableSubjectsList.Add(subject);
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
                    if (numOfStudents.Text.Equals("") || numOF.Text.Equals("") || Course.Text.Equals("") || softwareCombo.Text.Equals("") || minNumOfClasses.Text.Equals("") || osType.Text.Equals("") || name.Text.Equals("") || code.Text.Equals(""))
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
                            List<Software> softwares = new List<Software>();
                            CheckBox checkBox;
                            StringBuilder sb = new StringBuilder();

                            for (int i = 0; i < softwareCombo.Items.Count; i++)
                            {
                                checkBox = (softwareCombo.Items[i] as ComboBoxItem).Content as CheckBox;
                                if (checkBox != null)
                                {
                                    if (checkBox.IsChecked.Value)
                                    {
                                        softwares.Add(Softwares[i - 1]);
                                    }
                                }

                            }
                            ComputerCentre.SubjectRepository.Get(id).Softwares = softwares;
                            ComputerCentre.SubjectRepository.Get(id).Course = (Course)Course.SelectedItem;
                            ComputerCentre.SubjectRepository.Get(id).MinNumOfClassesPerTerm = Int32.Parse(minNumOfClasses.Text);
                            ComputerCentre.SubjectRepository.Get(id).NumOfClasses = Int32.Parse(numOF.Text);
                            ComputerCentre.SubjectRepository.Context.SaveChanges();
                            ObservableSubjectsList.Clear();
                            foreach (Subject subject in ComputerCentre.SubjectRepository.GetAll().ToList()) ObservableSubjectsList.Add(subject);
                            MessageBox.Show("Successfully updated subject");
                            btnAdd.Content = "Add";
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
            foreach (Subject s in SubjectsList)
            {
                if (code.Equals(s.Code)) return false;
            }
            return true;
        }

        public void loadSoftwares(object sender, RoutedEventArgs e)
        {
            Softwares = ComputerCentre.SoftwareRepository.GetAll().ToList();
            var combo = sender as ComboBox;
            combo.Items.Add(new ComboBoxItem() { IsSelected = true, Content = "No softwares selected" });

            CheckBox checkBox;

            foreach (Software s in Softwares)
            {
                checkBox = new CheckBox() { Content = s.Code };
                checkBox.Checked += CheckBox_Checked;
                checkBox.Unchecked += CheckBox_Checked;
                combo.Items.Add(new ComboBoxItem() { Content = checkBox });
            }
            // combo.ItemTemplate = SubjectsWindow.makeDataTemplate();

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox;
            StringBuilder sb = new StringBuilder("");

            bool atLeastOneChecked = false;

            foreach (ComboBoxItem cbi in softwareCombo.Items)
            {
                checkBox = cbi.Content as CheckBox;
                if (checkBox != null)
                {
                    if (checkBox.IsChecked.Value)
                    {
                        sb.Append(", " + checkBox.Content.ToString());
                        atLeastOneChecked = true;
                    }
                }

            }

            if (atLeastOneChecked) (softwareCombo.Items[0] as ComboBoxItem).Content = sb.ToString().Substring(2);
            else (softwareCombo.Items[0] as ComboBoxItem).Content = "No softwares selected";
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                Subject dataRowView = (Subject)gvData.SelectedItems[0];
                desc.Text = dataRowView.Description;
                name.Text = dataRowView.Name;
                code.Text = dataRowView.Code;
                if (dataRowView.Table) table.Text = "YES";
                else table.Text = "NO";
                if (dataRowView.Table) smartTable.Text = "YES";
                else smartTable.Text = "NO";
                if (dataRowView.Projector) projector.Text = "YES";
                else projector.Text = "NO";
                osType.Text = dataRowView.OsType.ToString();

                CheckBox checkBox;

                foreach (ComboBoxItem cbi in softwareCombo.Items)
                {

                    checkBox = cbi.Content as CheckBox;
                    if (checkBox != null) checkBox.IsChecked = false;
                }

                foreach (Software s in dataRowView.Softwares)
                {
                    foreach (ComboBoxItem cbi in softwareCombo.Items)
                    {
                        checkBox = cbi.Content as CheckBox;
                        if(checkBox != null)
                        {
                            if (s.Code.Equals(checkBox.Content)) checkBox.IsChecked = true;
                        }
                        
                    }
                }
                softwareCombo.Text = SoftwaresToString(dataRowView.Softwares);
                numOfStudents.Text = dataRowView.NumOfStudents.ToString();
                minNumOfClasses.Text = dataRowView.MinNumOfClassesPerTerm.ToString();
                numOF.Text = dataRowView.NumOfClasses.ToString();
                Course.Text = dataRowView.Course.ToString();
                btnAdd.Content = "Update";
                subjectCode = dataRowView.Code;
            }
            else
            {
                MessageBox.Show("Please select any classroom from the list..");
            }
        }

        public int FindID(string code)
        {
            foreach (Subject s in SubjectsList)
            {
                if (code.Equals(s.Code))
                {
                    return s.Id;
                }
            }
            return 0;
        }
        
     
        

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                Subject dataRowView = (Subject)gvData.SelectedItems[0];
                ComputerCentre.SubjectRepository.Remove(dataRowView);
                ObservableSubjectsList.Remove(dataRowView);
                ComputerCentre.SubjectRepository.Context.SaveChanges();
                view();
                if (btnAdd.Content.Equals("Update")) 
                {
                    btnAdd.Content = "Add";
                    Empty();
                }
                MessageBox.Show("Successfully deleted subject");
                

            }
            else
                MessageBox.Show("Please Select Any Subject From The list...");
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Empty();
        }
        private void Empty()
        {
            desc.Text = "";
            name.Text = "";
            code.Text = "";
            table.Text = "";
            smartTable.Text = "";
            projector.Text = "";
            osType.Text = "";
            CheckBox checkBox;
            foreach (ComboBoxItem cbi in softwareCombo.Items)
            {
                checkBox = cbi.Content as CheckBox;
                if (checkBox != null)
                {
                    checkBox.IsChecked = false;
                }

            }
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
        private void softwareCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ComboBox).SelectedIndex = 0;
        }

       
    }
}
