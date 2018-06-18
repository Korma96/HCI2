using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for TutorialWindow.xaml
    /// </summary>
    public partial class TutorialWindow : Window
    {
        TutorialViewModel vm { get; set; }
        public event EventHandler handler;
        public event EventHandler handlerForCurrentView;
            
        public TutorialWindow()
        {
            InitializeComponent();
            vm = this.DataContext as TutorialViewModel;
            NextButton.IsEnabled = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            handler(this, e);
        }

        private void LetsStart_Click(object sender, RoutedEventArgs e)
        {
            handlerForCurrentView(vm.index, e);
            this.Hide();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if(vm.index >= vm.ExplanationList.Count - 1)
            {
                return;
            }
            vm.SwitchView = vm.index + 1;
            DescriptionTextBlock.Text = vm.ExplanationList[vm.index + 1];
            StepTextBlock.Text = "Step " + (vm.index + 2).ToString() + "/" + vm.ExplanationList.Count.ToString() + " :";
            vm.index++;
            if (vm.index == vm.ExplanationList.Count - 1)
            {
                NextButton.IsEnabled = false;
            }
            NextButton.IsEnabled = false;
        }

    }
}
