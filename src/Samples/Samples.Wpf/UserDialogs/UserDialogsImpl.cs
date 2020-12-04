using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Acr.UserDialogs.Infrastructure;
using Xamarin.Forms.Platform.WPF.Controls;

namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        readonly Func<Action, Task> dispatcher;

        // For not implemented features but not throw exception
        class DummyDisposable : IDisposable 
        {
            void IDisposable.Dispose() { }
        }

        public UserDialogsImpl(Func<Action, Task> dispatcher = null)
        {
            this.dispatcher = dispatcher ?? new Func<Action, Task>(async x => await System.Windows.Application
                .Current
                .Dispatcher
                .InvokeAsync(() => x(), DispatcherPriority.Normal)
            );
        }

        public override IDisposable ActionSheet(ActionSheetConfig config)
        {
            return new DummyDisposable();
        }

        public override IDisposable Alert(AlertConfig config)
        {
            Dispatch(() =>
            {
                FormsContentDialog dialog = new FormsContentDialog()
                {
                    Title = config.Title,
                    Content = config.Message,
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = config.OkText
                };
                dialog.PrimaryButtonClick += (s, e) => { HideContentDialog(); e.Cancel = true; };
                ShowContentDialog(dialog);
            });
            return new DisposableAction(HideContentDialog);
        }

        public override IDisposable Confirm(ConfirmConfig config)
        {
            Dispatch(() =>
            {
                FormsContentDialog dialog = new FormsContentDialog()
                {
                    Title = config.Title,
                    Content = config.Message,
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = config.OkText,
                    IsSecondaryButtonEnabled = true,
                    SecondaryButtonText = config.CancelText
                };
                dialog.PrimaryButtonClick += (s, e) => { HideContentDialog(); config.OnAction(true); e.Cancel = true; };
                dialog.SecondaryButtonClick += (s, e) => { HideContentDialog(); config.OnAction(false); e.Cancel = true; };
                ShowContentDialog(dialog);
            });
            return new DisposableAction(HideContentDialog);
        }

        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            return new DummyDisposable();
        }

        public override IDisposable Login(LoginConfig config)
        {
            return new DummyDisposable();
        }

        public override IDisposable Prompt(PromptConfig config)
        {
            Dispatch(() =>
            {
                var dialog = new FormsContentDialog()
                {
                    DataContext = config,
                    Title = config.Title,
                    // Content will be set
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = config.OkText,
                    IsSecondaryButtonEnabled = config.IsCancellable,
                    SecondaryButtonText = config.CancelText
                };

                if (config.InputType == InputType.Password || config.InputType == InputType.NumericPassword)
                {
                    var control = new PasswordPromptControl();
                    control.PasswordEdit.PasswordChanged += (s, e) =>
                    {
                        config.Text = control.PasswordEdit.Password;
                        if (config.OnTextChanged != null)
                        {
                            var args = new PromptTextChangedArgs() { Value = control.PasswordEdit.Password };
                            config.OnTextChanged(args);
                            dialog.IsPrimaryButtonEnabled = args.IsValid;
                            if (control.PasswordEdit.Password != args.Value)
                            {
                                control.PasswordEdit.Password = args.Value;
                            }
                        }
                    };
                    dialog.Content = control;
                    // First run of text changed
                    if (config.OnTextChanged != null)
                    {
                        var args = new PromptTextChangedArgs() { Value = control.PasswordEdit.Password };
                        config.OnTextChanged(args);
                        dialog.IsPrimaryButtonEnabled = args.IsValid;
                        control.PasswordEdit.Password = args.Value;
                    }

                }
                else
                {
                    var control = new DefaultPromptControl();
                    control.TextEdit.TextChanged += (s, e) =>
                    {
                        if (config.OnTextChanged != null)
                        {
                            var args = new PromptTextChangedArgs() { Value = control.TextEdit.Text };
                            config.OnTextChanged(args);
                            dialog.IsPrimaryButtonEnabled = args.IsValid;
                            if (control.TextEdit.Text != args.Value)
                            {
                                int selStart = control.TextEdit.SelectionStart;
                                control.TextEdit.Text = args.Value;
                                control.TextEdit.SelectionStart = selStart;
                            }
                        }
                    };
                    dialog.Content = control;
                    // First run of text changed
                    if (config.OnTextChanged != null)
                    {
                        var args = new PromptTextChangedArgs() { Value = control.TextEdit.Text };
                        config.OnTextChanged(args);
                        dialog.IsPrimaryButtonEnabled = args.IsValid;
                        int selStart = control.TextEdit.SelectionStart;
                        control.TextEdit.Text = args.Value;
                        control.TextEdit.SelectionStart = selStart;
                    }
                }

                dialog.PrimaryButtonClick += (s, e) => { HideContentDialog(); config.OnAction(new PromptResult(true, config.Text)); e.Cancel = true; };
                dialog.SecondaryButtonClick += (s, e) => { HideContentDialog(); config.OnAction(new PromptResult(false, config.Text)); e.Cancel = true; };
                ShowContentDialog(dialog);
            });
            return new DisposableAction(HideContentDialog);
        }

        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            return new DummyDisposable();
        }

        public override IDisposable Toast(ToastConfig config)
        {
            return new DummyDisposable();
        }

        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config)
        {
            return new ProgressDialog(config, this);
        }

        public void ShowContentDialog(FormsContentDialog dialog)
        {
            Dispatch(() =>
            {
                if (System.Windows.Application.Current?.MainWindow is FormsWindow window)
                {
                    try
                    {
                        window.HideContentDialog();
                    }
                    catch (Exception) { }
                    window.ShowContentDialog(dialog);
                }
            });
        }

        public void HideContentDialog()
        {
            Dispatch(() =>
            {
                try
                {
                    if (System.Windows.Application.Current?.MainWindow is FormsWindow window)
                    {
                        window.HideContentDialog();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Dismiss", $"Error hide content dialog - {ex.Message}");
                }
            });
        }

        public void Dispatch(Action action)
        {
            this.dispatcher.Invoke(action);
        }
    }
}
