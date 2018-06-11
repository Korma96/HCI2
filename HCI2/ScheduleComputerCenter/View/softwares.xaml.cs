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
    /// Interaction logic for softwares.xaml
    /// </summary>
    public partial class softwares : Window
    {
       

        DataTable dt;
        List<Software> softwaresList = new List<Software>();
        string softwareCode = "";

        public softwares()
        {
            InitializeComponent();
            view();
        }
        public void view()
        {
            softwaresList = ComputerCentre.SoftwareRepository.GetAll().ToList();
            dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Code");
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
                dr["Code"] = s.Code;
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

        private void TextBoxYear(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("(1|2)[^0-9]{3}").IsMatch(e.Text);
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (btnAdd.Content.Equals("Add"))
            {
                if (osType.Text.Equals("") || nameSoftware.Text.Equals("") || code.Text.Equals("") || yearOfFounding.Text.Equals("") || price.Text.Equals(""))
                {
                    MessageBox.Show("Some obligatory fields are empty");
                }
                else
                {
                    Software software = new Software();
                    software.Name = nameSoftware.Text;
                    software.Code = code.Text;
                    software.Description = desc.Text;
                    software.Manufacturer = manufacturer.Text;
                    software.OsType = getOsType(osType.Text);
                    software.Website = website.Text;
                    software.YearOfFounding = Int32.Parse(yearOfFounding.Text);
                    software.Price = Int32.Parse(price.Text);

                    if (UniqueCode(code.Text))
                    {
                        ComputerCentre.SoftwareRepository.Add(software);
                        ComputerCentre.SoftwareRepository.Context.SaveChanges();
                        MessageBox.Show("Successfully added software");
                        btnAdd.Content = "Add";
                        view();
                    }
                    else
                    {
                        MessageBox.Show("Software code has to be unique");
                    }
                }
            }
            else
            {
                
                if (!(code.Text).Equals(softwareCode) && !UniqueCode(code.Text))
                {
                    MessageBox.Show("Software code has to be unique");
                }
                else
                {
                    if (osType.Text.Equals("") || nameSoftware.Text.Equals("") || code.Text.Equals("") || yearOfFounding.Text.Equals("") || price.Text.Equals(""))
                    {
                        MessageBox.Show("Some obligatory fields are empty");           
                    }
                    else
                    {
                        int id = FindID(softwareCode);
                        ComputerCentre.SoftwareRepository.Get(id).Name = nameSoftware.Text;
                        ComputerCentre.SoftwareRepository.Get(id).Code = code.Text;
                        ComputerCentre.SoftwareRepository.Get(id).Description = desc.Text;
                        ComputerCentre.SoftwareRepository.Get(id).Manufacturer = manufacturer.Text;
                        ComputerCentre.SoftwareRepository.Get(id).OsType = getOsType(osType.Text);
                        ComputerCentre.SoftwareRepository.Get(id).Website = website.Text;
                        ComputerCentre.SoftwareRepository.Get(id).YearOfFounding = Int32.Parse(yearOfFounding.Text);
                        ComputerCentre.SoftwareRepository.Get(id).Price = Int32.Parse(price.Text);
                        ComputerCentre.SoftwareRepository.Context.SaveChanges();
                        MessageBox.Show("Successfully edited software");
                        btnAdd.Content = "Add";
                        view();
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
            foreach (Software s in softwaresList)
            {
                if (code.Equals(s.Code)) return false;
            }
            return true;
        }

        public int FindID(string code)
        {
            foreach (Software s in softwaresList)
            {
                if (code.Equals(s.Code))
                {
                    return s.Id;
                }
            }
            return 0;
        }
        
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                nameSoftware.Text = dataRowView["Name"].ToString();
                code.Text = dataRowView["Code"].ToString();
                desc.Text = dataRowView["Description"].ToString();
                osType.SelectedItem = dataRowView["OsType"].ToString();
                manufacturer.Text = dataRowView["Manufacturer"].ToString();
                website.Text = dataRowView["Website"].ToString();
                yearOfFounding.Text = dataRowView["YearOfFounding"].ToString();
                price.Text = dataRowView["Price"].ToString();
                btnAdd.Content = "Update";
                softwareCode = code.Text;
            }
            else
            {
                MessageBox.Show("Please select any software from the list..");
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                btnAdd.Content = "Add";
                DataRowView dataRowView = (DataRowView)gvData.SelectedItems[0];
                String code = dataRowView["code"].ToString();
                foreach (Software s in softwaresList)
                {
                    if (code.Equals(s.Code))
                    {
                        ComputerCentre.SoftwareRepository.Remove(s);
                        ComputerCentre.SoftwareRepository.Context.SaveChanges();
                        MessageBox.Show("Successfully deleted software");
                        view();
                        break;
                    }
                }
            }
            else MessageBox.Show("Please Select Any Software From the list...");
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            desc.Text = "";
            nameSoftware.Text = "";
            code.Text = "";
            osType.Text = "";
            manufacturer.Text = "";
            website.Text = "";
            yearOfFounding.Text = "";
            price.Text = "";
            btnAdd.Content = "Add";
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
   
}