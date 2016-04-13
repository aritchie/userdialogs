using System;
using Android.App;


namespace Acr.UserDialogs.Fragments
{
    public class DateTimeDialogFragment : AbstractDialogFragment<DateTimePromptConfig>
    {
        protected override Dialog CreateDialog(DateTimePromptConfig config)
        {
            var dialog = new DateTimeDialog(this.Activity);
            dialog.Init(config);
            return dialog;
        }
    }


    public class DateTimeAppCompatDialogFragment : AbstractAppCompatDialogFragment<DateTimePromptConfig>
    {
        protected override Dialog CreateDialog(DateTimePromptConfig config)
        {
            var dialog = new DateTimeDialog(this.Activity);
            dialog.Init(config);
            return dialog;
        }
    }
}
/*
  public override void DateTimePrompt(DateTimePromptConfig config)
        {
            switch (config.Mode)
            {
                case DateTimePromptMode.Date:
                    this.DatePrompt(config);
                    break;

                case DateTimePromptMode.Time:
                    this.TimePrompt(config);
                    break;

                case DateTimePromptMode.DateAndTime:
                    break;
            }

        }


        protected virtual void DatePrompt(DateTimePromptConfig config)
        {
            var dateTime = config.SelectedDateTime ?? DateTime.Now;
            var dialog = new DatePickerDialog(
                this.GetTopActivity(),
                (sender, args) => dateTime = args.Date,
                dateTime.Year,
                dateTime.Month + 1,
                dateTime.Day
            );
            if (!String.IsNullOrWhiteSpace(config.Title))
                dialog.SetTitle(config.Title);

            //if (config.MinimumDate != null)
            //    dialog.DatePicker.MinDate = 0; // from epoch time

            //if (config.MaximumDate != null)
            //    dialog.DatePicker.MaxDate = 0; // from epoch time

            dialog.DismissEvent += (sender, args) => config.OnResult?.Invoke(new DateTimePromptResult(true, dateTime));
            dialog.CancelEvent += (sender, args) => config.OnResult?.Invoke(new DateTimePromptResult(false, dateTime));
            dialog.Show();
        }


        protected virtual void TimePrompt(DateTimePromptConfig config)
        {
            var dateTime = config.SelectedDateTime ?? DateTime.Now;
            var dialog = new TimePickerDialog(
                this.GetTopActivity(),
                (sender, args) => dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, args.HourOfDay, args.Minute, 0),
                dateTime.Hour,
                dateTime.Minute,
                false // TODO: 24h could be a configurable property
            );
            if (!String.IsNullOrWhiteSpace(config.Title))
                dialog.SetTitle(config.Title);

            dialog.DismissEvent += (sender, args) => config.OnResult?.Invoke(new DateTimePromptResult(true, dateTime));
            dialog.CancelEvent += (sender, args) => config.OnResult?.Invoke(new DateTimePromptResult(false, dateTime));
            dialog.Show();
        }
     */