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

        public Term OldTermForUpdate { get; set; }
        public int OldTermForUpdateRow { get; set; }
        public int OldTermForUpdateColumn { get; set; }

        public AddNewOrUpdateTermDialog(MainWindow mainWindow, string title, Term oldTermForUpdate, int oldTermForUpdateRow, int oldTermForUpdateColumn)
        {
            InitializeComponent();

            Title = title;

            MWindow = mainWindow;

            this.Owner = MWindow;

            this.OldTermForUpdate = oldTermForUpdate;
            this.OldTermForUpdateRow = oldTermForUpdateRow;
            this.OldTermForUpdateColumn = oldTermForUpdateColumn;

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
                if (subject.Course != null) subjectName += " (" + subject.Course.Code + ")";
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
            if (rowIndex == -1) return;

            int preracunatRowIndex = perarcunajListviewStartRowNaLeftGridStartRow(rowIndex, columnIndex);

            string startTime = (MWindow.LeftGrid.Children[preracunatRowIndex] as TextBlock).Text.Split('-')[0].Trim();
            
            int rowSpan;

            if (this.OldTermForUpdate == null) rowSpan = MWindow.ObservableList[columnIndex][rowIndex].RowSpan;
            else rowSpan = this.OldTermForUpdate.RowSpan;

            string endTime = (MWindow.LeftGrid.Children[preracunatRowIndex + rowSpan - 1] as TextBlock).Text.Split('-')[1].Trim();

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

            Classroom classroom = MWindow.Classrooms[ComboClassRoom.SelectedIndex];
            Subject subject = ListSubjects[ComboSubject.SelectedIndex];
            if (!MWindow.ClassroomMatchSubjectNeeds(classroom, subject))
            {
                return;
            }
            

            string time1Str = StartTimePicker.getTime();
            string time2Str = EndTimePicker.getTime();

            TimeSpan time1;
            bool time1Bool = TimeSpan.TryParse(time1Str, out time1);
            TimeSpan time2;
            bool time2Bool = TimeSpan.TryParse(time2Str, out time2);

            if (!checkTimes(time1Bool, time2Bool, time1, time2))
            {
                return;
            }

            int columnDay = MainWindow.GetColumnForDay(MWindow.TopTopGrid, ComboDay.Text);
            int columnClassRoom = MainWindow.GetColumnForClassRoom(MWindow.TopBottomGrid, ComboClassRoom.Text, columnDay, NumOfClassRooms);

            // ovo dodavanje crtica smo uveli da bismo znali da razlikujemo da li se radi o pocetku ili kraju termina
            int startRow = MainWindow.GetRowForTime(MWindow.LeftGrid, time1Str + " -");
            int endRow = MainWindow.GetRowForTime(MWindow.LeftGrid, "- " + time2Str);
            int rowSpan = endRow - startRow + 1;
            int preracunatStartRow = perarcunajStartRowNaListviewStartRow(startRow, columnClassRoom);
            int preracunatEndRow = preracunatStartRow + rowSpan - 1;

            if (daLiJeZauzetNekiDeoTermina(preracunatStartRow, preracunatEndRow, columnClassRoom))
            {
                return;
            }

            if(this.OldTermForUpdate != null)
            {
                MWindow.obrisiTerminiUbaciPrazneTermine(this.OldTermForUpdate, this.OldTermForUpdateRow, this.OldTermForUpdateColumn);
                ComputerCentre.context.SaveChanges();

                preracunatStartRow = perarcunajStartRowNaListviewStartRow(startRow, columnClassRoom);
            }

            Term oldTerm = MWindow.ObservableList[columnClassRoom][preracunatStartRow];
            Term newTerm = new Term(time1, time2, subject, oldTerm.Day, oldTerm.ClassroomIndex, rowSpan); 
                   
            obrisiPrazneTermine(columnClassRoom, preracunatStartRow, rowSpan);

            newTerm = ComputerCentre.context.Terms.Add(newTerm);
            ComputerCentre.context.SaveChanges();

            MWindow.ObservableList[columnClassRoom].Insert(preracunatStartRow, newTerm);

            this.Close();
        }

        private bool checkTimes(bool time1Bool, bool time2Bool, TimeSpan time1, TimeSpan time2)
        {
            if (time1Bool && time2Bool)
            {
                if (time1 >= time2)
                {
                    MessageBox.Show("End time must greater than start time!");
                    return false;
                }
                else if (time1 < sevenHours)
                {
                    MessageBox.Show("Start time must equal or greater than 7 o'clock!");
                    return false;
                }
                else if (time2 > twentyTwoHours)
                {
                    MessageBox.Show("End time must equal or less than 22 o'clock!");
                    return false;
                }

                return true;
            }

            MessageBox.Show("Problem with time!");
            return false;
        }

        private bool daLiJeZauzetNekiDeoTermina(int preracunatStartRow, int preracunatEndRow, int columnClassRoom)
        {
            bool zauzeto;

            for (int row = preracunatStartRow; row <= preracunatEndRow; row++)
            {
                if (MWindow.ObservableList[columnClassRoom][row].Subject != null)
                {
                    zauzeto = true;
                    if(OldTermForUpdate != null)
                    {
                        if (row == preracunatStartRow || row == preracunatEndRow) // dovoljno je proveravati samo za pocetno i krajnje polje
                        {
                            zauzeto = !daLiPomereniTerminZauzimaStaraPolja(OldTermForUpdateRow, OldTermForUpdateColumn, OldTermForUpdate.RowSpan, row, columnClassRoom);
                        }
                    }

                    if (zauzeto)
                    {
                        MessageBox.Show("Zauzet neki deo termina!");
                        return true;
                    }

                }

            }

            return false;
        }

        private bool daLiPomereniTerminZauzimaStaraPolja(int oldTermForUpdateRow, int oldTermForUpdateColumn, int oldTermForUpdateRowSpan, int row, int columnClassRoom)
        {
            if (OldTermForUpdateColumn != columnClassRoom) return false;

            int kraj = oldTermForUpdateRow + oldTermForUpdateRowSpan;

            for (int i = OldTermForUpdateRow; i < kraj; i++)
            {
                if (i == row) return true;
            }

            return false;
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

        private int perarcunajListviewStartRowNaLeftGridStartRow(int startRow, int columnClassRoom)
        {
            int preracunatStartRow = 0;

            for (int i = 0;  i < startRow; i++)
            {
                preracunatStartRow += MWindow.ObservableList[columnClassRoom][i].RowSpan;
            }

            return preracunatStartRow;
        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void obrisiPrazneTermine(int column, int row, int rowSpan)
        {
            Term term;

            for (int i = 0; i < rowSpan; i++)
            {
                /* indexOfTerm = columnAndRowsForRemove[i] + 1; // plus 1 sam dodao jer smo prethodno dodali novi element
                                                              // i sve se transiralo za 1*/
                term = MWindow.ObservableList[column][row];
                ComputerCentre.TermRepository.Remove(term);
                MWindow.ObservableList[column].RemoveAt(row);
                //day.Terms.RemoveAt(row);
            }

        }

        void Dialog_Closed(object sender, EventArgs e)
        {
            MWindow.SelectedElement.SelectedItems.Clear();
            MWindow.SelectedElement = null;
        }
    }
}
