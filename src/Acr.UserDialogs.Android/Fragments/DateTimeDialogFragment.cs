using System;
using Acr.UserDialogs.Builders;
using Android.App;


namespace Acr.UserDialogs.Fragments
{
    public class DateTimeDialogFragment : AbstractDialogFragment<DateTimePromptConfig>
    {


        protected override Dialog CreateDialog(DateTimePromptConfig config)
        {
            // TODO: set buttons
            switch (config.Mode)
            {
                case DateTimePromptMode.Date:
                    return DateTimeBuilder.BuildDatePicker(this.Activity, config);

                case DateTimePromptMode.Time:
                    return DateTimeBuilder.BuildTimePicker(this.Activity, config);

                default:
                    throw new ArgumentException("Invalid Date/Time Prompt Mode");
            }
        }
    }


    public class DateTimeAppCompatDialogFragment : AbstractAppCompatDialogFragment<DateTimePromptConfig>
    {
        protected override Dialog CreateDialog(DateTimePromptConfig config)
        {
            switch (config.Mode)
            {
                case DateTimePromptMode.Date:
                    return DateTimeBuilder.BuildDatePicker(this.Activity, config);

                case DateTimePromptMode.Time:
                    return DateTimeBuilder.BuildTimePicker(this.Activity, config);

                default:
                    throw new ArgumentException("Invalid Date/Time Prompt Mode");
            }
        }
    }
}