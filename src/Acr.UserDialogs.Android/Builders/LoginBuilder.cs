using System;
using Android.App;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public static class LoginBuilder
    {
        public static AlertDialog.Builder Build(Activity activity, LoginConfig config)
        {
            var txtUser = new EditText(activity)
            {
                Hint = config.LoginPlaceholder,
                InputType = InputTypes.TextVariationVisiblePassword,
                Text = config.LoginValue ?? String.Empty
            };
            var txtPass = new EditText(activity)
            {
                Hint = config.PasswordPlaceholder ?? "*"
            };
            PromptBuilder.SetInputType(txtPass, InputType.Password);

            var layout = new LinearLayout(activity)
            {
                Orientation = Orientation.Vertical
            };

            txtUser.SetMaxLines(1);
            txtPass.SetMaxLines(1);

            layout.AddView(txtUser, ViewGroup.LayoutParams.MatchParent);
            layout.AddView(txtPass, ViewGroup.LayoutParams.MatchParent);

            return new AlertDialog
                .Builder(activity)
                .SetCancelable(false)
                .SetTitle(config.Title)
                .SetMessage(config.Message)
                .SetView(layout)
                .SetPositiveButton(config.OkText, (s, a) =>
                    config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, true))
                )
                .SetNegativeButton(config.CancelText, (s, a) =>
                    config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, false))
                );
        }


        public static AppCompatAlertDialog.Builder Build(AppCompatActivity activity, LoginConfig config)
        {
            var txtUser = new EditText(activity)
            {
                Hint = config.LoginPlaceholder,
                InputType = InputTypes.TextVariationVisiblePassword,
                Text = config.LoginValue ?? String.Empty
            };
            var txtPass = new EditText(activity)
            {
                Hint = config.PasswordPlaceholder ?? "*"
            };
            PromptBuilder.SetInputType(txtPass, InputType.Password);

            var layout = new LinearLayout(activity)
            {
                Orientation = Orientation.Vertical
            };

            txtUser.SetMaxLines(1);
            txtPass.SetMaxLines(1);

            layout.AddView(txtUser, ViewGroup.LayoutParams.MatchParent);
            layout.AddView(txtPass, ViewGroup.LayoutParams.MatchParent);

            return new AppCompatAlertDialog
                .Builder(activity)
                .SetCancelable(false)
                .SetTitle(config.Title)
                .SetMessage(config.Message)
                .SetView(layout)
                .SetPositiveButton(config.OkText, (s, a) =>
                    config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, true))
                )
                .SetNegativeButton(config.CancelText, (s, a) =>
                    config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, false))
                );
        }
    }
}