using System;
using Acr.UserDialogs.Builders;
using Android.App;


namespace Acr.UserDialogs.Fragments
{
    public abstract class AbstractBuilderDialogFragment<TConfig, TBuilder> : AbstractDialogFragment<TConfig>
        where TConfig : class
        where TBuilder : IAlertDialogBuilder<TConfig>, new()
    {
        protected override Dialog CreateDialog(TConfig config)
        {
            return new TBuilder()
                .Build(this.Activity, this.Config)
                .Create();
        }
    }


    public abstract class AbstractBuilderAppCompatDialogFragment<TConfig, TBuilder> : AbstractAppCompatDialogFragment<TConfig>
        where TConfig : class
        where TBuilder : IAlertDialogBuilder<TConfig>, new()
    {
        protected override Dialog CreateDialog(TConfig config)
        {
            return new TBuilder()
                .Build(this.AppCompatActivity, this.Config)
                .Create();
        }
    }
}