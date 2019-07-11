using System.ComponentModel;
using System.Linq;
using Xamarin.Forms.Platform.Tizen;
using EDateTimeSelector = ElmSharp.DateTimeSelector;
using TForms = Xamarin.Forms.Platform.Tizen.Forms;

[assembly: ExportRenderer(typeof(Acr.UserDialogs.DateTimeView), typeof(Acr.UserDialogs.DateTimeViewRenderer))]

namespace Acr.UserDialogs
{
	public class DateTimeViewRenderer : ViewRenderer<DateTimeView, EDateTimeSelector>
	{
		static readonly string DateStyle = "date_layout";
		static readonly string TimeStyle = "time_layout";

		static readonly string[] UseDateFormat = { "%Y", "%m", "%b", "%B", "%h", "%d", "%e", "%c", "%x", "%F" };
		static readonly string[] UseTimeFormat = { "%I", "%H", "%k", "%l", "%M", "%p", "%P", "%X", "%r", "%R", "%T" };

		int _changedCallbackDepth = 0;

		protected override void OnElementChanged(ElementChangedEventArgs<DateTimeView> e)
		{
			if (Control == null)
			{
				var selector = new EDateTimeSelector(TForms.Context.MainWindow);
				SetNativeControl(selector);
			}

			if (e.OldElement != null)
			{
				Control.DateTimeChanged -= DateTimeChangedHandler;
			}

			if (e.NewElement != null)
			{
				Control.DateTimeChanged += DateTimeChangedHandler;
			}

			UpdateMaximumDate();
			UpdateMinimumDate();
			UpdateDate();
			UpdateDisplayFormat();

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == DateTimeView.DateTimeProperty.PropertyName)
			{
				UpdateDate();
			}
			else if (e.PropertyName == DateTimeView.MinimumDateProperty.PropertyName)
			{
				UpdateMinimumDate();
			}
			else if (e.PropertyName == DateTimeView.MaximumDateProperty.PropertyName)
			{
				UpdateMaximumDate();
			}
			else if (e.PropertyName == DateTimeView.DisplayFormatProperty.PropertyName)
			{
				UpdateDisplayFormat();
			}

			base.OnElementPropertyChanged(sender, e);
		}

		void DateTimeChangedHandler(object sender, ElmSharp.DateChangedEventArgs e)
		{
			_changedCallbackDepth++;
			Element.DateTime = e.NewDate;
			_changedCallbackDepth--;
		}

		void UpdateDisplayFormat()
		{
			string targetStyle = DateStyle;
			string targetFormat = Element.DisplayFormat;

			bool isTimeFormat = UseDateFormat.Count(b => targetFormat.Contains(b)) == 0;
			bool isDateFormat = UseTimeFormat.Count(b => targetFormat.Contains(b)) == 0;

			if (isTimeFormat && isDateFormat == false)
				targetStyle = TimeStyle;
			else if (isDateFormat && isTimeFormat == false)
				targetStyle = DateStyle;

			Control.Style = targetStyle;
			Control.Format = targetFormat;
		}

		void UpdateMinimumDate()
		{
			Control.MinimumDateTime = Element.MinimumDate;
		}

		void UpdateMaximumDate()
		{
			Control.MaximumDateTime = Element.MaximumDate;
		}

		void UpdateDate()
		{
			if (_changedCallbackDepth == 0)
			{
				Control.DateTime = Element.DateTime;
			}
		}
	}
}