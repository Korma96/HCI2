using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for classrooms.xaml
    /// </summary>
    public partial class classrooms : Window
    {
        DataTable dt;
        List<Classroom> classroomsList = new List<Classroom>();
        public List<Software> Softwares;

        string classroomCode = "";

        public classrooms()
        {
            InitializeComponent();
            view();
        }

        public void view()
        {
            classroomsList = ComputerCentre.context.Classrooms.Include(s => s.Softwares).ToList();
            dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Code");
            dt.Columns.Add("Description");
            dt.Columns.Add("NumOfSeats");
            dt.Columns.Add("Projector");
            dt.Columns.Add("Table");
            dt.Columns.Add("SmartTable");
            dt.Columns.Add("OsType");
            dt.Columns.Add("Softwares");
            foreach (Classroom cr in classroomsList)
            {
                DataRow dr = dt.NewRow();
                if (cr.Description.Length > 40) dr["Description"] = cr.Description.Substring(1, 40) + "...";
                else dr["Description"] = cr.Description;
                dr["Name"] = cr.Name;
                dr["Code"] = cr.Code;
                dr["NumOfSeats"] = cr.NumOfSeats;
                if (cr.Projector) dr["Projector"] = "YES";
                else dr["Projector"] = "NO";
                if (cr.Table) dr["Table"] = "YES";
                else dr["Table"] = "NO";
                dr["OsType"] = cr.OsType;
                if (cr.SmartTable) dr["SmartTable"] = "YES";
                else dr["SmartTable"] = "NO";
                dr["Softwares"] = SoftwaresToString(cr.Softwares);
                dt.Rows.Add(dr);
            }
            gvData.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                noClassrooms.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                noClassrooms.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        List<Software> s = new List<Software>();
       
        public void loadSoftwares(object sender, RoutedEventArgs e)
        {
            Softwares = ComputerCentre.SoftwareRepository.GetAll().ToList();
            var combo = sender as ComboBox;
            combo.Items.Add(new ComboBoxItem() { IsSelected = true,  Content = "No softwares selected" });

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

        public string SoftwaresToString(List<Software> soft)
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (btnAdd.Content.Equals("Add"))
            {
                if (projector.Text.Equals("") || smartTable.Text.Equals("") || table.Text.Equals("") || osType.Text.Equals("") || name.Text.Equals("") || code.Text.Equals(""))
                {
                    MessageBox.Show("Some obligatory fields are empty");

                }
                else
                {
                    Classroom classroom = new Classroom();
                    classroom.Name = name.Text;
                    classroom.Code = code.Text;
                    classroom.Description = desc.Text;
                    classroom.NumOfSeats = Int32.Parse(numOfSeats.Text);
                    if (smartTable.Text.Equals("YES")) classroom.SmartTable = true;
                    else classroom.SmartTable = false;
                    if (table.Text.Equals("YES")) classroom.Table = true;
                    else
                        classroom.Table = false;
                    if (projector.Text.Equals("YES")) classroom.Projector = true;
                    else classroom.Projector = false;
                    classroom.OsType = getOsType(osType.Text);
                    List<Software> softwares = new List<Software>();

                    CheckBox checkBox;
                    StringBuilder sb = new StringBuilder();

                    for(int i = 0; i < softwareCombo.Items.Count; i++)
                    {
                        checkBox = (softwareCombo.Items[i] as ComboBoxItem).Content as CheckBox;
                        if (checkBox != null)
                        {
                            if (checkBox.IsChecked.Value)
                            {
                                softwares.Add(Softwares[i-1]);
                            }
                        }

                    }

                   
                    classroom.Softwares = softwares;

                    if (UniqueCode(code.Text))
                    {
                        ComputerCentre.ClassroomRepository.Add(classroom);
                        ComputerCentre.ClassroomRepository.Context.SaveChanges();
                        btnAdd.Content = "Add";
                        view();
                        MessageBox.Show("Successfully added classroom");
                        Empty();
                    }
                    else
                    {
                        MessageBox.Show("Code of classroom has to be unique");
                    }
                }
            }
            else
            {
                
                if (!(code.Text).Equals(classroomCode) && !UniqueCode(code.Text))
                {
                    MessageBox.Show("Classroom code has to be unique");
                }
                else
                {
                    if (numOfSeats.Text.Equals("") || smartTable.Text.Equals("") || table.Text.Equals("") || osType.Text.Equals("") || name.Text.Equals("") || code.Text.Equals(""))
                    {
                        MessageBox.Show("Some obligatory fields are empty");

                    }
                    else
                    {
                        int id = FindID(classroomCode);
                        ComputerCentre.ClassroomRepository.Get(id).Name = name.Text;
                        ComputerCentre.ClassroomRepository.Get(id).Code = code.Text;
                        ComputerCentre.ClassroomRepository.Get(id).Description = desc.Text;
                        ComputerCentre.ClassroomRepository.Get(id).NumOfSeats = Int32.Parse(numOfSeats.Text);
                        if (smartTable.Text.Equals("YES")) ComputerCentre.ClassroomRepository.Get(id).SmartTable = true;
                        else ComputerCentre.ClassroomRepository.Get(id).SmartTable = false;
                        if (table.Text.Equals("YES")) ComputerCentre.ClassroomRepository.Get(id).Table = true;
                        else
                            ComputerCentre.ClassroomRepository.Get(id).Table = false;
                        if (projector.Text.Equals("YES")) ComputerCentre.ClassroomRepository.Get(id).Projector = true;
                        else ComputerCentre.ClassroomRepository.Get(id).Projector = false;
                        ComputerCentre.ClassroomRepository.Get(id).OsType = getOsType(osType.Text);
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
                        ComputerCentre.ClassroomRepository.Get(id).Softwares = softwares;
                        ComputerCentre.ClassroomRepository.Context.SaveChanges();
                        btnAdd.Content = "Add";
                        view();
                        MessageBox.Show("Successfully updated classroom");
                        Empty();
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
        
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox;
            StringBuilder sb = new StringBuilder("");

            bool atLeastOneChecked = false;

            foreach(ComboBoxItem cbi in softwareCombo.Items)
            {
                checkBox = cbi.Content as CheckBox;
                if(checkBox != null)
                {
                    if (checkBox.IsChecked.Value)
                    {
                        sb.Append(", " + checkBox.Content.ToString());
                        atLeastOneChecked = true;
                    }
                }
                
            }

            if(atLeastOneChecked) (softwareCombo.Items[0] as ComboBoxItem).Content = sb.ToString().Substring(2);
            else (softwareCombo.Items[0] as ComboBoxItem).Content = "No softwares selected";
        }
        
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView dataRowView = (DataRowView) gvData.SelectedItems[0];
                desc.Text = dataRowView["Description"].ToString();
                code.Text = dataRowView["Code"].ToString();
                name.Text = dataRowView["Name"].ToString();
                table.Text = dataRowView["Table"].ToString();
                smartTable.Text = dataRowView["SmartTable"].ToString();
                projector.Text = dataRowView["Projector"].ToString();
                osType.Text = dataRowView["OsType"].ToString();
                CheckBox checkBox;
                int id = FindID(code.Text);

                foreach (ComboBoxItem cbi in softwareCombo.Items)
                {

                    checkBox = cbi.Content as CheckBox;
                    if (checkBox != null) checkBox.IsChecked = false;
                }

                foreach (Software s in ComputerCentre.ClassroomRepository.Get(id).Softwares)
                {
                    foreach (ComboBoxItem cbi in softwareCombo.Items)
                    {
                        checkBox = cbi.Content as CheckBox;
                        if (checkBox != null)
                        {
                            if (s.Code.Equals(checkBox.Content)) checkBox.IsChecked = true;
                        }

                    }
                }
                numOfSeats.Text = dataRowView["NumOfSeats"].ToString();
                btnAdd.Content = "Update";
                classroomCode = code.Text;
            }
            else
            {
                MessageBox.Show("Please select any classroom from the list..");
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                btnAdd.Content = "Add";
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                String code = dataRowView["Code"].ToString();
                foreach (Classroom cr in classroomsList)
                {
                    if (code.Equals(cr.Code))
                    {
                        ComputerCentre.ClassroomRepository.Remove(cr);
                        ComputerCentre.ClassroomRepository.Context.SaveChanges();
                        view();
                        if (btnAdd.Content.Equals("Update"))
                        {
                            Empty();
                            btnAdd.Content = "Add";
                        }
                        MessageBox.Show("Successfully deleted classroom");
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Select Any Classroom From the list...");
            }
        }
        public Boolean UniqueCode(String code)
        {
            foreach (Classroom cr in classroomsList)
            {
                if (code.Equals(cr.Code)) return false;
            }
            return true;
        }

        public int FindID(string code)
        {
            foreach (Classroom c in classroomsList)
            {
                if (code.Equals(c.Code))
                {
                    return c.Id;
                }
            }
            return 0;
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
            numOfSeats.Text = "";
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
