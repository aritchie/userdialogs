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
    public class LoginBuilder
    {
        public Dialog Build(Activity activity, LoginConfig config)
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

            var builder = new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetTitle(config.Title)
                .SetMessage(config.Message)
                .SetView(layout)
                .SetPositiveButton(config.Positive.Text, (s, a) =>
                    config.OnAction(new DialogResult<Credentials>(DialogChoice.Positive, new Credentials(txtUser.Text, txtPass.Text)))
                );

            if (config.Neutral.IsVisible)
            {
                builder.SetNeutralButton(config.Neutral.Text, (s, a) =>
                    config.OnAction(new DialogResult<Credentials>(DialogChoice.Neutral, new Credentials(txtUser.Text, txtPass.Text)))
                );
            }
            
            if (config.Negative.IsVisible)
            {
                builder.SetNegativeButton(config.Negative.Text, (s, a) =>
                    config.OnAction(new DialogResult<Credentials>(DialogChoice.Negative, new Credentials(txtUser.Text, txtPass.Text)))
                );
            }
                
            return builder.Create();
        }


        public Dialog Build(AppCompatActivity activity, LoginConfig config)
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

            var builder = new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetTitle(config.Title)
                .SetMessage(config.Message)
                .SetView(layout)
                .SetPositiveButton(config.Positive.Text, (s, a) =>
                    config.OnAction(new DialogResult<Credentials>(DialogChoice.Positive, new Credentials(txtUser.Text, txtPass.Text)))
                );

            if (config.Neutral.IsVisible)
            {
                builder.SetNeutralButton(config.Neutral.Text, (s, a) =>
                    config.OnAction(new DialogResult<Credentials>(DialogChoice.Neutral, new Credentials(txtUser.Text, txtPass.Text)))
                );
            }

            if (config.Negative.IsVisible)
            {
                builder.SetNegativeButton(config.Negative.Text, (s, a) =>
                    config.OnAction(new DialogResult<Credentials>(DialogChoice.Negative, new Credentials(txtUser.Text, txtPass.Text)))
                );
            }

            return builder.Create();
        }


        //protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        //{
        //    base.OnKeyPress(sender, args);
        //    if (args.KeyCode != Keycode.Back)
        //    {
        //        args.Handled = true;
        //        return;
        //    }
        //    this.Config?.OnAction(new LoginResult(false, null, null));
        //    this.Dismiss();
        //}
    }
}