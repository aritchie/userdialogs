using System;
using System.Drawing;
using Splat;


namespace Acr.UserDialogs {

    public enum ToastEvent {
        Info,
        Warning,
        Error,
        Success
    }


    public class ToastConfig {

        public Color? BackgroundColor { get; set; }
        public IBitmap Logo { get; set; }
        public string Message { get; set; }
        public TimeSpan Duration { get; set; }
        public Action OnTap { get; set; }
        //public Color? ActionColor { get; set; }
        // top or bottom? snackbar can't do this


        /// <summary>
        /// If you have set a custom background color and/or image, they will be overwritten by this method!
        /// </summary>
        /// <returns></returns>
        public ToastConfig SetEvent() {
            return this;
        }


        public ToastConfig SetDuration(int millis) {
            return this;
        }
    }
}
