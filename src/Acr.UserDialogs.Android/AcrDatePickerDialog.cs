using System;
using Android.App;
using Android.Content;
using Android.Widget;
using String = System.String;

namespace Acr.UserDialogs
{
    public class AcrDatePickerDialog : DatePickerDialog
    {
        private readonly string _title;

        public AcrDatePickerDialog(Context context, int theme, EventHandler<DateSetEventArgs> callBack, int year, int monthOfYear, int dayOfMonth, string title) : base(context, theme, callBack, year, monthOfYear, dayOfMonth)
        {
            _title = title;
        }

        public override void OnDateChanged(DatePicker view, int year, int month, int dayOfMonth)
        {
            if (!String.IsNullOrWhiteSpace(_title))
                SetTitle(_title);
        }
    }
}