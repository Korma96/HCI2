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
    public partial class AddNewOrUpdateTermDialog : Window
    {
        public MainWindow MWindow;
        /*public Grid MainGrid { get; set; }
        public Grid LeftGrid { get; set; }
        public Grid TopTopGrid { get; set; }
        public Grid TopBottomGrid { get; set; }

        public TextBlock SelectedElement { get; set; }*/

        public TimePicker StartTimePicker { get; set; }
        public TimePicker EndTimePicker { get; set; }

        public ComboBox ComboSubject { get; set; }
        public ComboBox ComboDay { get; set; }
        public ComboBox ComboClassRoom { get; set; }

        private const int NUM_OF_CLASSROOMS = 6;
        private const int NUM_OF_DAYS = 6;

        public AddNewOrUpdateTermDialog(MainWindow mainWindow, string title)
        {
            InitializeComponent();

            Title = title;

            MWindow = mainWindow;

            /*MainGrid = mainWindow.MainGrid;
            LeftGrid = mainWindow.LeftGrid;
            TopTopGrid = mainWindow.TopTopGrid;
            TopBottomGrid = mainWindow.TopBottomGrid;

            SelectedElement = mainWindow.SelectedElement;*/

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
            ComboDay.Items.Add(new ComboBoxItem() { Content = "PONEDELJAK" });
            ComboDay.Items.Add(new ComboBoxItem() { Content = "UTORAK" });
            ComboDay.Items.Add(new ComboBoxItem() { Content = "SREDA" });
            ComboDay.Items.Add(new ComboBoxItem() { Content = "ČETVRTAK" });
            ComboDay.Items.Add(new ComboBoxItem() { Content = "PETAK" });
            ComboDay.Items.Add(new ComboBoxItem() { Content = "SUBOTA" });
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

        public void inicijalizacijaDialoga()
        {
            int rowSpan = Grid.GetRowSpan(MWindow.SelectedElement);
            int rowIndex = Grid.GetRow(MWindow.SelectedElement);
            int columnIndex = Grid.GetColumn(MWindow.SelectedElement);

            string startTime = (MWindow.LeftGrid.Children[rowIndex] as TextBlock).Text.Split('-')[0].Trim();
            string endTime = (MWindow.LeftGrid.Children[rowIndex + rowSpan - 1] as TextBlock).Text.Split('-')[1].Trim();

            int classRoomSelectedIndex = columnIndex % NUM_OF_CLASSROOMS;

            double a = columnIndex / NUM_OF_CLASSROOMS;
            int daySelectedIndex = Convert.ToInt32(Math.Floor(a)); // floor zaokruzuje na najblizi manji broj

            StartTimePicker.setHoursAndMinutes(startTime);

            EndTimePicker.setHoursAndMinutes(endTime);

            (ComboDay.Items[daySelectedIndex] as ComboBoxItem).IsSelected = true;

            (ComboClassRoom.Items[classRoomSelectedIndex] as ComboBoxItem).IsSelected = true;

        }

        private void saveClick(object sender, RoutedEventArgs e)
        {
            MWindow.SelectedElement.Background = Brushes.LightGray;
            MWindow.SelectedElement = null;

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
                    MessageBox.Show("Start time must greater than end time!");
                }
                else
                {
                    int columnDay = MainWindow.GetColumnForDay(MWindow.TopTopGrid, ComboDay.Text);
                    int columnClassRoom = MainWindow.GetColumnForClassRoom(MWindow.TopBottomGrid, ComboClassRoom.Text, columnDay, NUM_OF_CLASSROOMS);


                    List<int[]> indexesForNewTerm = new List<int[]>();

                    // ovo dodavanje crtica smo uveli da bismo znali da razlikujemo da li se radi o pocetku ili kraju termina
                    int startRow = MainWindow.GetRowForTime(MWindow.LeftGrid, time1Str + " -");
                    int endRow = MainWindow.GetRowForTime(MWindow.LeftGrid, "- " + time2Str);
                    int index;
                    int rowSpan = 0;
                    TextBlock block;

                    for (int row = startRow; row <= endRow; row++)
                    {
                        index = MWindow.GetIndexOfMainGridElement(MWindow.MainGrid, row, columnClassRoom);
                        //rect = MainGrid.Children[index] as Rectangle;

                        if (index == -1/*rect == null*/)
                        {
                            //MessageBox.Show("Zauzet neki deo termina!");
                            //return;
                            continue;
                        }
                        else
                        {
                            block = MWindow.MainGrid.Children[index] as TextBlock;
                            if (!block.Text.Equals(""))
                            {
                                MessageBox.Show("Zauzet neki deo termina!");
                                return;
                            }
                            else
                            {
                                indexesForNewTerm.Add(new int[3] { index, row, columnClassRoom });
                                rowSpan = Grid.GetRowSpan(block);
                                if (rowSpan > 1)
                                {
                                    row += rowSpan - 1;
                                }
                                //if (row + rowSpan - 1 >= endRow) break;
                            }
                        }
                    }

                    int indexOfFirst = indexesForNewTerm[0][0];
                    int rowOfFirst = indexesForNewTerm[0][1];
                    int colOfFirst = indexesForNewTerm[0][2];

                    /*UIElement oldElement = MainGrid.Children[indexOfFirst];
                    int oldRow = Grid.GetRow(oldElement);
                    int oldColumn = Grid.GetColumn(oldElement);*/

                    obrisiRectanglove(indexesForNewTerm);

                    TextBlock newTextBlock = MWindow.MainGrid.Children[indexOfFirst] as TextBlock;   //MWindow.createNewTextBlock(ComboSubject.Text);
                    newTextBlock.Text = ComboSubject.Text;
                    newTextBlock.Background = Brushes.Aqua;
                    newTextBlock.Foreground = Brushes.Red;
                    Grid.SetRowSpan(newTextBlock, endRow - startRow + 1/*indexesForNewTerm.Count*/);
                    //Grid.SetRow(newTextBlock, rowOfFirst);
                    //Grid.SetColumn(newTextBlock, colOfFirst);
                    //MainGrid.Children.Insert(indexOfFirst, newTextBlock);
                    //MWindow.addNewIndex(indexOfFirst, rowOfFirst, colOfFirst);

                    this.Close();
                }

            }
            else MessageBox.Show("Problem with time!");
        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void obrisiRectanglove(List<int[]> indexesForNewTerm)
        {
            int index;

            for (int i = 1; i < indexesForNewTerm.Count; i++)
            {
                //MainGrid.Children.RemoveAt(indexesForNewTerm[i] - i);
                index = indexesForNewTerm[i][0];
                //row = indexesForNewTerm[i][1];
                //col = indexesForNewTerm[i][2];

                //(MWindow.MainGrid.Children[index] as TextBlock).Text = "";
                MWindow.MainGrid.Children.RemoveAt(index - i + 1);

                //MWindow.removeIndex(row, col);
            }

        }
    }
}
