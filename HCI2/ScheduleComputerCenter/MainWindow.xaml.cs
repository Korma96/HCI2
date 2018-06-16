using ScheduleComputerCenter.View;
using ScheduleComputerCenter.Commands;
using ScheduleComputerCenter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace ScheduleComputerCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point startPoint = new Point();
        public ObservableCollection<Subject> Subjects { get; set; }
        public ObservableCollection<ObservableCollection<Term>> ObservableList { get; set; }

        private const int NUM_OF_DAYS = 6;
        public int NumOfClassrooms { get; set; }

        private const int NUM_OF_ROWS_IN_MAIN_GRID = 60;
        private const int NUM_OF_COLS_IN_MAIN_GRID = 36;

        public static double HEIGHT_OF_ROWS_IN_MAIN_GRID = 50.0;
        private const double WIDTH_OF_COLS_IN_MAIN_GRID = 32.0;

        public string[] days = { "PONEDELJAK", "UTORAK", "SREDA", "ČETVRTAK", "PETAK", "SUBOTA" };
        public List<Classroom> Classrooms { get; set; }
        private string[] times = { "07:00", "07:15", "07:30", "07:45", "08:00", "08:15", "08:30", "08:45", "09:00", "09:15", "09:30", "09:45", "10:00", "10:15", "10:30", "10:45", "11:00", "11:15", "11:30", "11:45", "12:00", "12:15", "12:30", "12:45", "13:00", "13:15", "13:30", "13:45", "14:00", "14:15", "14:30", "14:45", "15:00", "15:15", "15:30", "15:45", "16:00", "16:15", "16:30", "16:45", "17:00", "17:15", "17:30", "17:45", "18:00", "18:15", "18:30", "18:45", "19:00", "19:15", "19:30", "19:45", "20:00", "20:15", "20:30", "20:45", "21:00", "21:15", "21:30", "21:45", "22:00" };

        public ListView SelectedElement { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            WindowState = WindowState.Maximized;

            Subjects = new ObservableCollection<Subject>();
            FilterSubjectsForListView("");
            ObservableList = new ObservableCollection<ObservableCollection<Term>>();

            // popunjavanje baze
            //SComputerCentre.AddDummyData();

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

            definisiRedove(LeftGrid, NUM_OF_ROWS_IN_MAIN_GRID, HEIGHT_OF_ROWS_IN_MAIN_GRID);
            definisiKolone(LeftGrid, 1, 100);
            dodajTextBlockTimes(LeftGrid, NUM_OF_ROWS_IN_MAIN_GRID);

            definisiRedove(RightGrid, NUM_OF_ROWS_IN_MAIN_GRID, HEIGHT_OF_ROWS_IN_MAIN_GRID);
            definisiKolone(RightGrid, 1, 100);
            dodajTextBlockTimes(RightGrid, NUM_OF_ROWS_IN_MAIN_GRID);

            List<Day> days = null;
            try
            {
                days = ComputerCentre.DayRepository.GetAll().ToList();
            }
            catch(Exception e)
            {

            }
            
            if(days != null)
            {
                if (days.Count > 0)
                {
                    //ComputerCentre.DayRepository.RemoveRange(days);
                    //ComputerCentre.context.SaveChanges();

                    //if (days.Count > 0) NumOfClassrooms = days[0].Terms.Count / 60;
                    //else NumOfClassrooms = 0;

                    List<Term> termsForClassroom = new List<Term>(); ;

                    Term term;
                    int numOfTermsByClassroom = 0;
                    int totalNumOfTerms = 0;

                    foreach (Day day in days)
                    {
                        for (int i = 1; i <= day.Terms.Count; i++)
                        {
                            term = day.Terms[i - 1];
                            numOfTermsByClassroom += term.RowSpan;
                            termsForClassroom.Add(term);

                            if (numOfTermsByClassroom % 60 == 0)
                            {
                                ObservableList.Add(new ObservableCollection<Term>(termsForClassroom));

                                totalNumOfTerms += numOfTermsByClassroom;
                                numOfTermsByClassroom = 0;
                                termsForClassroom = new List<Term>();
                            }

                        }
                    }

                    Classrooms = ComputerCentre.ClassroomRepository.GetAll().ToList();
                    NumOfClassrooms = Classrooms.Count;
                    if (NumOfClassrooms > 0)
                    {
                        definisiRedove(TopTopGrid, 1, 40);
                        definisiKolone(TopTopGrid, NUM_OF_DAYS * NumOfClassrooms, WIDTH_OF_COLS_IN_MAIN_GRID);
                        dodajTextBlockDays(TopTopGrid, NUM_OF_DAYS, NumOfClassrooms);

                        definisiRedove(TopBottomGrid, 1, 40);
                        definisiKolone(TopBottomGrid, NUM_OF_DAYS * NumOfClassrooms, WIDTH_OF_COLS_IN_MAIN_GRID);
                        dodajTextBlockClassrooms(TopBottomGrid, NumOfClassrooms);

                        definisiRedove(BottomTopGrid, 1, 40);
                        definisiKolone(BottomTopGrid, NUM_OF_DAYS * NumOfClassrooms, WIDTH_OF_COLS_IN_MAIN_GRID);
                        dodajTextBlockClassrooms(BottomTopGrid, NumOfClassrooms);

                        definisiRedove(BottomBottomGrid, 1, 40);
                        definisiKolone(BottomBottomGrid, NUM_OF_DAYS * NumOfClassrooms, WIDTH_OF_COLS_IN_MAIN_GRID);
                        dodajTextBlockDays(BottomBottomGrid, NUM_OF_DAYS, NumOfClassrooms);

                        MainGrid.RowDefinitions.Add(new RowDefinition());
                        for (int i = 0; i < NUM_OF_DAYS * NumOfClassrooms; i++) MainGrid.ColumnDefinitions.Add(new ColumnDefinition());


                        DataTemplate dataTemplate = makeDataTemplate();

                        ListView listView;
                        
                        Style styleListView = new Style(typeof(ListView));
                        Style styleListViewItem = new Style(typeof(ListViewItem));

                        styleListView.Setters.Add(new EventSetter(ListViewItem.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Cell_MouseLeftButtonUp)));
                        styleListView.Setters.Add(new EventSetter(ListViewItem.MouseRightButtonUpEvent, new MouseButtonEventHandler(Cell_MouseRightButtonUp)));


                        /*Trigger trigger = new Trigger() { Property = ListViewItem.IsSelectedProperty,  Value = true };
                        trigger.Setters.Add(new Setter(ListViewItem.FontSizeProperty, 20));
                        trigger.Setters.Add(new Setter(ListViewItem.FontWeightProperty, "Bold"));
                        trigger.Setters.Add(new Setter() { TargetName = "Border", Property = BorderBrushProperty, Value = "Black" });
                        trigger.Setters.Add(new Setter() { TargetName = "Border", Property = BorderThicknessProperty, Value = new Thickness(2) });
                       */
                        //styleListViewItem.Triggers.Add(trigger);


                        //styleListViewItem.Setters.Add(new Setter(ListViewItem.BackgroundProperty, new Binding() { RelativeSource = new RelativeSource(RelativeSourceMode.Self), Path = new PropertyPath("IsSelected"), Converter = new Converters.BackGroundConverter() }));
                        styleListViewItem.Setters.Add(new Setter(ListViewItem.BackgroundProperty, Brushes.LightGray));
                        styleListViewItem.Setters.Add(new Setter(ListViewItem.HeightProperty, new Binding("RowSpan") { Converter = new Converters.HeightConverter() }));
                        //styleListViewItem.Setters.Add(new Setter(ListViewItem.BorderBrushProperty, Brushes.Red));
                        //styleListViewItem.Setters.Add(new Setter(ListViewItem.BorderThicknessProperty, new Binding() { RelativeSource = new RelativeSource(RelativeSourceMode.Self), Path = new PropertyPath("IsSelected"), Converter = new Converters.BackGroundConverter() }));

                        //styleListViewItem.Setters.Add(new EventSetter(ListViewItem.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Cell_MouseLeftButtonUp)));
                        //styleListViewItem.Setters.Add(new EventSetter(ListViewItem.MouseRightButtonUpEvent, new MouseButtonEventHandler(Cell_MouseRightButtonUp)));

                        /*try { 
                            Setter setter = new Setter() { Property = ListViewItem.TemplateProperty };
                            ControlTemplate controlTemplate = new ControlTemplate(typeof(ListViewItem));
                            FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
                            border.SetValue(Border.NameProperty, "Border");
                            border.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter)));
                            controlTemplate.VisualTree = border;
                            controlTemplate.Triggers.Add(trigger);
                            setter.Value = controlTemplate;
                            styleListView.Setters.Add(setter);
                        }
                        catch(Exception e)
                        {

                        }*/



                        for (int i = 0; i < NUM_OF_DAYS * NumOfClassrooms; i++)
                        {
                            listView = new ListView() { ItemsSource = ObservableList[i], Height = NUM_OF_ROWS_IN_MAIN_GRID * HEIGHT_OF_ROWS_IN_MAIN_GRID + 4 };

                            listView.Style = styleListView;
                            listView.ItemContainerStyle = styleListViewItem;

                            listView.AllowDrop = true;
                            listView.DragEnter += Subjects_DragEnter;
                            listView.Drop += Subjects_Drop;

                            listView.ItemTemplate = dataTemplate;

                            Grid.SetRow(listView, 0);
                            Grid.SetColumn(listView, i);
                            MainGrid.Children.Add(listView);
                        }


                        this.Focus(); // ovo smo stavili jer je na pocetku new command-a bila uvek disable-ovana
                    }
                    else
                    {
                        MessageBox.Show("Ne postoji nijedna ucionica!");
                    }

                }
                else
                {
                    MessageBox.Show("Ne postoji nijedan dan!");
                }
            }
            else
            {
                MessageBox.Show("Ne postoji nijedan dan!");
            }

        }

        private DataTemplate makeDataTemplate()
        {
            FrameworkElementFactory textBlockName = new FrameworkElementFactory(typeof(TextBlock));
            //textBlockName.SetValue(TextBlock.NameProperty, "SubjectName");
            textBlockName.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
            textBlockName.SetValue(LayoutTransformProperty, new RotateTransform(270));
            //textBlockName.SetValue(WidthProperty, HEIGHT_OF_ROWS_IN_MAIN_GRID);
            //textBlockName.SetBinding(TextBlock.TextProperty, new Binding("Id") {TargetNullValue = "***" });
            textBlockName.SetBinding(TextBlock.TextProperty, new Binding("Subject.Name"));
            textBlockName.SetBinding(TextBlock.ToolTipProperty, new Binding("Subject.Name"));

            FrameworkElementFactory textBlock1 = new FrameworkElementFactory(typeof(TextBlock));
            textBlock1.SetValue(TextBlock.TextProperty, " ");
            textBlock1.SetValue(LayoutTransformProperty, new RotateTransform(270));

            FrameworkElementFactory textBlockMark = new FrameworkElementFactory(typeof(TextBlock));
            //textBlockName.SetValue(TextBlock.NameProperty, "SubjectMark");
            textBlockMark.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
            textBlockMark.SetValue(LayoutTransformProperty, new RotateTransform(270));
            //textBlockMark.SetValue(WidthProperty, HEIGHT_OF_ROWS_IN_MAIN_GRID);
            //textBlockMark.SetBinding(TextBlock.TextProperty, new Binding("Id") { TargetNullValue = "***"});
            textBlockMark.SetBinding(TextBlock.TextProperty, new Binding("Subject.Course.Mark"));
            textBlockMark.SetBinding(TextBlock.ToolTipProperty, new Binding("Subject.Course.Mark"));

            //FrameworkElementFactory textBlock2 = new FrameworkElementFactory(typeof(TextBlock));
            //textBlock2.SetValue(TextBlock.TextProperty, ")");
            //textBlock2.SetValue(LayoutTransformProperty, new RotateTransform(270));

            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
            //stackPanel.SetBinding(StackPanel.HeightProperty, new Binding("RowSpan"));
            //stackPanel.SetValue(StackPanel.BackgroundProperty, Brushes.Aqua);
            //wrapPanel.SetValue(WrapPanel.HeightProperty, new GridLength(HEIGHT_OF_ROWS_IN_MAIN_GRID));
            stackPanel.AppendChild(textBlockName);
            stackPanel.AppendChild(textBlock1);
            stackPanel.AppendChild(textBlockMark);
            //stackPanel.AppendChild(textBlock2);

            DataTemplate dataTemplate = null;
            try
            {
                dataTemplate = new DataTemplate();
                dataTemplate.VisualTree = stackPanel;
            }
            catch { }

            /*DataTrigger dataTriggerName = new DataTrigger();
            dataTriggerName.Binding = new Binding("Subject.Name");
            dataTriggerName.Value = null;
            dataTriggerName.Setters.Add(new Setter(TextBlock.TextProperty, "***"));

            DataTrigger dataTriggerMark = new DataTrigger();
            dataTriggerMark.Binding = new Binding("Subject.Mark");
            dataTriggerMark.Value = null;
            dataTriggerMark.Setters.Add(new Setter(TextBlock.TextProperty, "***"));

            dataTemplate.Triggers.Add(dataTriggerName);
            dataTemplate.Triggers.Add(dataTriggerMark);*/

            return dataTemplate;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (!ComputerCentre.SoftwareRepository.GetAll().ToList().Any())
            {
                MessageBox.Show("No softwares available, please add software first!");
            }
            else
            {
                var w = new View.classrooms();
                w.ShowDialog();
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var w = new View.courses();
            w.ShowDialog();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            var w = new View.softwares();
            w.ShowDialog();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        { 
            if (!ComputerCentre.SoftwareRepository.GetAll().ToList().Any())
            {
                MessageBox.Show("No softwares available,\nplease add at least one software first!");
            }
            else if (!ComputerCentre.CourseRepository.GetAll().ToList().Any())
            {
                MessageBox.Show("No courses available,\nplease add at least one course first!");
            }
            else
            {
                var w = new View.SubjectsWindow();
                w.ShowDialog();
            }
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            var w = new View.classrooms();
            w.ShowDialog();
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

        private void dodajTextBlockDays(Grid grid, int rows, int numOfClassrooms)
        {
            TextBlock newTextBlock;

            for (int i = 0; i < rows; i++)
            {
                newTextBlock = new TextBlock() { Margin = new Thickness(2), Text = days[i], Background = Brushes.Gray };
                Grid.SetColumnSpan(newTextBlock, numOfClassrooms);
                Grid.SetRow(newTextBlock, 0);
                Grid.SetColumn(newTextBlock, i* numOfClassrooms);
                grid.Children.Add(newTextBlock);
            }
        }

        private void dodajTextBlockClassrooms(Grid grid, int numOfClassrooms)
        {
            TextBlock newTextBlock;
            for(int i = 0; i < NUM_OF_DAYS; i++)
            {
                for (int j = 0; j < numOfClassrooms; j++)
                {
                    newTextBlock = new TextBlock() { Margin = new Thickness(2), Text = Classrooms[j].Name, Background = Brushes.Gray };
                    Grid.SetRow(newTextBlock, 0);
                    Grid.SetColumn(newTextBlock, i*numOfClassrooms + j);
                    grid.Children.Add(newTextBlock);
                }
            }
        }

        /*private void dodajTextBlockove(int rows, int cols)
        {
            TextBlock newTextBlock;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    newTextBlock = new TextBlock() { Margin = new Thickness(2), Text = "", LayoutTransform = new RotateTransform(270), Background = Brushes.LightGray, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                    newTextBlock.MouseLeftButtonUp += Cell_MouseLeftButtonUp;
                    newTextBlock.MouseRightButtonUp += Cell_MouseRightButtonUp;
                    Grid.SetRow(newTextBlock, i);
                    Grid.SetColumn(newTextBlock, j);
                    MainGrid.Children.Add(newTextBlock);
                   
                }
            }
        }*/

        public static void definisiRedove(Grid grid, double rows, double height)
        {
            for(int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(height) });
            }
        }

        public static void definisiKolone(Grid grid, int cols, double width)
        {
            for (int i = 0; i < cols; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(width) });
            }
        }

        /*
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
        }*/

        private void Cell_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListView oldSelectedElement = SelectedElement;
            SelectedElement = sender as ListView;
            if (SelectedElement.SelectedItems.Count == 0) SelectedElement = null;

            if (oldSelectedElement != null)
            {
                if (SelectedElement != null)
                {
                    if (!oldSelectedElement.Equals(SelectedElement))
                    {
                        oldSelectedElement.SelectedItems.Clear();
                    }
                }
            }
        }

        private void Cell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListView oldSelectedElement = SelectedElement;
            SelectedElement = sender as ListView;
            if (SelectedElement.SelectedItems.Count == 0) SelectedElement = null;

            if (oldSelectedElement != null)
            {
                if(SelectedElement != null)
                {
                    if (oldSelectedElement.Equals(SelectedElement))
                    {

                        SelectedElement = null;
                    }
                }
                
                oldSelectedElement.SelectedItems.Clear();
            }

        }

        private void AddNewTermCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedElement == null) e.CanExecute = false;
            else
            {
                int indexOfClassroom = Grid.GetColumn(SelectedElement);
                int indexOfTerm = SelectedElement.SelectedIndex;

                e.CanExecute = (ObservableList[indexOfClassroom][indexOfTerm].Subject == null);
            }

            e.Handled = true;
        }

        private void AddNewTermCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddNewOrUpdateTermDialog anoutd = new AddNewOrUpdateTermDialog(this, "Add new term");
            anoutd.inicijalizacijaDialoga(null);
            anoutd.ShowDialog();
            
        }

        private void UpdateTermCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(SelectedElement == null) e.CanExecute = false;
            else
            {
                int indexOfClassroom = Grid.GetColumn(SelectedElement);
                int indexOfTerm = SelectedElement.SelectedIndex;

                e.CanExecute = (ObservableList[indexOfClassroom][indexOfTerm].Subject != null);
            }

            e.Handled = true;
        }

        private void UpdateTermCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddNewOrUpdateTermDialog anoutd = new AddNewOrUpdateTermDialog(this, "Update term");
            anoutd.inicijalizacijaDialoga(null);
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
        }


        public UIElement GetMainGridElement(Grid grid, int r, int c, int numOfClassroom)
        {
            /*for (int i = 0; i < grid.Children.Count; i++)
            {
                UIElement e = grid.Children[i];
                if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c)
                    return e;
            }
            return null;

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

            string key = r + "_" + c;

            if(indeksi.ContainsKey(key))
            {
                return indeksi[key];
            }

            return -1;
        }*/

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

       /*public TextBlock createNewTextBlock(string comboSubject)
        {
            TextBlock newTextBlock = new TextBlock() { Margin = new Thickness(2), Text = comboSubject, LayoutTransform = new RotateTransform(270), Background = Brushes.Aqua, Foreground = Brushes.Red, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            newTextBlock.MouseLeftButtonUp += Cell_MouseLeftButtonUp;
            newTextBlock.MouseRightButtonUp += Cell_MouseRightButtonUp;

            return newTextBlock;
        }*/

        private void Subjects_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void Subjects_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if(listViewItem != null)
                {
                    try
                    {
                        // Find the data behind the ListViewItem
                        Subject subject = (Subject)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);

                        // Initialize the drag & drop operation
                        DataObject dragData = new DataObject("myFormat", subject);
                        DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                    }
                    catch { }
                   
                }
                
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

        private void Subjects_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        //private void RemoveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var selectedItem = SubjectsListView.SelectedItem;
        //    for(int i=0; i<Subjects.Count; i++)
        //    {
        //        if(Subjects[i].Name.Equals(((Subject)selectedItem).Name))
        //        {
        //            Subjects.RemoveAt(i);
        //            break;
        //        }
        //    }
        //}

        private void Subjects_Drop(object sender, DragEventArgs e)
        {
            if(SelectedElement != null)
            {
                SelectedElement.SelectedItems.Clear();
            }

            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject subject = e.Data.GetData("myFormat") as Subject;
                //Subjects.Remove(subject);

                ListView listView = sender as ListView;
                int indexOfClassroom = Grid.GetColumn(listView);
                List<Classroom> classrooms = ComputerCentre.context.Classrooms.Include(s => s.Softwares).ToList();
                int numOfClassrooms = classrooms.Count();
                indexOfClassroom = indexOfClassroom % numOfClassrooms;

                Classroom classroom = classrooms[indexOfClassroom];
                if(!ClassroomMatchSubjectNeeds(classroom, subject))
                {
                    return;
                }
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
                int indexOfTerm = listView.ItemContainerGenerator.IndexFromContainer(listViewItem);
                //listView.SelectedItem = listViewItem;
                listView.SelectedIndex = indexOfTerm;
                SelectedElement = listView;

                AddNewOrUpdateTermDialog anoutd = new AddNewOrUpdateTermDialog(this, "Add new term");
                anoutd.inicijalizacijaDialoga(subject.Name);
                anoutd.ShowDialog();
                
                //ListView listView = ItemsControl.ItemsControlFromItemContainer(listViewItem) as ListView;

                /*int indexOfClassroom = Grid.GetColumn(listView);
                int indexOfTerm = listView.ItemContainerGenerator.IndexFromContainer(listViewItem);

                // Ovo radim jer ne osvezi raspored ako bih samo set-ovao subject za odabrani termin 
                Term newTerm = ObservableList[indexOfClassroom][indexOfTerm];
                newTerm.Subject = subject;
                ObservableList[indexOfClassroom].Insert(indexOfTerm, newTerm);
                ObservableList[indexOfClassroom].RemoveAt(indexOfTerm + 1);*/
            }
        }

        private bool ClassroomMatchSubjectNeeds(Classroom classroom, Subject subject)
        {
            if(subject.Projector)
            {
                if(!classroom.Projector)
                {
                    MessageBox.Show("Projector doesn`t exist in classroom!");
                    return false;
                }
            }
            if(subject.SmartTable)
            {
                if(!classroom.SmartTable)
                {
                    MessageBox.Show("Smart table doesn`t exist in classroom!");
                    return false;
                }
            }
            if(subject.Table)
            {
                if(!classroom.Table)
                {
                    MessageBox.Show("Table doesn`t exist in classroom!");
                    return false;
                }
            }
            if (!classroom.Softwares.Contains(subject.Software))
            {
                MessageBox.Show("Software doesn`t exist in classroom!");
                return false;
            }
            return true;
        }

        //private void AddButton_Click(object sender, RoutedEventArgs e)
        //{
        //    List<Subject> s = autoComplete.collection.ToList();

        //    String text = this.autoComplete.Text;
        //    foreach (var sub in s)
        //    {
        //        if (sub.Name.Equals(text))
        //        {
        //            // check if subject already exists on ListView
        //            foreach (var subj in Subjects)
        //            {
        //                if (text.Equals(subj.Name))
        //                {
        //                    return;
        //                }
        //            }
        //            Subjects.Add(sub);
        //        }
        //    }

        //}

        public void FilterSubjectsForListView(string text)
        {
            List<Subject> subjects;
            try
            {
                subjects = ComputerCentre.SubjectRepository.GetAll().AsQueryable().Include(x => x.Software).ToList();
            }
            catch
            {
                subjects = new List<Subject>();
            }
            Subjects = new ObservableCollection<Subject>();
            subjects = subjects.OrderBy(o => o.Name).ToList(); ;
            foreach(var s in subjects)
            {
                if (s.Name.ToUpper().Contains(text.ToUpper()))
                {
                    Subjects.Add(s);
                }
            }
            SubjectsListView.ItemsSource = Subjects;
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = FilterTextBox.Text;
            FilterSubjectsForListView(text);
        }

        private void StartTutorial(object sender, RoutedEventArgs e)
        {
            var t = new TutorialWindow();
            t.ShowDialog();
        }
    }
}
