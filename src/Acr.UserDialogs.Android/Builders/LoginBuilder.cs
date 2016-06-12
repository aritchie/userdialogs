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
    public class LoginBuilder : AbstractAlertDialogBuilder<LoginConfig>
    {
        public override AlertDialog.Builder Build(Activity activity, LoginConfig config)
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

            return this
                .CreateBaseBuilder(activity, config.AndroidStyleId)
                .SetCancelable(false)
                .SetTitle(config.Title)
                .SetMessage(config.Message)
                .SetView(layout)
                .SetPositiveButton(config.OkText, (s, a) =>
                    config.OnResult(new LoginResult(true, txtUser.Text, txtPass.Text))
                )
                .SetNegativeButton(config.CancelText, (s, a) =>
                    config.OnResult(new LoginResult(false, txtUser.Text, txtPass.Text))
                );
        }


        public override AppCompatAlertDialog.Builder Build(AppCompatActivity activity, LoginConfig config)
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

            return this
                .CreateBaseBuilder(activity, config.AndroidStyleId)
                .SetCancelable(false)
                .SetTitle(config.Title)
                .SetMessage(config.Message)
                .SetView(layout)
                .SetPositiveButton(config.OkText, (s, a) =>
                    config.OnResult(new LoginResult(true, txtUser.Text, txtPass.Text))
                )
                .SetNegativeButton(config.CancelText, (s, a) =>
                    config.OnResult(new LoginResult(false, txtUser.Text, txtPass.Text))
                );
        }
    }
}