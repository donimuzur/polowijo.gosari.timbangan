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

namespace polowijo.gosari.timbangan.UI._OTHERS
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
        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(TimeSpan), typeof(TimePicker),
        new UIPropertyMetadata(DateTime.Now.TimeOfDay, new PropertyChangedCallback(OnValueChanged)));
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = obj as TimePicker;
            control.Hours = ((TimeSpan)e.NewValue).Hours;
            control.Minutes = ((TimeSpan)e.NewValue).Minutes;
            control.Seconds = ((TimeSpan)e.NewValue).Seconds;
        }
        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }
        public static readonly DependencyProperty HoursProperty =
        DependencyProperty.Register("Hours", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));
        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        public static readonly DependencyProperty MinutesProperty =
        DependencyProperty.Register("Minutes", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));
        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }
        public static readonly DependencyProperty SecondsProperty =
        DependencyProperty.Register("Seconds", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));
        private static void OnTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = obj as TimePicker;
            control.Value = new TimeSpan(control.Hours, control.Minutes, control.Seconds);
        }
        private void SecondChange(string change)
        {
            var currentTime = new DateTime(2018, 11, 28, this.Hours, this.Minutes, this.Seconds);
            if (change == "up")
            {
                currentTime = currentTime.Add(new TimeSpan(0, 0, 1));
                this.Seconds = currentTime.Second;
                this.Hours = currentTime.Hour;
                this.Minutes = currentTime.Minute;
            }
            else if (change == "down")
            {
                currentTime = currentTime.Subtract(new TimeSpan(0, 0, 1));
                this.Seconds = currentTime.Second;
                this.Hours = currentTime.Hour;
                this.Minutes = currentTime.Minute;
            }
        }
        private void MinuteChange(string change)
        {
            var currentTime = new DateTime(2018, 11, 28, this.Hours, this.Minutes, this.Seconds);
            if (change == "up")
            {
                currentTime = currentTime.Add(new TimeSpan(0, 1, 0));
                this.Seconds = currentTime.Second;
                this.Hours = currentTime.Hour;
                this.Minutes = currentTime.Minute;
            }
            else if (change == "down")
            {
                currentTime = currentTime.Subtract(new TimeSpan(0, 1, 0));
                this.Seconds = currentTime.Second;
                this.Hours = currentTime.Hour;
                this.Minutes = currentTime.Minute;
            }
        }
        private void HourChange(string change)
        {
            var currentTime = new DateTime(2018, 11, 28, this.Hours, this.Minutes, this.Seconds);

            if (change == "up")
            {
                currentTime = currentTime.Add(new TimeSpan(1, 0, 0));
                this.Seconds = currentTime.Second;
                this.Hours = currentTime.Hour;
                this.Minutes = currentTime.Minute;
            }
            else if (change == "down")
            {
                currentTime = currentTime.Subtract(new TimeSpan(1, 0, 0));
                this.Seconds = currentTime.Second;
                this.Hours = currentTime.Hour;
                this.Minutes = currentTime.Minute;
            }
        }
        public static bool IsValidHour(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 0 && i <= 23;
        }
        public static bool IsValidMinute(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 0 && i <= 59;
        }
        private void Down(object sender, KeyEventArgs args)
        {
            switch (((Grid)sender).Name)
            {
                case "sec":
                    if (args.Key == Key.Up)
                        SecondChange("up");
                    if (args.Key == Key.Down)
                        SecondChange("down");
                    break;

                case "min":
                    if (args.Key == Key.Up)
                        MinuteChange("up");
                    if (args.Key == Key.Down)
                        MinuteChange("down");
                    break;

                case "hour":
                    if (args.Key == Key.Up)
                        HourChange("up");
                    if (args.Key == Key.Down)
                        HourChange("down");
                    break;
            }
        }
        private void mmTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidHour(((TextBox)sender).Text + e.Text);
        }
        private void ddTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidMinute(((TextBox)sender).Text + e.Text);
        }
        private void yyTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidMinute(((TextBox)sender).Text + e.Text);
        }

        private void HoursUp_Click(object sender, RoutedEventArgs e)
        {
            HourChange("up");
        }

        private void HoursDown_Click(object sender, RoutedEventArgs e)
        {
            HourChange("down");
        }

        private void SecondUp_Click(object sender, RoutedEventArgs e)
        {
            SecondChange("up");
        }

        private void SecondDown_Click(object sender, RoutedEventArgs e)
        {
            SecondChange("down");
        }

        private void MinuteUp_Click(object sender, RoutedEventArgs e)
        {
            MinuteChange("up");
        }

        private void MinutesDown_Click(object sender, RoutedEventArgs e)
        {
            MinuteChange("down");
        }
    }
}
