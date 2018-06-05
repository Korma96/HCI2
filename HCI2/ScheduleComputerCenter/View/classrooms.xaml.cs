using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ScheduleComputerCenter.Repository;

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for classrooms.xaml
    /// </summary>
    public partial class classrooms : Window
    {
        DataTable dt;
        List<Classroom> classroomsList = new List<Classroom>();

        public classrooms()
        {
        
            InitializeComponent();
            view();
        }

        public void view()
        {
            classroomsList = ComputerCentre.ClassroomRepository.GetAll().ToList();
            dt = new DataTable();
            dt.Columns.Add("Description");
            dt.Columns.Add("Name");
            dt.Columns.Add("NumOfSeats");
            dt.Columns.Add("Projector");
            dt.Columns.Add("Table");
            dt.Columns.Add("SmartTable");
            dt.Columns.Add("OsType");
            dt.Columns.Add("Software");
            foreach (Classroom cr in classroomsList)
            {
                DataRow dr = dt.NewRow();
                dr["Description"] = cr.Description;
                dr["Name"] = cr.Name;
                dr["NumOfSeats"] = cr.NumOfSeats;
                dr["Projector"] = cr.Projector;
                dr["Table"] = cr.Table;
                dr["OsType"] = cr.OsType;
                dr["SmartTable"] = cr.SmartTable;
                dr["Softwares"] = cr.Softwares;
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
       

        public void loadSoftwares(object sender, RoutedEventArgs e)
        {

            List<Software> softwares = ComputerCentre.SoftwareRepository.GetAll().ToList();
            var combo = sender as ComboBox;
            combo.ItemsSource = softwares;
            combo.SelectedIndex = 0;

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (btnAdd.Content.Equals("Add"))
            {
                if (!projector.Text.Equals("Projector in classroom") || !smartTable.Text.Equals("Smart table in classroom") || !table.Text.Equals("Table in classroom") || !osType.Text.Equals("OS in classroom"))
                {
                    Classroom classroom = new Classroom();
                    classroom.Name = name.Text;
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
                    softwares.Add((Software) software.SelectedItem);
                    classroom.Softwares = softwares ;

                    if (UniqueName(name.Text))
                    {
                        ComputerCentre.ClassroomRepository.Add(classroom);
                        ComputerCentre.ClassroomRepository.Context.SaveChanges();
                        MessageBox.Show("Successfully added classroom");
                        btnAdd.Content = "Add";
                        view();
                    }
                    else
                    {
                        MessageBox.Show("Name of classroom has to be unique");
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
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView dataRowView = (DataRowView) gvData.SelectedItems[0];
                desc.Text = dataRowView["Description"].ToString();
                table.Text = dataRowView["Table"].ToString();
                smartTable.Text = dataRowView["SmartTable"].ToString();
                projector.Text = dataRowView["Projector"].ToString();
                osType.Text = dataRowView["OsType"].ToString();
                software.Text = dataRowView["Software"].ToString();
                numOfSeats.Text = dataRowView["NumOfSeats"].ToString();
                btnAdd.Content = "Update";
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
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                String name = dataRowView["name"].ToString();
                foreach (Classroom cr in classroomsList)
                {
                    if (name.Equals(cr.Name))
                    {
                        ComputerCentre.ClassroomRepository.Remove(cr);
                        ComputerCentre.ClassroomRepository.Context.SaveChanges();
                        view();
                        break;
                    }
                }


            }
            else 
            MessageBox.Show("Please Select Any Classroom From the list...");
        }
        public Boolean UniqueName(String name)
        {
            foreach (Classroom cr in classroomsList)
            {
                if (name.Equals(cr.Name)) return false;
            }
            return true;
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
