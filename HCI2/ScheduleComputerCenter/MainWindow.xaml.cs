using ScheduleComputerCenter.View;
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

namespace ScheduleComputerCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const int NUM_OF_DAYS = 6;

        public object SelectedElement { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            //SelectedElement = new Rectangle();

            WindowState = WindowState.Maximized;
            
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
            object oldSelectedElement = SelectedElement;
            SelectedElement = sender;

            if (oldSelectedElement != null) // prethodno je bio neki element selektovan
            {
                if (!oldSelectedElement.Equals(SelectedElement))
                {
                    promeniBoju(oldSelectedElement, Brushes.LightGray);
                }
                
            }

            promeniBoju(SelectedElement, Brushes.Blue);
        }

        private void Cell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            object oldSelectedElement = SelectedElement;

            if (oldSelectedElement != null) // prethodno je bio neki element selektovan
            {
                promeniBoju(oldSelectedElement, Brushes.LightGray);
            }

            SelectedElement = sender;
            if (oldSelectedElement != null)
            {
                if (oldSelectedElement.Equals(SelectedElement))
                {
                    SelectedElement = null;
                    return;
                }
            }

            promeniBoju(SelectedElement, Brushes.Blue);

        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (SelectedElement != null);
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddNewTermDialog antd = new AddNewTermDialog(this);
            antd.ShowDialog();
        }

        private void promeniBoju(object selectedElement, Brush brush)
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


        public static UIElement GetMainGridElement(Grid grid, int r, int c, int numOfClassroom)
        {
            /*for (int i = 0; i < grid.Children.Count; i++)
            {
                UIElement e = grid.Children[i];
                if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c)
                    return e;
            }
            return null;*/

            int index = GetIndexOfMainGridElement(grid, r, c, numOfClassroom);
            
            try { return grid.Children[index];  }
            catch { return null; }
        }

        public static int GetIndexOfMainGridElement(Grid grid, int r, int c, int numOfClassroom)
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                Rectangle rect = grid.Children[i] as Rectangle;
                if(rect != null)
                {
                    if (Grid.GetRow(rect) == r && Grid.GetColumn(rect) == c)
                        return i;
                }
                
            }
            return -1;
            //int index = r * NUM_OF_DAYS * numOfClassroom + c;
            //return index;
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
            TextBlock newTextBlock = new TextBlock() { Margin = new Thickness(2), Text = comboSubject, LayoutTransform = new RotateTransform(270), Background = Brushes.LightGray, Foreground = Brushes.Red/*, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center*/ };
            newTextBlock.MouseLeftButtonUp += Cell_MouseLeftButtonUp;
            newTextBlock.MouseRightButtonUp += Cell_MouseRightButtonUp;

            return newTextBlock;
        }

    }
}
