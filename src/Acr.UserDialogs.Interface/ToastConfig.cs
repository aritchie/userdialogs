using System;
using System.Drawing;


namespace Acr.UserDialogs
{
    public class ToastConfig
    {
        public static TimeSpan DefaultDuration { get; set; } = TimeSpan.FromSeconds(2.5);
        public static Color DefaultMessageTextColor { get; set; } = Color.White;
        public static Color DefaultPrimaryTextColor { get; set; } = Color.White;
        public static Color DefaultSecondaryTextColor { get; set; } = Color.White;

        public string Message { get; set; }
        public Color MessageTextColor { get; set; } = DefaultMessageTextColor;
        public TimeSpan Duration { get; set; }
        public ToastAction Action { get; set; }


        public ToastConfig(string message)
        {
            this.Message = message;
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
            return this;
        }


        public ToastConfig SetAction(ToastAction action)
        {
            this.Action = action;
            return this;
        }


        public ToastConfig SetMessageTextColor (Color color)
        {
            this.MessageTextColor = color;
            return this;
        }
    }
}
