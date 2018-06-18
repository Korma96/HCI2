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
                " Moving term using drag and drop",
                " Adding term using mouse right click, or using shortcut(CTRL+N)",
                " Updating term using mouse right click, or using shortcut(CTRL+U)",
                " Removing term using mouse right click, or using shortcut(CTRL+R)",
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
