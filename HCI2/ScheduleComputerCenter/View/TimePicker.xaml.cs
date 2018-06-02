﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleComputerCenter.View
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
  
        }

        public void setHoursAndMinutes(string hoursAndMinutes)
        {
            string[] tokens = hoursAndMinutes.Split(':');
            this.txtHours.Text = tokens[0].Trim();
            this.txtMinutes.Text = tokens[1].Trim();

            int value;
            int.TryParse(this.txtHours.Text, out value);
            OldHours = value;
            int.TryParse(this.txtMinutes.Text, out value);
            OldMinutes = value;
        }

        public int OldHours { get; set; }
        public int OldMinutes { get; set; }

        #region Properties

        /// <summary>
        /// Gets or sets the date time value.
        /// </summary>
        /// <value>The date time value.</value>
        public DateTime? DateTimeValue
        {
            get
            {
                string hours = this.txtHours.Text;
                string minutes = this.txtMinutes.Text;
                if (!string.IsNullOrWhiteSpace(hours) && !string.IsNullOrWhiteSpace(minutes))
                {
                    string value = string.Format("{0}:{1}", this.txtHours.Text, this.txtMinutes.Text);
                    DateTime time = DateTime.Parse(value);
                    return time;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                DateTime? time = value;
                if (time.HasValue)
                {
                    string timeString = time.Value.ToShortTimeString();
                    //9:54
                    string[] values = timeString.Split(':');
                    if (values.Length == 2)
                    {
                        this.txtHours.Text = values[0];
                        this.txtMinutes.Text = values[1];
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the time span value.
        /// </summary>
        /// <value>The time span value.</value>
        public TimeSpan? TimeSpanValue
        {
            get
            {
                DateTime? time = this.DateTimeValue;
                if (time.HasValue)
                {
                    return new TimeSpan(time.Value.Ticks);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                TimeSpan? timeSpan = value;
                if (timeSpan.HasValue)
                {
                    this.DateTimeValue = new DateTime(timeSpan.Value.Ticks);
                }
            }
        }

        #endregion

        #region Event Subscriptions

        /// <summary>
        /// Handles the Click event of the btnDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            string controlId = this.GetControlWithFocus().Name;
            if ("txtHours".Equals(controlId))
            {
                this.ChangeHours(false);
            }
            else if ("txtMinutes".Equals(controlId))
            {
                this.ChangeMinutes(false);
            }
            
        }

        /// <summary>
        /// Handles the Click event of the btnUp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            string controlId = this.GetControlWithFocus().Name;
            if ("txtHours".Equals(controlId))
            {
                this.ChangeHours(true);
            }
            else if ("txtMinutes".Equals(controlId))
            {
                this.ChangeMinutes(true);
            }
           
        }


        /// <summary>
        /// Handles the KeyUp event of the txt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            // check for up and down keyboard presses
            if (Key.Up.Equals(e.Key))
            {
                btnUp_Click(this, null);
            }
            else if (Key.Down.Equals(e.Key))
            {
                btnDown_Click(this, null);
            }
        }

        /// <summary>
        /// Handles the MouseWheel event of the txt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseWheelEventArgs"/> instance containing the event data.</param>
        private void txt_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                btnUp_Click(this, null);
            }
            else
            {
                btnDown_Click(this, null);
            }
        }

        /// <summary>
        /// Handles the PreviewKeyUp event of the txt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void txt_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            // make sure all characters are number
            bool allNumbers = textBox.Text.All(Char.IsNumber);
            if (!allNumbers)
            {
                e.Handled = true;
                return;
            }


            // make sure user did not enter values out of range
            int value;
            int.TryParse(textBox.Text, out value);
            if ("txtHours".Equals(textBox.Name))
            {

                if (value > 23 && textBox.Text.Length == 2)
                {
                    textBox.Text = OldHours + "";
                }
                else
                {
                    OldHours = value;
                }
            }
            else if ("txtMinutes".Equals(textBox.Name))
            {
                if(textBox.Text.Length == 2)
                {
                    if (value != 0 && value != 15 && value != 30 && value != 45)
                    {
                        string newText;
                        if (OldMinutes < 10) newText = "0" + OldMinutes;
                        else newText = OldMinutes + "";
                        textBox.Text = newText;
                    }
                    else
                    {
                        OldMinutes = value;
                    }
                }
                
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Changes the hours.
        /// </summary>
        /// <param name="isUp">if set to <c>true</c> [is up].</param>
        private void ChangeHours(bool isUp)
        {
            int value = Convert.ToInt32(this.txtHours.Text);
            if (isUp)
            {
                value += 1;
                if (value == 24)
                {
                    value = 0;
                }
            }
            else
            {
                value -= 1;
                if (value == -1)
                {
                    value = 23;
                }
            }

            OldHours = value;

            string newText;
            if (value < 10) newText = "0" + value;
            else newText = "" + value;

            this.txtHours.Text = newText;
        }

        /// <summary>
        /// Changes the minutes.
        /// </summary>
        /// <param name="isUp">if set to <c>true</c> [is up].</param>
        private void ChangeMinutes(bool isUp)
        {
            int value = Convert.ToInt32(this.txtMinutes.Text);
            if (isUp)
            {
                value += 15;
                if (value == 60)
                {
                    value = 0;
                }
            }
            else
            {
                value -= 15;
                if (value == -15)
                {
                    value = 45;
                }
            }

            OldMinutes = value;

            string textValue = Convert.ToString(value);
            if (value < 10)
            {
                textValue = "0" + Convert.ToString(value);
            }
            this.txtMinutes.Text = textValue;
        }

        /// <summary>
        /// Gets the control with focus.
        /// </summary>
        /// <returns></returns>
        private TextBox GetControlWithFocus()
        {
            TextBox txt = new TextBox();
            if (this.txtHours.IsFocused)
            {
                txt = this.txtHours;
            }
            else if (this.txtMinutes.IsFocused)
            {
                txt = this.txtMinutes;
            }
         
            return txt;
        }

        /// <summary>
        /// Gets the entered value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static string GetEnteredValue(Key key)
        {
            string value = string.Empty;
            switch (key)
            {
                case Key.D0:
                case Key.NumPad0:
                    value = "0";
                    break;
                case Key.D1:
                case Key.NumPad1:
                    value = "1";
                    break;
                case Key.D2:
                case Key.NumPad2:
                    value = "2";
                    break;
                case Key.D3:
                case Key.NumPad3:
                    value = "3";
                    break;
                case Key.D4:
                case Key.NumPad4:
                    value = "4";
                    break;
                case Key.D5:
                case Key.NumPad5:
                    value = "5";
                    break;
                case Key.D6:
                case Key.NumPad6:
                    value = "6";
                    break;
                case Key.D7:
                case Key.NumPad7:
                    value = "7";
                    break;
                case Key.D8:
                case Key.NumPad8:
                    value = "8";
                    break;
                case Key.D9:
                case Key.NumPad9:
                    value = "9";
                    break;
            }
            return value;
        }

        #endregion

        private void HoursOrMinutes_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            
            string newText;
            if ("txtHours".Equals(textBox.Name))
            {
                int value;
                if(int.TryParse(textBox.Text, out value))
                {
                    if (value >= 0 && value < 23)
                    {
                        if (textBox.Text.Length == 1) textBox.Text = "0" + textBox.Text;

                        return;
                    }
                    
                }

                if (OldHours < 10) newText = "0" + OldHours;
                else newText = OldHours + "";
                textBox.Text = newText;
            }
            else if ("txtMinutes".Equals(textBox.Name))
            {
                if (!textBox.Text.Equals("00") && !textBox.Text.Equals("15") && !textBox.Text.Equals("30") && !textBox.Text.Equals("45"))
                {
                    if (OldMinutes < 10) newText = "0" + OldMinutes;
                    else newText = OldMinutes + "";
                    textBox.Text = newText;
                }    
                    
            }

            
        }

        public string getTime()
        {
            return txtHours.Text + ":" + txtMinutes.Text;
        }
    }
}