using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ScheduleComputerCenter.View
{
    public class TutorialViewModel : INotifyPropertyChanged
    {
        private int _switchView;
        public int SwitchView
        {
            get
            {
                return _switchView;
            }
            set
            {
                _switchView = value;
                OnPropertyChanged("SwitchView");
            }
        }
        public List<String> ExplanationList { get; set; }
        public List<Image> ImagesList { get; set; }
        public int index;
        public event PropertyChangedEventHandler PropertyChanged;
        public string StepTextBlockText { get; set; }
        public string DescriptionTextBlockText { get; set; }

        public TutorialViewModel()
        {
            SwitchView = 0;
            ExplanationList = new List<string>()
            {
                " Adding term to schedule using drag and drop",
                " Adding term to schedule using mouse right click, or using shortcut(CTRL+N)",
                " Updating term using mouse right click, or using shortcut(CTRL+U)",
                " CRUD operations on entities by choosing option from menu or using shortcuts"
            };
            index = 0;
            StepTextBlockText = "Step " + (index + 1).ToString() + "/" + ExplanationList.Count.ToString() + " :";
            DescriptionTextBlockText = ExplanationList[0];
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
//if (index >= ExplanationList.Count)
//{
//    NextButton.IsEnabled = false;
//    return;
//}
//StepStackPanel.Children.RemoveAt(0);
//if(index == 1 || index == 2)
//{
//    StepStackPanel.Children.RemoveAt(0);
//}
//StepTextBlock.Text = StepsList[index];
//DescriptionTextBlock.Text = ExplanationList[index];

//Image nextImage = ImagesList[index];
//Viewbox vb = new Viewbox();
//vb.HorizontalAlignment = HorizontalAlignment.Left;
//vb.Child = nextImage;
//if (index == 0 || index == 1)
//{
//    vb.Width = 400;
//    vb.Height = 375;
//}
//else
//{
//    vb.Width = 800;
//    vb.Height = 375;
//}

//StepStackPanel.Children.Add(vb);
//if(index == 0)
//{
//    Image nextImage2 = new Image() { Source = new BitmapImage(new Uri("../../Images/add_subject2.png", UriKind.RelativeOrAbsolute)) };
//    Viewbox vb2 = new Viewbox();
//    vb2.HorizontalAlignment = HorizontalAlignment.Right;
//    vb2.Child = nextImage2;
//    vb2.Width = 400;
//    vb2.Height = 375;
//    StepStackPanel.Children.Add(vb2);
//}
//if (index == 1)
//{
//    Image nextImage2 = new Image() { Source = new BitmapImage(new Uri("../../Images/update2.png", UriKind.RelativeOrAbsolute)) };
//    Viewbox vb2 = new Viewbox();
//    vb2.HorizontalAlignment = HorizontalAlignment.Right;
//    vb2.Child = nextImage2;
//    vb2.Width = 400;
//    vb2.Height = 375;
//    StepStackPanel.Children.Add(vb2);
//}
//index++;
//if (index >= ExplanationList.Count)
//{
//    NextButton.IsEnabled = false;
//}
//UpdateLayout();
//InvalidateVisual();
