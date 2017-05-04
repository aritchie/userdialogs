using System;
using System.Drawing;
using Splat;

namespace Acr.UserDialogs
{
    public class ToastConfig
    {
        /// <summary>
        /// The default duration for how long the toast should remain on-screen.  Defaults to 2.5 seconds
        /// </summary>
        public static TimeSpan DefaultDuration { get; set; } = TimeSpan.FromSeconds(2.5);

        /// <summary>
        /// The default message text color to use.  If not set, defaults very depending on platform.
        /// </summary>
        public static Color? DefaultMessageTextColor { get; set; }

        /// <summary>
        /// The default text color in the action button.  If not set, defaults very depending on platform.
        /// </summary>
        public static Color? DefaultActionTextColor { get; set; }

        /// <summary>
        /// The default toast background color.  If not set, defaults very depending on platform.
        /// </summary>
        public static Color? DefaultBackgroundColor { get; set; }

        public string Message { get; set; }
        public Color? MessageTextColor { get; set; } = DefaultMessageTextColor;
        public Color? BackgroundColor { get; set; } = DefaultBackgroundColor;
        public TimeSpan Duration { get; set; } = DefaultDuration;
        public ToastAction Action { get; set; }
        public IBitmap Icon { get; set; }

        public ToastConfig(string message)
        {
            this.Message = message;
        }


        public ToastConfig SetBackgroundColor(Color color)
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
            return this.SetAction(cfg);
        }


        public ToastConfig SetAction(ToastAction action)
        {
            this.Action = action;
            if (action.TextColor == null)
                action.TextColor = DefaultActionTextColor;

            return this;
        }


        public ToastConfig SetMessageTextColor(Color color)
        {
            this.MessageTextColor = color;
            return this;
        }

        public ToastConfig SetIcon(IBitmap icon)
        {
            this.Icon = icon;
            return this;
        }
    }
}
