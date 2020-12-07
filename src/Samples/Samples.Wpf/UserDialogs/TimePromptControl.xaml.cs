using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Acr.UserDialogs
{
    // Inspired by http://jobijoy.blogspot.com/2007/10/time-picker-user-control.html
    /// <summary>
    /// Interaction logic for XAML
    /// </summary>
    public partial class TimePromptControl : ContentControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(TimeSpan), typeof(TimePromptControl),
            new UIPropertyMetadata(DateTime.Now.TimeOfDay, new PropertyChangedCallback(OnValueChanged)));
        public static readonly DependencyProperty Use24HourClockProperty = DependencyProperty.Register(nameof(Use24HourClock), typeof(bool), typeof(TimePromptControl));

        public static readonly DependencyProperty HoursProperty = DependencyProperty.Register(nameof(Hours), typeof(int), typeof(TimePromptControl),
            new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));
        public static readonly DependencyProperty MinutesProperty = DependencyProperty.Register(nameof(Minutes), typeof(int), typeof(TimePromptControl),
            new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));
        public static readonly DependencyProperty DesignatorIndexProperty = DependencyProperty.Register(nameof(DesignatorIndex), typeof(int), typeof(TimePromptControl),
            new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        int MaxHours => Use24HourClock ? 23 : DesignatorIndex == 0 ? 12 : 11;
        int MinHours => Use24HourClock ? 0 : DesignatorIndex == 0 ? 0 : 1;

        public TimePromptControl()
        {
            InitializeComponent();
            Designator.ItemsSource = new string[] {
                System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AMDesignator,
                System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.PMDesignator
            };
        }

        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public bool Use24HourClock
        {
            get { return (bool)GetValue(Use24HourClockProperty); }
            set { SetValue(Use24HourClockProperty, value); }
        }

        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }

        public int DesignatorIndex
        {
            get { return (int)GetValue(DesignatorIndexProperty); }
            set { SetValue(DesignatorIndexProperty, value); }
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePromptControl control = obj as TimePromptControl;
            control.Hours = control.Use24HourClock || ((TimeSpan)e.NewValue).Hours <= 12 ? ((TimeSpan)e.NewValue).Hours : ((TimeSpan)e.NewValue).Hours - 12;
            control.Minutes = ((TimeSpan)e.NewValue).Minutes;
            control.DesignatorIndex = ((TimeSpan)e.NewValue).Hours > 12 ? 1 : 0;
        }

        private static void OnTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePromptControl control = obj as TimePromptControl;
            control.Hours = Math.Min(Math.Max(control.MinHours, control.Hours), control.MaxHours);
            control.Minutes = Math.Min(Math.Max(0, control.Minutes), 59);
            control.Value = new TimeSpan(control.Use24HourClock || control.DesignatorIndex == 0 ? control.Hours : control.Hours + 12, control.Minutes, 0);
        }

        private void IncHours()
        {
            if (Hours < MaxHours)
                Hours++;
            else
                Hours = MinHours;
        }

        private void DecHours()
        {
            if (Hours > MinHours)
                Hours--;
            else
                Hours = MaxHours;
        }

        private void IncMinutes()
        {
            if (Minutes < 59)
                Minutes++;
            else
                Minutes = 0;
        }

        private void DecMinutes()
        {
            if (Minutes > 0)
                Minutes--;
            else
                Minutes = 59;
        }

        private void Down(object sender, KeyEventArgs args)
        {
        }

        private void Hour_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                IncHours();
            if (e.Key == Key.Down)
                DecHours();
        }

        private void Hour_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                IncHours();
            else
                DecHours();
        }

        private void Hour_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Int32.TryParse(e.Text, out var result))
            {
                e.Handled = result < 0 || result > MaxHours;
            }
            else
                e.Handled = true;
        }

        private void Minute_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                IncMinutes();
            if (e.Key == Key.Down)
                DecMinutes();
        }

        private void Minute_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                IncMinutes();
            else
                DecMinutes();
        }

        private void Minute_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Int32.TryParse(e.Text, out var result))
            {
                e.Handled = result < 0 || result > 59;
            }
            else
                e.Handled = true;
        }

        private void HourUp_Click(object sender, RoutedEventArgs e)
        {
            IncHours();
        }

        private void HourDn_Click(object sender, RoutedEventArgs e)
        {
            DecHours();
        }

        private void MinuteUp_Click(object sender, RoutedEventArgs e)
        {
            IncMinutes();
        }

        private void MinuteDn_Click(object sender, RoutedEventArgs e)
        {
            DecMinutes();
        }
    }
}
