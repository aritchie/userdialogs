using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;


namespace Acr.UserDialogs.Fragments
{
    public class TimeDialogFragment : AbstractDialogFragment<TimePromptConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnResult(new TimePromptResult(false, TimeSpan.MinValue));
            base.OnKeyPress(sender, args);
        }


        protected override Dialog CreateDialog(TimePromptConfig config)
        {
            return TimePromptBuilder.Build(this.Activity, config);
        }
    }


    public class TimeAppCompatDialogFragment : AbstractAppCompatDialogFragment<TimePromptConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnResult(new TimePromptResult(false, TimeSpan.MinValue));
            base.OnKeyPress(sender, args);
        }


        protected override Dialog CreateDialog(TimePromptConfig config)
        {
            return TimePromptBuilder.Build(this.Activity, config);
        }
    }
}