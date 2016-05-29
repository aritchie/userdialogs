using System;
using System.Drawing;
using Splat;


namespace Acr.UserDialogs
{

    public enum ToastEvent
    {
        Info,
        Warn,
        Error,
        Success
    }


    public class ToastConfig
    {
        public static TimeSpan DefaultDuration { get; set; } = TimeSpan.FromSeconds(2.5);

        public Color BackgroundColor { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Color TextColor { get; set; }
        public TimeSpan Duration { get; set; }
        public Action Action { get; set; }


        public ToastConfig(string title, string description = null)
        {
            this.Title = title;
            this.Description = description;
            this.Duration = DefaultDuration;
        }


        public ToastConfig SetDescription(string description)
        {
            this.Description = description;
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


        public ToastConfig SetAction(Action action)
        {
            this.Action = action;
            return this;
        }
    }
}
