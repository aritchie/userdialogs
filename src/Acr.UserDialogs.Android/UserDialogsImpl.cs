using System;
using Android.App;


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {

        public UserDialogsImpl(Activity activity) {
            var app = Android.App.Application.Context.ApplicationContext as Android.App.Application;
            if (app == null)
                throw new Exception("Application Context is not an application");

            ActivityMonitor.CurrentTopActivity = activity;
            app.RegisterActivityLifecycleCallbacks(new ActivityMonitor());
        }


        public override void Alert(AlertConfig config) {
            throw new NotImplementedException();
        }


        public override void ActionSheet(ActionSheetConfig config) {
            throw new NotImplementedException();
        }


        public override void Confirm(ConfirmConfig config) {
            throw new NotImplementedException();
        }


        public override void Login(LoginConfig config) {
            throw new NotImplementedException();
        }


        public override void Prompt(PromptConfig config) {
            throw new NotImplementedException();
        }


        public override void Toast(string message, int timeoutSeconds = 3, Action onClick = null) {
            throw new NotImplementedException();
        }


        protected override IProgressDialog CreateDialogInstance() {
            throw new NotImplementedException();
        }
    }
}
//        public static void RequestMainThread(Action action) {
//            if (Application.SynchronizationContext == SynchronizationContext.Current)
//                action();
//            else
//                Application.SynchronizationContext.Post(x => MaskException(action), null);
//        }


//using System;
//using System.Linq;
//using Android.App;
//using Android.Text;
//using Android.Text.Method;
//using Android.Views;
//using Android.Widget;
//using AndroidHUD;


//namespace Acr.MvvmCross.Plugins.UserDialogs.Droid {
    
//    public class DroidUserDialogService : AbstractUserDialogService {

//        public override void Alert(AlertConfig config) {
//            Utils.RequestMainThread(() => 
//                new AlertDialog
//                    .Builder(Utils.GetActivityContext())
//                    .SetMessage(config.Message)
//                    .SetTitle(config.Title)
//                    .SetPositiveButton(config.OkText, (o, e) => {
//                        if (config.OnOk != null) 
//                            config.OnOk();
//                    })
//                    .Show()
//            );
//        }


//        public override void ActionSheet(ActionSheetConfig config) {
//            var array = config
//                .Options
//                .Select(x => x.Text)
//                .ToArray();

//            Utils.RequestMainThread(() => 
//                new AlertDialog
//                    .Builder(Utils.GetActivityContext())
//                    .SetTitle(config.Title)
//                    .SetItems(array, (sender, args) => config.Options[args.Which].Action())
//                    .Show()
//            );
//        }


//        public override void Confirm(ConfirmConfig config) {
//            Utils.RequestMainThread(() => 
//                new AlertDialog
//                    .Builder(Utils.GetActivityContext())
//                    .SetMessage(config.Message)
//                    .SetTitle(config.Title)
//                    .SetPositiveButton(config.OkText, (o, e) => config.OnConfirm(true))
//                    .SetNegativeButton(config.CancelText, (o, e) => config.OnConfirm(false))
//                    .Show()
//            );
//        }


//        public override void Login(LoginConfig config) {
//            var context = Utils.GetActivityContext();
//            var txtUser = new EditText(context) {
//                Hint = config.LoginPlaceholder,
//                Text = config.LoginValue ?? String.Empty
//            };

//            var txtPass = new EditText(context) {
//                Hint = config.PasswordPlaceholder ?? "*",
//                InputType = InputTypes.ClassText | InputTypes.TextVariationPassword,
//                TransformationMethod = PasswordTransformationMethod.Instance
//            };
//            var layout = new LinearLayout(context) {
//                Orientation = Orientation.Vertical
//            };

//            txtUser.SetMaxLines(1);
//            txtPass.SetMaxLines(1);

//            layout.AddView(txtUser, ViewGroup.LayoutParams.MatchParent);
//            layout.AddView(txtPass, ViewGroup.LayoutParams.MatchParent);

//            Utils.RequestMainThread(() => 
//                new AlertDialog
//                    .Builder(Utils.GetActivityContext())
//                    .SetTitle(config.Title)
//                    .SetMessage(config.Message)
//                    .SetView(layout)
//                    .SetPositiveButton(config.OkText, (o, e) =>
//                        config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, true))
//                    )
//                    .SetNegativeButton(config.CancelText, (o, e) =>
//                        config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, false))
//                    )
//                    .Show()
//            );
//        }


//        public override void Prompt(PromptConfig config) {
//            Utils.RequestMainThread(() => {
//                var txt = new EditText(Utils.GetActivityContext()) {
//                    Hint = config.Placeholder
//                };

//                if (config.InputType != InputType.Default) 
//                    txt.SetMaxLines(1);

//                SetInputType(txt, config.InputType);

//                new AlertDialog
//                    .Builder(Utils.GetActivityContext())
//                    .SetMessage(config.Message)
//                    .SetTitle(config.Title)
//                    .SetView(txt)
//                    .SetPositiveButton(config.OkText, (o, e) =>
//                        config.OnResult(new PromptResult {
//                            Ok = true, 
//                            Text = txt.Text
//                        })
//                    )
//                    .SetNegativeButton(config.CancelText, (o, e) => 
//                        config.OnResult(new PromptResult {
//                            Ok = false, 
//                            Text = txt.Text
//                        })
//                    )
//                    .Show();
//            });
//        }

//        private static void SetInputType(TextView txt, InputType inputType) {
//            switch (inputType) {
//                case InputType.Email:
//                    txt.InputType = InputTypes.TextVariationEmailAddress;
//                    break;
//                case InputType.Number:
//                    txt.InputType = InputTypes.ClassNumber;
//                    break;
//                case InputType.Password:
//                    txt.TransformationMethod = PasswordTransformationMethod.Instance;
//                    txt.InputType = InputTypes.TextVariationPassword;
//                    break;
//            }
//        }


//        public override void Toast(string message, int timeoutSeconds, Action onClick) {
//            Utils.RequestMainThread(() => {
//                onClick = onClick ?? (() => {});

//                AndHUD.Shared.ShowToast(
//                    Utils.GetActivityContext(), 
//                    message, 
//                    MaskType.Clear,
//                    TimeSpan.FromSeconds(timeoutSeconds),
//                    false,
//                    onClick
//                );
//            });
//        }


//        protected override IProgressDialog CreateDialogInstance() {
//            return new ProgressDialog();
//        }
//    }
//}