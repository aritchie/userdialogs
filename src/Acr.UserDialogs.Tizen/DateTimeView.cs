using System;
using Xamarin.Forms;

namespace Acr.UserDialogs
{
	/// <summary>
	/// The DateTime view is used to display the input date and time values.
	/// </summary>
	/// <example>
	/// <code>
	/// var dateView = new DateTimeView
	/// {
	///     HorizontalOptions = LayoutOptions.Center,
	///     VerticalOptions = LayoutOptions.Start,
	///     DateTime = DateTime.Now,
	///     MaximumDate = new DateTime(2030, 12, 31),
	///     MinimumDate = new DateTime(2017, 1, 1),
	///     DisplayFormat = "%F",
	/// }
	/// </code>
	/// </example>
	public class DateTimeView : View
	{
		/// <summary>
		/// BindableProperty. Identifies the DateTime bindable property.
		/// </summary>
		public static readonly BindableProperty DateTimeProperty = BindableProperty.Create(nameof(DateTime), typeof(DateTime), typeof(DateTimeView), DateTime.Now, BindingMode.TwoWay, coerceValue: CoerceDate,
			propertyChanged: DatePropertyChanged);

		/// <summary>
		/// BindableProperty. Identifies the DisplayFormat bindable property.
		/// </summary>
		public static readonly BindableProperty DisplayFormatProperty = BindableProperty.Create(nameof(DisplayFormat), typeof(string), typeof(DateTimeView), "%F");

		/// <summary>
		/// BindableProperty. Identifies the MinimumDate bindable property.
		/// </summary>
		public static readonly BindableProperty MinimumDateProperty = BindableProperty.Create(nameof(MinimumDate), typeof(DateTime), typeof(DateTimeView), new DateTime(1900, 1, 1),
			validateValue: ValidateMinimumDate, coerceValue: CoerceMinimumDate);

		/// <summary>
		/// BindableProperty. Identifies the MaximumDate bindable property.
		/// </summary>
		public static readonly BindableProperty MaximumDateProperty = BindableProperty.Create(nameof(MaximumDate), typeof(DateTime), typeof(DateTimeView), new DateTime(2100, 12, 31),
			validateValue: ValidateMaximumDate, coerceValue: CoerceMaximumDate);

		/// <summary>
		///  Gets or sets the current value of a DateTime.
		/// </summary>
		public DateTime DateTime
		{
			get { return (DateTime)GetValue(DateTimeProperty); }
			set { SetValue(DateTimeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the DateTime format.
		/// Format is a combination of allowed LIBC date format specifiers like: "%b %d, %Y %I : %M %p".
		/// </summary>
		/// <remarks>
		/// These specifiers can be arranged in any order and the widget displays the fields accordingly.
		/// However, you cannot use date and time settings in combination.
		/// The default format is taken as per the system locale settings.
		/// </remarks>
		/// <remarks>
		/// The maximum allowed format length is 64 chars.<br>
		/// The format can include separators for each individual DateTime field except for the AM/PM field.<br>
		/// Each separator can be a maximum of 6 UTF-8 bytes. Space is also taken as a separator.<br>
		/// Following are the allowed set of format specifiers for each DateTime field.<br>
		/// %Y : The year as a decimal number including the century.<br>
		/// %m : The month as a decimal number (range 01 to 12).<br>
		/// %b : The abbreviated month name according to the current locale.<br>
		/// %B : The full month name according to the current locale.<br>
		/// %h : The abbreviated month name according to the current locale(same as %b).<br>
		/// %d : The day of the month as a decimal number(range 01 to 31).<br>
		/// %e : The day of the month as a decimal number(range 1 to 31). Single digits are preceded by a blank.<br>
		/// %I : The hour as a decimal number using a 12-hour clock(range 01 to 12).<br>
		/// %H : The hour as a decimal number using a 24-hour clock(range 00 to 23).<br>
		/// %k : The hour(24-hour clock) as a decimal number(range 0 to 23). Single digits are preceded by a blank.<br>
		/// %l : The hour(12-hour clock) as a decimal number(range 1 to 12). Single digits are preceded by a blank.<br>
		/// %M : The minute as a decimal number(range 00 to 59).<br>
		/// %p : Either 'AM' or 'PM' according to the given time value, or the corresponding strings for the current locale. Noon is treated as 'PM' and midnight as 'AM'.<br>
		/// %P : Like %p, but in lower case: 'am' or 'pm' or a corresponding string for the current locale.<br>
		/// %c : The preferred date and time representation for the current locale.<br>
		/// %x : The preferred date representation for the current locale without the time.<br>
		/// %X : The preferred time representation for the current locale without the date.<br>
		/// %r : The complete calendar time using the AM/PM format of the current locale.<br>
		/// %R : The hour and minute in decimal numbers using the format H:M.<br>
		/// %T : The time of the day in decimal numbers using the format H:M:S.<br>
		/// %F : The date using the format %Y-%m-%d.<br>
		/// </remarks>
		public string DisplayFormat
		{
			get { return (string)GetValue(DisplayFormatProperty); }
			set { SetValue(DisplayFormatProperty, value); }
		}

		/// <summary>
		/// Gets or sets the upper boundary of a DateTime value.
		/// </summary>
		public DateTime MaximumDate
		{
			get { return (DateTime)GetValue(MaximumDateProperty); }
			set { SetValue(MaximumDateProperty, value); }
		}

		/// <summary>
		/// Gets or sets the lower boundary of a DateTime value.
		/// </summary>
		public DateTime MinimumDate
		{
			get { return (DateTime)GetValue(MinimumDateProperty); }
			set { SetValue(MinimumDateProperty, value); }
		}

		/// <summary>
		/// An event fired when the DateTime property changes.
		/// </summary>
		public event EventHandler<DateChangedEventArgs> DateChanged;

		static object CoerceDate(BindableObject bindable, object value)
		{
			var picker = (DateTimeView)bindable;
			DateTime dateValue = (DateTime)value;

			if (dateValue > picker.MaximumDate)
				dateValue = picker.MaximumDate;

			if (dateValue < picker.MinimumDate)
				dateValue = picker.MinimumDate;

			return dateValue;
		}

		static object CoerceMaximumDate(BindableObject bindable, object value)
		{
			DateTime dateValue = ((DateTime)value).Date;
			var selector = (DateTimeView)bindable;
			if (selector.DateTime > dateValue)
				selector.DateTime = dateValue;

			return dateValue;
		}

		static object CoerceMinimumDate(BindableObject bindable, object value)
		{
			DateTime dateValue = ((DateTime)value).Date;
			var selector = (DateTimeView)bindable;
			if (selector.DateTime < dateValue)
				selector.DateTime = dateValue;

			return dateValue;
		}

		static void DatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var selector = (DateTimeView)bindable;
			EventHandler<DateChangedEventArgs> selected = selector.DateChanged;

			if (selected != null)
				selected(selector, new DateChangedEventArgs((DateTime)oldValue, (DateTime)newValue));
		}

		static bool ValidateMaximumDate(BindableObject bindable, object value)
		{
			return (DateTime)value >= ((DateTimeView)bindable).MinimumDate;
		}

		static bool ValidateMinimumDate(BindableObject bindable, object value)
		{
			return (DateTime)value <= ((DateTimeView)bindable).MaximumDate;
		}
	}
}