using System;
using Foundation;

namespace AI
{
	//https://forums.xamarin.com/discussion/27184/convert-nsdate-to-datetime
	public static class DateTimeExtensions
	{
		public static DateTime ToDateTime(this NSDate nsDate)
		{
			if (nsDate == null) return new DateTime(); // ?

			NSCalendar cal = NSCalendar.CurrentCalendar;
			int year = (int)cal.GetComponentFromDate(NSCalendarUnit.Year, nsDate);
			int month = (int)cal.GetComponentFromDate(NSCalendarUnit.Month, nsDate);
			int day = (int)cal.GetComponentFromDate(NSCalendarUnit.Day, nsDate);
			int hour = (int)cal.GetComponentFromDate(NSCalendarUnit.Hour, nsDate);
			int minute = (int)cal.GetComponentFromDate(NSCalendarUnit.Minute, nsDate);
			int second = (int)cal.GetComponentFromDate(NSCalendarUnit.Second, nsDate);
			int nanosecond = (int)cal.GetComponentFromDate(NSCalendarUnit.Nanosecond, nsDate);

			int millisecond = (nanosecond / 1000000);

			return new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Local);
		}

		public static NSDate ToNSDate(this DateTime dt)
		{
			if (dt == DateTime.MinValue) return null; // ?

			DateTime ldt = dt.ToLocalTime();
			NSDateComponents components = new NSDateComponents();
			components.Year = ldt.Year;
			components.Month = ldt.Month;
			components.Day = ldt.Day;
			components.Hour = ldt.Hour;
			components.Minute = ldt.Minute;
			components.Second = ldt.Second;
			components.Nanosecond = (ldt.Millisecond * 1000000);

			return NSCalendar.CurrentCalendar.DateFromComponents(components);
		}
	}
}

