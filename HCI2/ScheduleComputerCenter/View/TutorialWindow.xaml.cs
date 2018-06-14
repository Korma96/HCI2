using System.Windows;

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for TutorialWindow.xaml
    /// </summary>
    public partial class TutorialWindow : Window
    {
        TutorialViewModel vm { get; set; }
        public TutorialWindow()
        {
            InitializeComponent();
            vm = this.DataContext as TutorialViewModel;
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
            if(vm.index == vm.ExplanationList.Count -1)
            {
                NextButton.IsEnabled = false;
            }
        }
    }
}
