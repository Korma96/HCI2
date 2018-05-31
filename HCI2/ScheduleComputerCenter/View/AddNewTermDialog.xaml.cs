using System;
using System.Collections.Generic;
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
    public partial class AddNewTermDialog : Window
    {
        public MainWindow MWindow;
        public Grid MainGrid { get; set; }
        public Grid LeftGrid { get; set; }
        public Grid TopTopGrid { get; set; }
        public Grid TopBottomGrid { get; set; }

        public object SelectedElement { get; set; }

        public TimePicker StartTimePicker { get; set; }
        public TimePicker EndTimePicker { get; set; }

        public ComboBox ComboSubject { get; set; }
        public ComboBox ComboDay { get; set; }
        public ComboBox ComboClassRoom { get; set; }

        private const int NUM_OF_CLASSROOMS = 6;
        private const int NUM_OF_DAYS = 6;

        public AddNewTermDialog(MainWindow mainWindow)
        {
            InitializeComponent();

            MWindow = mainWindow;

            MainGrid = mainWindow.MainGrid;
            LeftGrid = mainWindow.LeftGrid;
            TopTopGrid = mainWindow.TopTopGrid;
            TopBottomGrid = mainWindow.TopBottomGrid;

            SelectedElement = mainWindow.SelectedElement;

            UIElement element = SelectedElement as UIElement;
            int rowSpan = Grid.GetRowSpan(element);
            int rowIndex = Grid.GetRow(element);
            int columnIndex = Grid.GetColumn(element);

            //MessageBox.Show("Row: " + rowIndex + ", Column: " + columnIndex);

            string startTime = (LeftGrid.Children[rowIndex] as TextBlock).Text.Split('-')[0].Trim();
            string endTime = (LeftGrid.Children[rowIndex + rowSpan - 1] as TextBlock).Text.Split('-')[1].Trim();

            int classRoomSelectedIndex = columnIndex % NUM_OF_CLASSROOMS;

            double a = columnIndex / NUM_OF_CLASSROOMS;
            int daySelectedIndex = Convert.ToInt32(Math.Floor(a));

            TextBlock subject = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Text = "Subject:" };
            Grid.SetRow(subject, 1);
            Grid.SetColumn(subject, 0);
            DialogGrid.Children.Add(subject);
            ComboSubject = new ComboBox() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, IsEditable = false, Height = 30 };
            ComboSubject.Items.Add(new ComboBoxItem() { IsSelected = true, Content = "OOP" });
            ComboSubject.Items.Add(new ComboBoxItem() { Content = "USI" });
            ComboSubject.Items.Add(new ComboBoxItem() { Content = "ORI" });
            ComboSubject.Items.Add(new ComboBoxItem() { Content = "PIGKUT" });
            ComboSubject.Items.Add(new ComboBoxItem() { Content = "ISA" });
            Grid.SetRow(ComboSubject, 1);
            Grid.SetColumn(ComboSubject, 1);
            DialogGrid.Children.Add(ComboSubject);

            TextBlock start = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Text = "Start:" };
            Grid.SetRow(start, 2);
            Grid.SetColumn(start, 0);
            DialogGrid.Children.Add(start);
            StartTimePicker = new TimePicker(startTime);
            Grid.SetRow(StartTimePicker, 2);
            Grid.SetColumn(StartTimePicker, 1);
            DialogGrid.Children.Add(StartTimePicker);

            TextBlock end = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Text = "End:" };
            Grid.SetRow(end, 3);
            Grid.SetColumn(end, 0);
            DialogGrid.Children.Add(end);
            EndTimePicker = new TimePicker(endTime);
            Grid.SetRow(EndTimePicker, 3);
            Grid.SetColumn(EndTimePicker, 1);
            DialogGrid.Children.Add(EndTimePicker);

            TextBlock day = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Text = "Day:" };
            Grid.SetRow(day, 4);
            Grid.SetColumn(day, 0);
            DialogGrid.Children.Add(day);
            ComboDay = new ComboBox() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, IsEditable = false, Height = 30 };
            ComboDay.Items.Add(new ComboBoxItem() { Content = "PONEDELJAK" });
            ComboDay.Items.Add(new ComboBoxItem() { Content = "UTORAK" });
            ComboDay.Items.Add(new ComboBoxItem() { Content = "SREDA" });
            ComboDay.Items.Add(new ComboBoxItem() { Content = "ČETVRTAK" });
            ComboDay.Items.Add(new ComboBoxItem() { Content = "PETAK" });
            ComboDay.Items.Add(new ComboBoxItem() { Content = "SUBOTA" });
            (ComboDay.Items[daySelectedIndex] as ComboBoxItem).IsSelected = true;
            Grid.SetRow(ComboDay, 4);
            Grid.SetColumn(ComboDay, 1);
            DialogGrid.Children.Add(ComboDay);

            TextBlock classRoom = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Text = "Class room:" };
            Grid.SetRow(classRoom, 5);
            Grid.SetColumn(classRoom, 0);
            DialogGrid.Children.Add(classRoom);
            ComboClassRoom = new ComboBox() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, IsEditable = false, Height = 30 };
            ComboClassRoom.Items.Add(new ComboBoxItem() { Content = "L1" });
            ComboClassRoom.Items.Add(new ComboBoxItem() { Content = "L2" });
            ComboClassRoom.Items.Add(new ComboBoxItem() { Content = "L3" });
            ComboClassRoom.Items.Add(new ComboBoxItem() { Content = "L4" });
            ComboClassRoom.Items.Add(new ComboBoxItem() { Content = "L5" });
            ComboClassRoom.Items.Add(new ComboBoxItem() { Content = "L6" });
            (ComboClassRoom.Items[classRoomSelectedIndex] as ComboBoxItem).IsSelected = true;
            Grid.SetRow(ComboClassRoom, 5);
            Grid.SetColumn(ComboClassRoom, 1);
            DialogGrid.Children.Add(ComboClassRoom);

            Button btnSave = new Button() { Width = 60, Height = 20, HorizontalAlignment = HorizontalAlignment.Center, Content = "Save" };
            btnSave.Click += saveClick;
            Grid.SetRow(btnSave, 7);
            Grid.SetColumn(btnSave, 0);
            DialogGrid.Children.Add(btnSave);

            Button btnCancel = new Button() { Width = 60, Height = 20, HorizontalAlignment = HorizontalAlignment.Center, Content="Cancel" };
            btnCancel.Click += cancelClick;
            Grid.SetRow(btnCancel, 7);
            Grid.SetColumn(btnCancel, 1);
            DialogGrid.Children.Add(btnCancel);
        }

        private void saveClick(object sender, RoutedEventArgs e)
        {
            string time1Str = StartTimePicker.getTime();
            string time2Str = EndTimePicker.getTime();

            TimeSpan time1;
            bool time1Bool = TimeSpan.TryParse(time1Str, out time1);
            TimeSpan time2;
            bool time2Bool = TimeSpan.TryParse(time2Str, out time2);

            if (time1Bool && time2Bool)
            {
                if(time2.Subtract(time1).Minutes <= 0)
                {
                    MessageBox.Show("Start time must greater than end time!");
                }
                else
                {
                    int columnDay = MainWindow.GetColumnForDay(TopTopGrid, ComboDay.Text);
                    int columnClassRoom = MainWindow.GetColumnForClassRoom(TopBottomGrid, ComboClassRoom.Text, columnDay, NUM_OF_CLASSROOMS);


                    List<int> indexesForNewTerm = new List<int>();

                    // ovo dodavanje crtica smo uveli da bismo znali da razlikujemo da li se radi o pocetku ili kraju termina
                    int startRow = MainWindow.GetRowForTime(LeftGrid, time1Str + " -");
                    int endRow = MainWindow.GetRowForTime(LeftGrid, "- " + time2Str);
                    int index;
                    int rowSpan = 0;
                    Rectangle rect;

                    for(int row = startRow; row <= endRow; row++)
                    {
                        index = MainWindow.GetIndexOfMainGridElement(MainGrid, row, columnClassRoom, NUM_OF_CLASSROOMS);
                        //rect = MainGrid.Children[index] as Rectangle;

                        if ( index == -1/*rect == null*/)
                        {
                            //MessageBox.Show("Zauzet neki deo termina!");
                            //return;
                            continue;
                        }
                        else
                        {
                            rect = MainGrid.Children[index] as Rectangle;
                            if(rect == null)
                            {
                                MessageBox.Show("Zauzet neki deo termina!");
                                return;
                            }
                            else
                            {
                                indexesForNewTerm.Add(index);
                                rowSpan = Grid.GetRowSpan(rect);
                                if(rowSpan > 1)
                                {
                                    row += rowSpan - 1;
                                }
                                //if (row + rowSpan - 1 >= endRow) break;
                            }
                        }
                    }

                    UIElement oldElement = MainGrid.Children[indexesForNewTerm[0]];
                    int oldRow = Grid.GetRow(oldElement);
                    int oldColumn = Grid.GetColumn(oldElement);

                    obrisiRectanglove(indexesForNewTerm);

                    TextBlock newTextBlock = MWindow.createNewTextBlock(ComboSubject.Text);
                    Grid.SetRowSpan(newTextBlock, endRow - startRow + 1/*indexesForNewTerm.Count*/);
                    Grid.SetRow(newTextBlock, oldRow);
                    Grid.SetColumn(newTextBlock, oldColumn);
                    //MainGrid.Children.Add(newTextBlock);
                    MainGrid.Children.Insert(indexesForNewTerm[0], newTextBlock);

                    this.Close();
                }
                
            }
            else MessageBox.Show("Problem with time!");
        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void obrisiRectanglove(List<int> indexesForNewTerm)
        {
           foreach(int index in indexesForNewTerm)
            {
                MainGrid.Children.RemoveAt(index);
            }
            
        }
    }
}
