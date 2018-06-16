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
using ScheduleComputerCenter.Repository;

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

        public static int NUM_OF_DAYS = 6;
        public static int NUM_OF_TERMS = 60;
        public int NumOfClassrooms { get; set; }

        private const int NUM_OF_ROWS_IN_MAIN_GRID = 60;
        private const int NUM_OF_COLS_IN_MAIN_GRID = 36;

        public static double HEIGHT_OF_ROWS_IN_MAIN_GRID = 50.0;
        private const double WIDTH_OF_COLS_IN_MAIN_GRID = 32.0;

        public string[] days = { "PONEDELJAK", "UTORAK", "SREDA", "ČETVRTAK", "PETAK", "SUBOTA" };

        public List<Classroom> Classrooms { get; set; }
        public List<Day> Days { get; set; }

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
            //ComputerCentre.AddDummyData();

            CommandBinding AddNewTermCommandBinding = new CommandBinding(RoutedCommands.AddNewTermCommand, AddNewTermCommand_Executed, AddNewTermCommand_CanExecute);
            CommandBinding UpdateTermCommandBinding = new CommandBinding(RoutedCommands.UpdateTermCommand, UpdateTermCommand_Executed, UpdateTermCommand_CanExecute);
            CommandBinding RemoveTermCommandBinding = new CommandBinding(RoutedCommands.RemoveTermCommand, RemoveTermCommand_Executed, RemoveTermCommand_CanExecute);
            CommandBinding ClassroomCommandBinding = new CommandBinding(RoutedCommands.ClassroomCommand, ClassroomCommand_Executed, ClassroomCommand_CanExecute);
            CommandBinding SubjectCommandBinding = new CommandBinding(RoutedCommands.SubjectCommand, SubjectCommand_Executed, SubjectCommand_CanExecute);
            CommandBinding CourseCommandBinding = new CommandBinding(RoutedCommands.CourseCommand, CourseCommand_Executed, CourseCommand_CanExecute);
            CommandBinding SoftwareCommandBinding = new CommandBinding(RoutedCommands.SoftwareCommand, SoftwareCommand_Executed, SoftwareCommand_CanExecute);
            CommandBinding StartTutorialCommandBinding = new CommandBinding(RoutedCommands.StartTutorialCommand, StartTutorialCommand_Executed, StartTutorialCommand_CanExecute);
            this.CommandBindings.Add(AddNewTermCommandBinding);
            this.CommandBindings.Add(UpdateTermCommandBinding);
            this.CommandBindings.Add(RemoveTermCommandBinding);
            this.CommandBindings.Add(ClassroomCommandBinding);
            this.CommandBindings.Add(SubjectCommandBinding);
            this.CommandBindings.Add(CourseCommandBinding);
            this.CommandBindings.Add(SoftwareCommandBinding);
            this.CommandBindings.Add(StartTutorialCommandBinding);

            Menu menu = new Menu() { Background = Brushes.Gray, Height = 25 };
            MenuItem Data = new MenuItem() { Header = "Data", FontWeight = FontWeights.Heavy, Height = 25 };
            MenuItem classrooms = new MenuItem() { Header = "Classrooms", Command = RoutedCommands.ClassroomCommand };
            MenuItem courses = new MenuItem() { Header = "Courses", Command = RoutedCommands.CourseCommand };
            MenuItem softwares = new MenuItem() { Header = "Softwares", Command = RoutedCommands.SoftwareCommand };
            MenuItem subjects = new MenuItem() { Header = "Subjects", Command = RoutedCommands.SubjectCommand };
            MenuItem Schedule = new MenuItem() { Header = "Schedule", FontWeight = FontWeights.Heavy, Height = 25 };
            MenuItem Tutorial = new MenuItem() { Header = "Tutorial", FontWeight = FontWeights.Heavy, Height = 25 };
            MenuItem StartTutorial = new MenuItem() { Header = "StartTutorial", Command = RoutedCommands.StartTutorialCommand};
            Tutorial.Items.Add(StartTutorial);

            Data.Items.Add(classrooms);
            Data.Items.Add(softwares);
            Data.Items.Add(subjects);
            Data.Items.Add(courses);
            menu.Items.Add(Data);
            menu.Items.Add(Schedule);
            menu.Items.Add(Tutorial);
            DockPanel.SetDock(menu, Dock.Top);
            MainDockPanel.Children.Add(menu);

            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItemNew = new MenuItem() { Header = "Add New Term...", Command = RoutedCommands.AddNewTermCommand };
            menuItemNew.Icon = new Image() { Source = new BitmapImage(new Uri("/ScheduleComputerCenter;component/Images/add_new.png", UriKind.RelativeOrAbsolute)) };
            contextMenu.Items.Add(menuItemNew);
            MenuItem menuItemUpdate = new MenuItem() { Header = "Update Term...", Command = RoutedCommands.UpdateTermCommand };
            menuItemUpdate.Icon = new Image() { Source = new BitmapImage(new Uri("/ScheduleComputerCenter;component/Images/update.png", UriKind.RelativeOrAbsolute)) };
            contextMenu.Items.Add(menuItemUpdate);
            MenuItem menuItemRemove = new MenuItem() { Header = "Remove Term...", Command = RoutedCommands.RemoveTermCommand };
            menuItemRemove.Icon = new Image() { Source = new BitmapImage(new Uri("/ScheduleComputerCenter;component/Images/remove.png", UriKind.RelativeOrAbsolute)) };
            contextMenu.Items.Add(menuItemRemove);
            MainGrid.ContextMenu = contextMenu;

            definisiRedove(LeftGrid, NUM_OF_ROWS_IN_MAIN_GRID, HEIGHT_OF_ROWS_IN_MAIN_GRID);
            definisiKolone(LeftGrid, 1, 100);
            dodajTextBlockTimes(LeftGrid, NUM_OF_ROWS_IN_MAIN_GRID);

            definisiRedove(RightGrid, NUM_OF_ROWS_IN_MAIN_GRID, HEIGHT_OF_ROWS_IN_MAIN_GRID);
            definisiKolone(RightGrid, 1, 100);
            dodajTextBlockTimes(RightGrid, NUM_OF_ROWS_IN_MAIN_GRID);

            prikaziUcioniceDaneTermine();

            this.Focus(); // ovo smo stavili jer je na pocetku new command-a bila uvek disable-ovana

        }

        public void ukloniUcioniceDaneTermine()
        {
            TopTopGrid.Children.Clear();
            TopBottomGrid.Children.Clear();
            BottomTopGrid.Children.Clear();
            BottomBottomGrid.Children.Clear();
            MainGrid.Children.Clear();

            TopTopGrid.RowDefinitions.Clear();
            TopBottomGrid.RowDefinitions.Clear();
            BottomTopGrid.RowDefinitions.Clear();
            BottomBottomGrid.RowDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();

            TopTopGrid.ColumnDefinitions.Clear();
            TopBottomGrid.ColumnDefinitions.Clear();
            BottomTopGrid.ColumnDefinitions.Clear();
            BottomBottomGrid.ColumnDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();

            ObservableList.Clear();
        }

        public void prikaziUcioniceDaneTermine()
        {
            Days = null;
            try
            {
                Days = ComputerCentre.DayRepository.GetAll().ToList();
            }
            catch { }

            if (Days != null)
            {
                if (Days.Count > 0)
                {
                    List<Term> termsForClassroom = new List<Term>(); ;

                    Term term;
                    int numOfTermsByClassroom = 0;
                    int totalNumOfTerms = 0;

                    Days.Sort(delegate (Day x, Day y)
                    {

                        return x.Id.CompareTo(y.Id);

                    });

                    foreach (Day day in Days)
                    {
                        day.Terms.Sort(delegate (Term x, Term y)
                        {
                            int res = x.ClassroomIndex.CompareTo(y.ClassroomIndex);
                            if (res == 0)
                            {
                                return x.StartTime.CompareTo(y.StartTime);
                            }
                            return res;
                        });
                        //day.Terms.Sort(delegate (Term x, Term y) { return x.StartTime.CompareTo(y.StartTime); });

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

                    Classrooms = ComputerCentre.context.Classrooms.Include(s => s.Softwares).ToList();
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

                        styleListViewItem.Setters.Add(new Setter(ListViewItem.BackgroundProperty, Brushes.LightGray));
                        styleListViewItem.Setters.Add(new Setter(ListViewItem.HeightProperty, new Binding("RowSpan") { Converter = new Converters.HeightConverter() }));


                        for (int i = 0; i < NUM_OF_DAYS * NumOfClassrooms; i++)
                        {
                            listView = new ListView() { ItemsSource = ObservableList[i], Height = NUM_OF_ROWS_IN_MAIN_GRID * HEIGHT_OF_ROWS_IN_MAIN_GRID + 4 };

                            listView.Style = styleListView;
                            listView.ItemContainerStyle = styleListViewItem;

                            listView.AllowDrop = true;
                            listView.DragEnter += Subjects_DragEnter;
                            listView.DragEnter += Terms_DragEnter;
                            listView.Drop += SubjectsOrTerms_Drop;
                            listView.PreviewMouseLeftButtonDown += SubjectsOrTerms_PreviewMouseLeftButtonDown;
                            listView.MouseMove += Terms_MouseMove;

                            listView.ItemTemplate = dataTemplate;

                            Grid.SetRow(listView, 0);
                            Grid.SetColumn(listView, i);
                            MainGrid.Children.Add(listView);
                        }

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

            this.Focus(); // ovo smo stavili jer je na pocetku new command-a bila uvek disable-ovana

        }

        private void SoftwareCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void CourseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void SubjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void ClassroomCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void SoftwareCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var w = new View.softwares();
            w.ShowDialog();

        }

        private void CourseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var w = new View.courses();
            w.ShowDialog();
        }

        private void SubjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
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

        public void dodajNoveTermineZaNovuUcionicu()
        {
            List<Term> newTerms;
            Day day;

            for (int i = 0; i < MainWindow.NUM_OF_DAYS; i++)
            {
                day = Days[i];
                newTerms = new List<Term>();

                for (int j = 0; j < NUM_OF_TERMS; j++)
                {
                    newTerms.Add(new Term(Day.times[j], Day.times[j + 1], null, day, NumOfClassrooms));
                }

                ComputerCentre.context.Entry(day).State = EntityState.Modified;
                day.Terms.AddRange(newTerms);
                ComputerCentre.context.SaveChanges();
            }
        }

        private void ClassroomCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!ComputerCentre.SoftwareRepository.GetAll().ToList().Any())
            {
                MessageBox.Show("No softwares available, please add software first!");
            }
            else
            {
                var w = new View.classrooms(this);
                w.ShowDialog();
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
            textBlockName.SetBinding(TextBlock.TextProperty, new Binding("Subject.Code"));
            textBlockName.SetBinding(TextBlock.ToolTipProperty, new Binding("Subject.Code"));

            FrameworkElementFactory textBlock1 = new FrameworkElementFactory(typeof(TextBlock));
            textBlock1.SetValue(TextBlock.TextProperty, " ");
            textBlock1.SetValue(LayoutTransformProperty, new RotateTransform(270));

            FrameworkElementFactory textBlockMark = new FrameworkElementFactory(typeof(TextBlock));
            //textBlockName.SetValue(TextBlock.NameProperty, "SubjectMark");
            textBlockMark.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
            textBlockMark.SetValue(LayoutTransformProperty, new RotateTransform(270));
            //textBlockMark.SetValue(WidthProperty, HEIGHT_OF_ROWS_IN_MAIN_GRID);
            //textBlockMark.SetBinding(TextBlock.TextProperty, new Binding("Id") { TargetNullValue = "***"});
            textBlockMark.SetBinding(TextBlock.TextProperty, new Binding("Subject.Course.Code"));
            textBlockMark.SetBinding(TextBlock.ToolTipProperty, new Binding("Subject.Course.Code"));

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

        private void dodajTextBlockTimes(Grid grid, int rows)
        {
            TextBlock newTextBlock;

            for (int i = 0; i < rows; i++)
            {
                newTextBlock = new TextBlock() { Margin = new Thickness(2), Text = Day.times[i] + " - " + Day.times[i + 1], Background = Brushes.Gray };
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

                if (indexOfTerm == -1) e.CanExecute = false;
                else e.CanExecute = (ObservableList[indexOfClassroom][indexOfTerm].Subject == null);
            }

            e.Handled = true;
        }

        private void AddNewTermCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddNewOrUpdateTermDialog anoutd = new AddNewOrUpdateTermDialog(this, "Add new term", null, -1, -1);
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

                if (indexOfTerm == -1) e.CanExecute = false;
                else e.CanExecute = (ObservableList[indexOfClassroom][indexOfTerm].Subject != null);
            }

            e.Handled = true;
        }

        private void UpdateTermCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indexOfClassroom = Grid.GetColumn(SelectedElement);
            int indexOfTerm = SelectedElement.SelectedIndex;

            Term term = ObservableList[indexOfClassroom][indexOfTerm];
            AddNewOrUpdateTermDialog anoutd = new AddNewOrUpdateTermDialog(this, "Update term", term, indexOfTerm, indexOfClassroom);
            anoutd.inicijalizacijaDialoga(term.Subject);
            anoutd.ShowDialog();

        }

        private void StartTutorialCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void StartTutorialCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var t = new TutorialWindow();
            t.ShowDialog();
        }

        private void RemoveTermCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedElement == null) e.CanExecute = false;
            else
            {
                int indexOfClassroom = Grid.GetColumn(SelectedElement);
                int indexOfTerm = SelectedElement.SelectedIndex;

                if (indexOfTerm == -1) e.CanExecute = false;
                else e.CanExecute = (ObservableList[indexOfClassroom][indexOfTerm].Subject != null);
            }

            e.Handled = true;
        }

        private void RemoveTermCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indexOfClassroom = Grid.GetColumn(SelectedElement);
            int indexOfTerm = SelectedElement.SelectedIndex;

            Term term = ObservableList[indexOfClassroom][indexOfTerm];

            string removeMessageBoxText = "Are you sure you want to remove selected term?";
            string removeCaption = "Remove Term";

           
            MessageBoxResult rsltMessageBox = MessageBox.Show(removeMessageBoxText, removeCaption, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(rsltMessageBox == MessageBoxResult.Yes)
            {
                ComputerCentre.context.Entry(term.Day).State = EntityState.Modified;
                obrisiTerminiUbaciPrazneTermine(term, indexOfTerm, indexOfClassroom);
                ComputerCentre.context.SaveChanges();
                SelectedElement.SelectedItems.Clear();
                SelectedElement = null;

            }

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


        private void SubjectsOrTerms_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void Terms_MouseMove(object sender, MouseEventArgs e)
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

                if (listViewItem != null)
                {
                    try
                    {
                        // Find the data behind the ListViewItem
                        Term term = (Term)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);

                        if(term.Subject != null)
                        {
                            listView.SelectedIndex = listView.ItemContainerGenerator.IndexFromContainer(listViewItem);
                            SelectedElement = listView;

                            // Initialize the drag & drop operation
                            DataObject dragData = new DataObject("myFormatTerm", term);
                            DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                        }

                    }
                    catch { }
                }
            }
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
                        DataObject dragData = new DataObject("myFormatSubject", subject);
                        DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Copy);
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
            if (!e.Data.GetDataPresent("myFormatSubject") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Terms_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormatTerm") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
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

        private void SubjectsOrTerms_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent("myFormatSubject"))
            {
                if (SelectedElement != null)
                {
                    SelectedElement.SelectedItems.Clear();
                }

                Subject subject = e.Data.GetData("myFormatSubject") as Subject;
                //Subjects.Remove(subject);

                ListView listView = sender as ListView;
                //int indexOfClassroom = Grid.GetColumn(listView);
                //List<Classroom> classrooms = ComputerCentre.context.Classrooms.Include(s => s.Softwares).ToList();
                //int numOfClassrooms = classrooms.Count();
                //indexOfClassroom = indexOfClassroom % numOfClassrooms;

                //Classroom classroom = classrooms[indexOfClassroom];
                //if (!ClassroomMatchSubjectNeeds(classroom, subject))
                //{
                //    return;
                //}
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
                int indexOfTerm = listView.ItemContainerGenerator.IndexFromContainer(listViewItem);
                //listView.SelectedItem = listViewItem;
                listView.SelectedIndex = indexOfTerm;
                SelectedElement = listView;

                AddNewOrUpdateTermDialog anoutd = new AddNewOrUpdateTermDialog(this, "Add new term", null, -1, -1);
                anoutd.inicijalizacijaDialoga(subject);
                anoutd.ShowDialog();
            }
            else if (e.Data.GetDataPresent("myFormatTerm"))
            {
                Term term = e.Data.GetData("myFormatTerm") as Term;
                int oldTermRow = SelectedElement.SelectedIndex;
                int oldTermColumn = Grid.GetColumn(SelectedElement);

                if (SelectedElement != null)
                {
                    //SelectedElement.SelectedItems.Clear();
                    for (int i = 0; i < MainGrid.Children.Count; i++) (MainGrid.Children[i] as ListView).SelectedItems.Clear();
                }

                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
                int indexOfTerm = listView.ItemContainerGenerator.IndexFromContainer(listViewItem);
                listView.SelectedIndex = indexOfTerm;
                SelectedElement = listView;

                AddNewOrUpdateTermDialog anoutd = new AddNewOrUpdateTermDialog(this, "Change term", term, oldTermRow, oldTermColumn);
                anoutd.inicijalizacijaDialoga(term.Subject);
                anoutd.ShowDialog();


            }
        }

        public void obrisiTerminiUbaciPrazneTermine(Term oldTerm, int row, int column)
        {
            Day day = oldTerm.Day;

            ObservableList[column].RemoveAt(row);
            ComputerCentre.TermRepository.Remove(oldTerm);

            //TimeSpan startTime = oldTerm.StartTime;
            //TimeSpan endTime = startTime.Add(new TimeSpan(0, 15, 0));
            TimeSpan startTime = oldTerm.StartTime.Add(new TimeSpan(0, (oldTerm.RowSpan - 1) * 15, 0));
            TimeSpan endTime = startTime.Add(new TimeSpan(0, 15, 0));

            Term newTerm;

            for (int i = 0; i < oldTerm.RowSpan; i++)
            {
                //day.Terms.Insert(row, new Term(startTime, endTime, null, day, oldTerm.ClassroomIndex));
                newTerm = ComputerCentre.context.Terms.Add(new Term(startTime, endTime, null, day, oldTerm.ClassroomIndex));
                ObservableList[column].Insert(row, newTerm);
                endTime = startTime;
                startTime = startTime.Subtract(new TimeSpan(0, 15, 0));
            }
        }

        public bool ClassroomMatchSubjectNeeds(Classroom classroom, Subject subject)
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

            if (subject.Softwares != null)
            {
                if(classroom.Softwares != null)
                {
                    foreach (Software s in subject.Softwares)
                    {
                        if (!classroom.Softwares.Contains(s))
                        {
                            MessageBox.Show("In classroom " + classroom.Code + " are the following softwares: " + SubjectsWindow.SoftwaresToString(classroom.Softwares) + ". Required softwares for the subject " + subject.Code + ": " + SubjectsWindow.SoftwaresToString(classroom.Softwares) + ".");
                            return false;
                        }
                    }
                }
                else
                {
                    if(subject.Softwares.Count > 0)
                    {
                        MessageBox.Show("There is no software in the classroom " + classroom.Code + ". Required softwares for the subject " + subject.Code + ": " + SubjectsWindow.SoftwaresToString(classroom.Softwares) + ".");
                        return false;
                    }
                }
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
                subjects = ComputerCentre.context.Subjects.Include(s => s.Softwares).Include(x => x.Course).ToList();
                //subjects = ComputerCentre.SubjectRepository.GetAll().AsQueryable().Include(x => x.Softwares).Include(x => x.Course).ToList();
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

    }
}
