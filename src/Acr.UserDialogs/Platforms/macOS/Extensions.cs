using System;
using AppKit;
using Foundation;

namespace Acr.UserDialogs
{
    public static class Extensions
    {
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

        public static void ActivateConstraints(params NSLayoutConstraint[] constraints)
        {
            foreach (var item in constraints)
                item.Active = true;
        }
    }
}