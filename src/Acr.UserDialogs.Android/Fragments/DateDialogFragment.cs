using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;


namespace Acr.UserDialogs.Fragments
{
    public class DateDialogFragment : AbstractDialogFragment<DatePromptConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnResult(new DatePromptResult(false, DateTime.MinValue));
            base.OnKeyPress(sender, args);
        }


        protected override Dialog CreateDialog(DatePromptConfig config)
        {
            return DatePromptBuilder.Build(this.Activity, config);
        }
    }


    public class DateAppCompatDialogFragment : AbstractAppCompatDialogFragment<DatePromptConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnResult(new DatePromptResult(false, DateTime.MinValue));
            base.OnKeyPress(sender, args);
        }


        protected override Dialog CreateDialog(DatePromptConfig config)
        {
            return DatePromptBuilder.Build(this.Activity, config);
        }
    }
}