using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleComputerCenter.Commands
{
    public static class RoutedCommands
    {
        public static readonly RoutedUICommand AddNewTermCommand = new RoutedUICommand(
            "Add New Term Command",
            "AddNewTermCommand",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control),
            }
            );

        public static readonly RoutedUICommand UpdateTermCommand = new RoutedUICommand(
            "Update Term Command",
            "UpdateTermCommand",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.U, ModifierKeys.Control),
            }
            );

        public static readonly RoutedUICommand RemoveTermCommand = new RoutedUICommand(
            "Remove Term Command",
            "RemoveTermCommand",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Control),
            }
            );
    }
}
