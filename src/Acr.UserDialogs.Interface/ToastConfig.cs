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
        public ToastAction PrimaryAction { get; set; }
        public ToastAction SecondaryAction { get; set; }


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


        public ToastConfig SetPrimaryAction(ToastAction action)
        {
            this.PrimaryAction = action;
            return this;
        }


        public ToastConfig SetPrimaryAction(Action<ToastAction> visitor)
        {
            var action = new ToastAction();
            visitor(action);
            return this.SetPrimaryAction(action);
        }


        public ToastConfig SetSecondaryAction(ToastAction action)
        {
            this.SecondaryAction = action;
            return this;
        }


        public ToastConfig SetSecondaryAction(Action<ToastAction> visitor)
        {
            var action = new ToastAction();
            visitor(action);
            return this.SetSecondaryAction(action);
        }
    }
}
