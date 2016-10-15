using System;
using System.Drawing;


namespace Acr.UserDialogs
{
    public class ToastConfig
    {
        public static TimeSpan DefaultDuration { get; set; } = TimeSpan.FromSeconds(2.5);
        public static Color? DefaultMessageTextColor { get; set; }
        public static Color? DefaultActionTextColor { get; set; }
        public static Color? DefaultBackgroundColor { get; set; }

        public string Message { get; set; }
        public Color? MessageTextColor { get; set; } = DefaultMessageTextColor;
        public Color? BackgroundColor { get; set; } = DefaultBackgroundColor;
        public TimeSpan Duration { get; set; } = DefaultDuration;
        public ToastAction Action { get; set; }


        public ToastConfig(string message)
        {
            this.Message = message;
        }


        public ToastConfig SetBackgroundColor (Color color)
        {
            this.BackgroundColor = color;
            return this;
        }


        public ToastConfig SetDuration(int millis)
        {
            return this.SetDuration(TimeSpan.FromMilliseconds(millis));
        }


        public ToastConfig SetDuration(TimeSpan? duration = null)
        {
            this.Duration = duration ?? DefaultDuration;
            return this;
        }


        public ToastConfig SetAction(Action<ToastAction> action)
        {
            var cfg = new ToastAction();
            action(cfg);
            return this.SetAction (cfg);
        }


        public ToastConfig SetAction(ToastAction action)
        {
            this.Action = action;
            if (action.TextColor == null)
                action.TextColor = DefaultActionTextColor;

            return this;
        }


        public ToastConfig SetMessageTextColor (Color color)
        {
            this.MessageTextColor = color;
            return this;
        }
    }
}
