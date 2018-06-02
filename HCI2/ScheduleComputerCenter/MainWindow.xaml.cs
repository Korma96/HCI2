using ScheduleComputerCenter.View;
using ScheduleComputerCenter.Commands;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using ScheduleComputerCenter.Model;

namespace ScheduleComputerCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point startPoint = new Point();
        public ObservableCollection<Subject> Subjects { get; set;}

        private Dictionary<string, int> indeksi;

        private const int NUM_OF_DAYS = 6;
        private const int NUM_OF_CLASSROOMS = 6;

        private const int NUM_OF_ROWS_IN_MAIN_GRID = 60;
        private const int NUM_OF_COLS_IN_MAIN_GRID = 36;

        private const int HEIGHT_OF_ROWS_IN_MAIN_GRID = 25;
        private const int WIDTH_OF_COLS_IN_MAIN_GRID = 32;

        private string[] days = { "PONEDELJAK", "UTORAK", "SREDA", "ČETVRTAK", "PETAK", "SUBOTA" };
        private string[] classrooms = { "L1", "L2", "L3", "L4", "L5", "L6" };
        private string[] times = { "07:00", "07:15", "07:30", "07:45", "08:00", "08:15", "08:30", "08:45", "09:00", "09:15", "09:30", "09:45", "10:00", "10:15", "10:30", "10:45", "11:00", "11:15", "11:30", "11:45", "12:00", "12:15", "12:30", "12:45", "13:00", "13:15", "13:30", "13:45", "14:00", "14:15", "14:30", "14:45", "15:00", "15:15", "15:30", "15:45", "16:00", "16:15", "16:30", "16:45", "17:00", "17:15", "17:30", "17:45", "18:00", "18:15", "18:30", "18:45", "19:00", "19:15", "19:30", "19:45", "20:00", "20:15", "20:30", "20:45", "21:00", "21:15", "21:30", "21:45", "22:00" };

        public TextBlock SelectedElement { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            //ComputerCentre.AddDummyData();

            WindowState = WindowState.Maximized;

            Subjects = new ObservableCollection<Subject>();

            indeksi = new Dictionary<string, int>();

            CommandBinding AddNewTermCommandBinding = new CommandBinding(RoutedCommands.AddNewTermCommand, AddNewTermCommand_Executed, AddNewTermCommand_CanExecute);
            CommandBinding UpdateTermCommandBinding = new CommandBinding(RoutedCommands.UpdateTermCommand, UpdateTermCommand_Executed, UpdateTermCommand_CanExecute);
            this.CommandBindings.Add(AddNewTermCommandBinding);
            this.CommandBindings.Add(UpdateTermCommandBinding);

            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItemNew = new MenuItem() { Header = "Add New Term...", Command = RoutedCommands.AddNewTermCommand };
            menuItemNew.Icon = new Image() { Source = new BitmapImage(new Uri("/ScheduleComputerCenter;component/Images/add_new.png", UriKind.RelativeOrAbsolute)) };
            contextMenu.Items.Add(menuItemNew);
            MenuItem menuItemUpdate = new MenuItem() { Header = "Update Term...", Command = RoutedCommands.UpdateTermCommand };
            menuItemUpdate.Icon = new Image() { Source = new BitmapImage(new Uri("/ScheduleComputerCenter;component/Images/update.png", UriKind.RelativeOrAbsolute)) };
            contextMenu.Items.Add(menuItemUpdate);
            MainGrid.ContextMenu = contextMenu;

            definisiRedove(TopTopGrid, 1, 40);
            definisiKolone(TopTopGrid, NUM_OF_DAYS * NUM_OF_CLASSROOMS, WIDTH_OF_COLS_IN_MAIN_GRID);
            dodajTextBlockDays(NUM_OF_DAYS);

            definisiRedove(TopBottomGrid, 1, 40);
            definisiKolone(TopBottomGrid, NUM_OF_DAYS * NUM_OF_CLASSROOMS, WIDTH_OF_COLS_IN_MAIN_GRID);
            dodajTextBlockClassrooms(NUM_OF_CLASSROOMS);

            definisiRedove(LeftGrid, NUM_OF_ROWS_IN_MAIN_GRID, HEIGHT_OF_ROWS_IN_MAIN_GRID);
            definisiKolone(LeftGrid, 1, 100);
            dodajTextBlockTimes(LeftGrid, NUM_OF_ROWS_IN_MAIN_GRID);

            definisiRedove(RightGrid, NUM_OF_ROWS_IN_MAIN_GRID, HEIGHT_OF_ROWS_IN_MAIN_GRID);
            definisiKolone(RightGrid, 1, 100);
            dodajTextBlockTimes(RightGrid, NUM_OF_ROWS_IN_MAIN_GRID);

            definisiRedove(MainGrid, NUM_OF_ROWS_IN_MAIN_GRID, HEIGHT_OF_ROWS_IN_MAIN_GRID);
            definisiKolone(MainGrid, NUM_OF_COLS_IN_MAIN_GRID, WIDTH_OF_COLS_IN_MAIN_GRID);
            dodajTextBlockove(NUM_OF_ROWS_IN_MAIN_GRID, NUM_OF_COLS_IN_MAIN_GRID);

            this.Focus(); // ovo smo stavili jer je na pocetku new command-a bila uvek disable-ovana
        }

        private void dodajTextBlockTimes(Grid grid, int rows)
        {
            TextBlock newTextBlock;

            for (int i = 0; i < rows; i++)
            {
                newTextBlock = new TextBlock() { Margin = new Thickness(2), Text = times[i] + " - " + times[i + 1], Background = Brushes.Gray };
                Grid.SetRow(newTextBlock, i);
                Grid.SetColumn(newTextBlock, 0);
                grid.Children.Add(newTextBlock);
            }
        }

        private void dodajTextBlockDays(int rows)
        {
            TextBlock newTextBlock;

            for (int i = 0; i < rows; i++)
            {
                newTextBlock = new TextBlock() { Margin = new Thickness(2), Text = days[i], Background = Brushes.Gray };
                Grid.SetColumnSpan(newTextBlock, NUM_OF_CLASSROOMS);
                Grid.SetRow(newTextBlock, 0);
                Grid.SetColumn(newTextBlock, i*NUM_OF_CLASSROOMS);
                TopTopGrid.Children.Add(newTextBlock);
            }
        }

        private void dodajTextBlockClassrooms(int numOfClassrooms)
        {
            TextBlock newTextBlock;
            for(int i = 0; i < NUM_OF_DAYS; i++)
            {
                for (int j = 0; j < numOfClassrooms; j++)
                {
                    newTextBlock = new TextBlock() { Margin = new Thickness(2), Text = classrooms[j], Background = Brushes.Gray };
                    Grid.SetRow(newTextBlock, 0);
                    Grid.SetColumn(newTextBlock, i*numOfClassrooms + j);
                    TopBottomGrid.Children.Add(newTextBlock);
                }
            }
        }

        private void dodajTextBlockove(int rows, int cols)
        {
            TextBlock newTextBlock;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    newTextBlock = new TextBlock() { Margin = new Thickness(2), Text = "", LayoutTransform = new RotateTransform(270), Background = Brushes.LightGray/*, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center*/ };
                    newTextBlock.MouseLeftButtonUp += Cell_MouseLeftButtonUp;
                    newTextBlock.MouseRightButtonUp += Cell_MouseRightButtonUp;
                    Grid.SetRow(newTextBlock, i);
                    Grid.SetColumn(newTextBlock, j);
                    MainGrid.Children.Add(newTextBlock);
                    //addNewIndex(i * rows + j, i, j);
                }
            }
        }

        public void addNewIndex(int index, int row, int col)
        {
            indeksi.Add(row + "_" + col, index);
        }

        public void removeIndex(int row, int col)
        {
            indeksi.Remove(row + "_" + col);
        }

        public static void definisiRedove(Grid grid, int rows, int height)
        {
            for(int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(height) });
            }
        }

        public static void definisiKolone(Grid grid, int cols, int width)
        {
            for (int i = 0; i < cols; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(width) });
            }
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (null != e.Data && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var data = e.Data.GetData(DataFormats.Xaml) as string[];
                // handle the files here!
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Xaml))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Cell_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock oldSelectedElement = SelectedElement;
            SelectedElement = sender as TextBlock;

            if (oldSelectedElement != null) // prethodno je bio neki element selektovan
            {
                if (!oldSelectedElement.Equals(SelectedElement))
                {
                    oldSelectedElement.Background = Brushes.LightGray;
                }
                
            }

            SelectedElement.Background = Brushes.Blue;
        }

        private void Cell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock oldSelectedElement = SelectedElement;

            if (oldSelectedElement != null) // prethodno je bio neki element selektovan
            {
                oldSelectedElement.Background = Brushes.LightGray;
            }

            SelectedElement = sender as TextBlock;

            if (oldSelectedElement != null)
            {
                if (oldSelectedElement.Equals(SelectedElement))
                {
                    SelectedElement = null;
                    return;
                }
            }

            SelectedElement.Background = Brushes.Blue;

        }

        private void AddNewTermCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedElement.Text.Equals("");
            e.Handled = true;
        }

        private void AddNewTermCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddNewOrUpdateTermDialog anoutd = new AddNewOrUpdateTermDialog(this, "Add new term");
            anoutd.inicijalizacijaDialoga();
            anoutd.ShowDialog();
            
        }

        private void UpdateTermCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !SelectedElement.Text.Equals("");
            e.Handled = true;
        }

        private void UpdateTermCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddNewOrUpdateTermDialog anoutd = new AddNewOrUpdateTermDialog(this, "Update term");
            anoutd.inicijalizacijaDialoga();
            anoutd.ShowDialog();

        }

        /*public void promeniBoju(object selectedElement, Brush brush)
        {
            if (SelectedElement is TextBlock)
            {
                (SelectedElement as TextBlock).Background = brush;
            }
            else if (SelectedElement is Rectangle)
            {
                (SelectedElement as Rectangle).Fill = brush;
            }
            else MessageBox.Show("Selektovan element neocekivanog tipa!");
        }*/


        public UIElement GetMainGridElement(Grid grid, int r, int c, int numOfClassroom)
        {
            /*for (int i = 0; i < grid.Children.Count; i++)
            {
                UIElement e = grid.Children[i];
                if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c)
                    return e;
            }
            return null;*/

            int index = GetIndexOfMainGridElement(grid, r, c);
            
            try { return grid.Children[index];  }
            catch { return null; }
        }

        public int GetIndexOfMainGridElement(Grid grid, int r, int c)
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                UIElement element = grid.Children[i];
               
                if (Grid.GetRow(element) == r && Grid.GetColumn(element) == c)
                    return i;
                
                
            }
            return -1;
            //int index = r * NUM_OF_DAYS * numOfClassroom + c;
            //return index;

            /*string key = r + "_" + c;

            if(indeksi.ContainsKey(key))
            {
                return indeksi[key];
            }

            return -1;*/
        }

        public static int GetRowForTime(Grid grid, string time)
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                TextBlock tb = grid.Children[i] as TextBlock;
                if(tb != null)
                {
                    if (tb.Text.Contains(time))
                        return Grid.GetRow(tb);
                }
                
            }

            return -1;
        }

        public static int GetColumnForDay(Grid grid, string day)
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                TextBlock tb = grid.Children[i] as TextBlock;
                if (tb != null)
                {
                    if (tb.Text.Equals(day))
                        return Grid.GetColumn(tb);
                }
            }

            return -1;
        }

        public static int GetColumnForClassRoom(Grid grid, string classRoom, int columnOfDay, int numOfClassRooms)
        {
            int column;
            int lowLimit = columnOfDay;
            int highLimit = columnOfDay + numOfClassRooms - 1;

            for (int i = 0; i < grid.Children.Count; i++)
            {
                TextBlock tb = grid.Children[i] as TextBlock;
                if (tb != null)
                {
                    column = Grid.GetColumn(tb);

                    if (tb.Text.Equals(classRoom) &&  column >= lowLimit && column <= highLimit)
                        return column;
                }
            }

            return -1;
        }

       public TextBlock createNewTextBlock(string comboSubject)
        {
            TextBlock newTextBlock = new TextBlock() { Margin = new Thickness(2), Text = comboSubject, LayoutTransform = new RotateTransform(270), Background = Brushes.Aqua, Foreground = Brushes.Red/*, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center*/ };
            newTextBlock.MouseLeftButtonUp += Cell_MouseLeftButtonUp;
            newTextBlock.MouseRightButtonUp += Cell_MouseRightButtonUp;

            return newTextBlock;
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                // Find the data behind the ListViewItem
                Subject subject = (Subject)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", subject);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            List<Subject> s = autoComplete.collection.ToList();

            String text = this.autoComplete.Text;
            foreach(var sub in s)
            {
                if(sub.Name.Equals(text))
                {
                    // check if subject already exists on ListView
                    foreach(var subj in Subjects)
                    {
                        if(text.Equals(subj.Name))
                        {
                            return;
                        }
                    }
                    Subjects.Add(sub);
                }
            }

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = SubjectsListView.SelectedItem;
            for(int i=0; i<Subjects.Count; i++)
            {
                if(Subjects[i].Name.Equals(((Subject)selectedItem).Name))
                {
                    Subjects.RemoveAt(i);
                    break;
                }
            }
        }

        //private void ListView_Drop(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent("myFormat"))
        //    {
        //        Student student = e.Data.GetData("myFormat") as Student;
        //        Studenti.Remove(student);
        //        Studenti2.Add(student);
        //    }
        //}

    }
}
