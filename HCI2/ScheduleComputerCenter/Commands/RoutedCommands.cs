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

        public static readonly RoutedUICommand ClassroomCommand = new RoutedUICommand(
            "Classroom Command",
            "ClassroomCommand",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Q, ModifierKeys.Control),
            }
            );
        public static readonly RoutedUICommand SubjectCommand = new RoutedUICommand(
            "Subject Command",
            "SubjectCommand",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.W, ModifierKeys.Control),
            }
            );
        public static readonly RoutedUICommand SoftwareCommand = new RoutedUICommand(
            "Software Command",
            "SoftwareCommand",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.A, ModifierKeys.Control),
            }
            );
        public static readonly RoutedUICommand CourseCommand = new RoutedUICommand(
            "Course Command",
            "CourseCommand",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Z, ModifierKeys.Control),
            }
            );

        public static readonly RoutedUICommand StartTutorialCommand = new RoutedUICommand(
            "Start Tutorial Command",
            "StartTutorialCommand",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.T, ModifierKeys.Control),
            }
            );

        public static ICommand EndTutorialCommand = new RoutedUICommand(
            "End Tutorial Command",
            "EndTutorialCommand",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E, ModifierKeys.Control),
            }
            );
    }
}
