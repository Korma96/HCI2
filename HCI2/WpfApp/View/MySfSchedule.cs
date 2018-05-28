using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Syncfusion.UI.Xaml.Schedule;

namespace WpfApp.View
{
    public class MySfSchedule : SfSchedule
    {
        public MySfSchedule()
        {
            //this.PreviewMouseDown += new MouseButtonEventHandler(DisableNavigationClick);
            //this.VisibleDatesChan;
        }

        private void DisableNavigationClick(object sender, MouseButtonEventArgs e)
        {
        }

        public void MoveToNextView()
        {
            
        }

        public void MoveToPreviousView()
        {

        }

        public new void MoveToDate(DateTime dateTime)
        {
             
        }

        public new void MoveToTime(TimeSpan ts)
        {

        }
       
    }
}
