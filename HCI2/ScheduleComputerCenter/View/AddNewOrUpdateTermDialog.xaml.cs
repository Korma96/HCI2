using ScheduleComputerCenter.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for AddNewTermDialog.xaml
    /// </summary>
    public partial class AddNewOrUpdateTermDialog : Window
    {
        public MainWindow MWindow;

        public TimePicker StartTimePicker { get; set; }
        public TimePicker EndTimePicker { get; set; }

        public ComboBox ComboSubject { get; set; }
        public ComboBox ComboDay { get; set; }
        public ComboBox ComboClassRoom { get; set; }

        public int NumOfClassRooms { get; set; }

        private const int NUM_OF_DAYS = 6;

        private TimeSpan sevenHours = TimeSpan.Parse("07:00");
        private TimeSpan twentyTwoHours = TimeSpan.Parse("22:00");

        public List<Subject> ListSubjects { get; set; }

        public AddNewOrUpdateTermDialog(MainWindow mainWindow, string title)
        {
            InitializeComponent();

            Title = title;

            MWindow = mainWindow;

            this.Closed += new EventHandler(Dialog_Closed);

            /*MainGrid = mainWindow.MainGrid;
            LeftGrid = mainWindow.LeftGrid;
            TopTopGrid = mainWindow.TopTopGrid;
            TopBottomGrid = mainWindow.TopBottomGrid;

            SelectedElement = mainWindow.SelectedElement;*/

            TextBlock subjectTextBlock = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Text = "Subject:" };
            Grid.SetRow(subjectTextBlock, 1);
            Grid.SetColumn(subjectTextBlock, 0);
            DialogGrid.Children.Add(subjectTextBlock);

            ComboSubject = new ComboBox() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, IsEditable = false, Height = 30 };
            ListSubjects = ComputerCentre.SubjectRepository.GetAll().ToList();
            string subjectName;
            foreach (Subject subject in ListSubjects)
            {
                subjectName = subject.Name;
                if (subject.Course != null) subjectName += " " + subject.Course.Name;
                ComboSubject.Items.Add(new ComboBoxItem() { Content = subjectName });
            }

            Grid.SetRow(ComboSubject, 1);
            Grid.SetColumn(ComboSubject, 1);
            DialogGrid.Children.Add(ComboSubject);

            TextBlock start = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Text = "Start:" };
            Grid.SetRow(start, 2);
            Grid.SetColumn(start, 0);
            DialogGrid.Children.Add(start);

            StartTimePicker = new TimePicker();
            Grid.SetRow(StartTimePicker, 2);
            Grid.SetColumn(StartTimePicker, 1);
            DialogGrid.Children.Add(StartTimePicker);

            TextBlock end = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Text = "End:" };
            Grid.SetRow(end, 3);
            Grid.SetColumn(end, 0);
            DialogGrid.Children.Add(end);

            EndTimePicker = new TimePicker();
            Grid.SetRow(EndTimePicker, 3);
            Grid.SetColumn(EndTimePicker, 1);
            DialogGrid.Children.Add(EndTimePicker);

            TextBlock day = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Text = "Day:" };
            Grid.SetRow(day, 4);
            Grid.SetColumn(day, 0);
            DialogGrid.Children.Add(day);

            ComboDay = new ComboBox() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, IsEditable = false, Height = 30 };
            foreach(string dayStr in MWindow.days) ComboDay.Items.Add(new ComboBoxItem() { Content = dayStr });
            Grid.SetRow(ComboDay, 4);
            Grid.SetColumn(ComboDay, 1);
            DialogGrid.Children.Add(ComboDay);

            TextBlock classRoomTxtBlock = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Text = "Class room:" };
            Grid.SetRow(classRoomTxtBlock, 5);
            Grid.SetColumn(classRoomTxtBlock, 0);
            DialogGrid.Children.Add(classRoomTxtBlock);

            ComboClassRoom = new ComboBox() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, IsEditable = false, Height = 30 };
            NumOfClassRooms = MWindow.Classrooms.Count;
            foreach(Classroom classroom in MWindow.Classrooms) ComboClassRoom.Items.Add(new ComboBoxItem() { Content = classroom.Name });
            Grid.SetRow(ComboClassRoom, 5);
            Grid.SetColumn(ComboClassRoom, 1);
            DialogGrid.Children.Add(ComboClassRoom);

            Button btnSave = new Button() { Width = 60, Height = 20, HorizontalAlignment = HorizontalAlignment.Center, Content = "Save" };
            btnSave.Click += saveClick;
            Grid.SetRow(btnSave, 7);
            Grid.SetColumn(btnSave, 0);
            DialogGrid.Children.Add(btnSave);

            Button btnCancel = new Button() { Width = 60, Height = 20, HorizontalAlignment = HorizontalAlignment.Center, Content = "Cancel" };
            btnCancel.Click += cancelClick;
            Grid.SetRow(btnCancel, 7);
            Grid.SetColumn(btnCancel, 1);
            DialogGrid.Children.Add(btnCancel);
        }

        public void inicijalizacijaDialoga(string subjectName)
        {
            int rowIndex = MWindow.SelectedElement.SelectedIndex;
            int columnIndex = Grid.GetColumn(MWindow.SelectedElement);
            int rowSpan = MWindow.ObservableList[columnIndex][rowIndex].RowSpan;

            string startTime = (MWindow.LeftGrid.Children[rowIndex] as TextBlock).Text.Split('-')[0].Trim();
            string endTime = (MWindow.LeftGrid.Children[rowIndex + rowSpan - 1] as TextBlock).Text.Split('-')[1].Trim();

            int classRoomSelectedIndex = columnIndex % NumOfClassRooms;

            double a = columnIndex / NumOfClassRooms;
            int daySelectedIndex = Convert.ToInt32(Math.Floor(a)); // floor zaokruzuje na najblizi manji broj

            selectSubject(subjectName);

            StartTimePicker.setHoursAndMinutes(startTime);

            EndTimePicker.setHoursAndMinutes(endTime);

            (ComboDay.Items[daySelectedIndex] as ComboBoxItem).IsSelected = true;

            selectClassRoom(classRoomSelectedIndex);

        }

        private void selectSubject(string subjectName)
        {
            if(subjectName != null)
            {
                foreach (ComboBoxItem cbi in ComboSubject.Items)
                {
                    if (cbi.Content.Equals(subjectName))
                    {
                        cbi.IsSelected = true;
                        return;
                    }
                }
            }
            else
            {
                (ComboSubject.Items[0] as ComboBoxItem).IsSelected = true;
            }
        }

        private void selectClassRoom(int classRoomSelectedIndex)
        {
            string classroomName =  (MWindow.TopBottomGrid.Children[classRoomSelectedIndex] as TextBlock).Text;
            foreach (ComboBoxItem cbi in ComboClassRoom.Items)
            {
                if (cbi.Content.Equals(classroomName))
                {
                    cbi.IsSelected = true;
                    return;
                }
            }
        }

        private void saveClick(object sender, RoutedEventArgs e)
        {
            //MWindow.SelectedElement.SelectedItems.Clear();
            //MWindow.SelectedElement = null;

            string time1Str = StartTimePicker.getTime();
            string time2Str = EndTimePicker.getTime();

            TimeSpan time1;
            bool time1Bool = TimeSpan.TryParse(time1Str, out time1);
            TimeSpan time2;
            bool time2Bool = TimeSpan.TryParse(time2Str, out time2);

            if (time1Bool && time2Bool)
            {
                if (time1 >= time2)
                {
                    MessageBox.Show("End time must greater than start time!");
                }
                else if(time1 < sevenHours)
                {
                    MessageBox.Show("Start time must equal or greater than 7 o'clock!");
                }
                else if(time2 > twentyTwoHours)
                {
                    MessageBox.Show("End time must equal or less than 22 o'clock!");
                }
                else
                {
                    int columnDay = MainWindow.GetColumnForDay(MWindow.TopTopGrid, ComboDay.Text);
                    int columnClassRoom = MainWindow.GetColumnForClassRoom(MWindow.TopBottomGrid, ComboClassRoom.Text, columnDay, NumOfClassRooms);

                    // ovo dodavanje crtica smo uveli da bismo znali da razlikujemo da li se radi o pocetku ili kraju termina
                    int startRow = MainWindow.GetRowForTime(MWindow.LeftGrid, time1Str + " -");
                    int endRow = MainWindow.GetRowForTime(MWindow.LeftGrid, "- " + time2Str);
                    int rowSpan = endRow - startRow + 1;
                    int preracunatStartRow = perarcunajStartRowNaListviewStartRow(startRow, columnClassRoom);
                    int preracunatEndRow = preracunatStartRow + rowSpan - 1;

                    List<int> columnAndRowsForRemove = new List<int>();
                    columnAndRowsForRemove.Add(columnClassRoom);


                    for (int row = preracunatStartRow; row <= preracunatEndRow; row++)
                    {
                        if (MWindow.ObservableList[columnClassRoom][row].Subject != null)
                        {
                            MessageBox.Show("Zauzet neki deo termina!");
                            return;
                        }
                        else
                        {
                            columnAndRowsForRemove.Add(row);
                        }
                    }

                    Subject subject = ListSubjects[ComboSubject.SelectedIndex];
                    Term oldTerm = MWindow.ObservableList[columnClassRoom][preracunatStartRow];
                    oldTerm.StartTime = time1;
                    oldTerm.EndTime = time2;
                    oldTerm.Subject = subject;
                    oldTerm.RowSpan = rowSpan;
                    MWindow.ObservableList[columnClassRoom].Insert(preracunatStartRow, oldTerm);
                    obrisiPrazneTermine(columnAndRowsForRemove);

                    ComputerCentre.context.Entry(oldTerm.Day).State = EntityState.Modified;

                    ComputerCentre.context.SaveChanges();

                    this.Close();
                }

            }
            else MessageBox.Show("Problem with time!");
        }

        private int perarcunajStartRowNaListviewStartRow(int startRow, int columnClassRoom)
        {
            int preracunatStartRow = -1;
            int i = 0;

            while(i <= startRow)
            {
                i += MWindow.ObservableList[columnClassRoom][i].RowSpan;
                preracunatStartRow++;
            }

            return preracunatStartRow;
        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void obrisiPrazneTermine(List<int> columnAndRowsForRemove)
        {
            int column = columnAndRowsForRemove[0];
            int indexOfTerm;

            for (int i = 1; i < columnAndRowsForRemove.Count; i++)
            {
                indexOfTerm = columnAndRowsForRemove[i] + 1; // plus 1 sam dodao jer smo prethodno dodali novi element
                                                             // i sve se transiralo za 1
                MWindow.ObservableList[column].RemoveAt(indexOfTerm);
            }

        }

        void Dialog_Closed(object sender, EventArgs e)
        {
            MWindow.SelectedElement.SelectedItems.Clear();
            MWindow.SelectedElement = null;
        }
    }
}
