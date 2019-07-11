using System;
using BigTed;
using Foundation;
using UIKit;
using Acr.UserDialogs.Infrastructure;


namespace Acr.UserDialogs
{
    public static class Extensions
    {
        public static UIColor ToNative(this System.Drawing.Color This)
            => new UIColor((float)This.R / 255.0f, (float)This.G / 255.0f, This.B / 255.0f, This.A / 255.0f);


        public static void SafeInvokeOnMainThread(this UIApplication app, Action action) => app.InvokeOnMainThread(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Log.Error("", ex.ToString());
            }
        });


        public static ProgressHUD.MaskType ToNative(this MaskType maskType)
        {
            switch (maskType)
            {
                case MaskType.Black: return ProgressHUD.MaskType.Black;
                case MaskType.Clear: return ProgressHUD.MaskType.Clear;
                case MaskType.Gradient: return ProgressHUD.MaskType.Gradient;
                case MaskType.None: return ProgressHUD.MaskType.None;
                default:
                    throw new ArgumentException("Invalid mask type");
            }
        }


		public static DateTime ToDateTime(this NSDate nsDate)
		{
			if (nsDate == null)
                return new DateTime();

			var cal = NSCalendar.CurrentCalendar;
			var year = (int)cal.GetComponentFromDate(NSCalendarUnit.Year, nsDate);
			var month = (int)cal.GetComponentFromDate(NSCalendarUnit.Month, nsDate);
			var day = (int)cal.GetComponentFromDate(NSCalendarUnit.Day, nsDate);
			var hour = (int)cal.GetComponentFromDate(NSCalendarUnit.Hour, nsDate);
			var minute = (int)cal.GetComponentFromDate(NSCalendarUnit.Minute, nsDate);
			var second = (int)cal.GetComponentFromDate(NSCalendarUnit.Second, nsDate);
			var nanosecond = (int)cal.GetComponentFromDate(NSCalendarUnit.Nanosecond, nsDate);
            var millisecond = (nanosecond / 1000000);

			return new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Local);
		}


		public static NSDate ToNSDate(this DateTime dt)
		{
			if (dt == DateTime.MinValue)
                return null;

			var ldt = dt.ToLocalTime();
			var components = new NSDateComponents
			{
			    Year = ldt.Year,
                Month = ldt.Month,
                Day = ldt.Day,
                Hour = ldt.Hour,
                Minute = ldt.Minute,
                Second = ldt.Second,
                Nanosecond = (ldt.Millisecond * 1000000)
			};
			return NSCalendar.CurrentCalendar.DateFromComponents(components);
		}
    }
}