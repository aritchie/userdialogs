using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Acr.UserDialogs.Infrastructure;


namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        readonly Func<Action, Task> dispatcher;


        public UserDialogsImpl(Func<Action, Task> dispatcher = null)
        {
            this.dispatcher = dispatcher ?? new Func<Action, Task>(x => CoreApplication
                .MainView
                .CoreWindow
                .Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, () => x())
                .AsTask()
            );
        }


        public override IDisposable Alert(AlertConfig config)
        {
            var dialog = new MessageDialog(config.Message, config.Title ?? String.Empty);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnAction?.Invoke()));
            IAsyncOperation<IUICommand> dialogTask = null;

            return this.DispatchAndDispose(
                //config.UwpSubmitOnEnterKey,
                //config.UwpCancelOnEscKey,
                () => dialogTask = dialog.ShowAsync(),
                () => dialogTask?.Cancel()
            );
        }


        public override IDisposable ActionSheet(ActionSheetConfig config)
        {
            var dlg = new ActionSheetContentDialog();

            var vm = new ActionSheetViewModel
            {
                Title = config.Title,
                Message = config.Message,
                Cancel = new ActionSheetOptionViewModel(config.Cancel != null, config.Cancel?.Text, () =>
                {
                    dlg.Hide();
                    config.Cancel?.Action?.Invoke();
                }),

                Destructive = new ActionSheetOptionViewModel(config.Destructive != null, config.Destructive?.Text, () =>
                {
                    dlg.Hide();
                    config.Destructive?.Action?.Invoke();
                }),

                Options = config
                    .Options
                    .Select(x => new ActionSheetOptionViewModel(true, x.Text, () =>
                    {
                        dlg.Hide();
                        x.Action?.Invoke();
                    }, x.ItemIcon ?? config.ItemIcon))
                    .ToList()
            };

            dlg.DataContext = vm;
            IAsyncOperation<ContentDialogResult> dialogTask = null;

            return this.DispatchAndDispose(
                //config.UwpSubmitOnEnterKey,
                //config.UwpCancelOnEscKey,
                () => dialogTask = dlg.ShowAsync(),
                () => dialogTask?.Cancel()
            );
        }


        public override IDisposable Confirm(ConfirmConfig config)
        {
            var dialog = new MessageDialog(config.Message, config.Title ?? String.Empty);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnAction?.Invoke(true)));
            dialog.DefaultCommandIndex = 0;

            dialog.Commands.Add(new UICommand(config.CancelText, x => config.OnAction?.Invoke(false)));
            dialog.CancelCommandIndex = 1;

            IAsyncOperation<IUICommand> dialogTask = null;
            return this.DispatchAndDispose(
                //config.UwpSubmitOnEnterKey,
                //config.UwpCancelOnEscKey,
                () => dialogTask = dialog.ShowAsync(),
                () => dialogTask?.Cancel()
            );
        }


        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            var picker = new DatePickerControl();
            if (config.MinimumDate != null)
                picker.DatePicker.MinDate = config.MinimumDate.Value;

            if (config.MaximumDate != null)
                picker.DatePicker.MaxDate = config.MaximumDate.Value;

            var popup = this.CreatePopup(picker);
            if (!config.IsCancellable)
                picker.CancelButton.Visibility = Visibility.Collapsed;
            else
            {
                picker.CancelButton.Content = config.CancelText;
                picker.CancelButton.Click += (sender, args) =>
                {
                    var result = new DatePromptResult(false, this.GetDateForCalendar(picker.DatePicker));
                    config.OnAction?.Invoke(result);
                    popup.IsOpen = false;
                };
            }

            picker.OkButton.Content = config.OkText;
            picker.OkButton.Click += (sender, args) =>
            {
                var result = new DatePromptResult(true, this.GetDateForCalendar(picker.DatePicker));
                config.OnAction?.Invoke(result);
                popup.IsOpen = false;
            };
            if (config.SelectedDate != null)
            {
                picker.DatePicker.SelectedDates.Add(config.SelectedDate.Value);
                picker.DatePicker.SetDisplayDate(config.SelectedDate.Value);
            }
            return this.DispatchAndDispose(
                //config.UwpSubmitOnEnterKey,
                //config.UwpCancelOnEscKey,
                () => popup.IsOpen = true,
                () => popup.IsOpen = false
            );
        }


        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            var picker = new TimePickerControl();
            picker.TimePicker.MinuteIncrement = config.MinuteInterval;

            var popup = this.CreatePopup(picker);

            if (!config.IsCancellable)
                picker.CancelButton.Visibility = Visibility.Collapsed;
            else
            {
                picker.CancelButton.Content = config.CancelText;
                picker.CancelButton.Click += (sender, args) =>
                {
                    var result = new TimePromptResult(false, picker.TimePicker.Time);
                    config.OnAction?.Invoke(result);
                    popup.IsOpen = false;
                };
            }

            if(config.Use24HourClock == true) picker.TimePicker.ClockIdentifier = "24HourClock";

            picker.OkButton.Content = config.OkText;
            picker.OkButton.Click += (sender, args) =>
            {
                var result = new TimePromptResult(true, picker.TimePicker.Time);
                config.OnAction?.Invoke(result);
                popup.IsOpen = false;
            };
            if (config.SelectedTime != null)
            {
                picker.TimePicker.Time = config.SelectedTime.Value;
            }
            return this.DispatchAndDispose(
                //config.UwpSubmitOnEnterKey,
                //config.UwpCancelOnEscKey,
                () => popup.IsOpen = true,
                () => popup.IsOpen = false
            );
        }


        public override IDisposable Login(LoginConfig config)
        {
            var vm = new LoginViewModel
            {
                LoginText = config.OkText,
                Title = config.Title ?? String.Empty,
                Message = config.Message ?? String.Empty,
                UserName = config.LoginValue,
                UserNamePlaceholder = config.LoginPlaceholder,
                PasswordPlaceholder = config.PasswordPlaceholder,
                CancelText = config.CancelText
            };
            vm.Login = new Command(() =>
                config.OnAction?.Invoke(new LoginResult(true, vm.UserName, vm.Password))
            );
            vm.Cancel = new Command(() =>
                config.OnAction?.Invoke(new LoginResult(false, vm.UserName, vm.Password))
            );
            var dlg = new LoginContentDialog
            {
                DataContext = vm
            };

            return this.DispatchAndDispose(
                //config.UwpSubmitOnEnterKey,
                //config.UwpCancelOnEscKey,
                () => dlg.ShowAsync(),
                dlg.Hide
            );
        }


        public override IDisposable Prompt(PromptConfig config)
        {
            var stack = new StackPanel();
            if (!String.IsNullOrWhiteSpace(config.Message))
                stack.Children.Add(new TextBlock { Text = config.Message, TextWrapping = TextWrapping.WrapWholeWords });

            var dialog = new ContentDialog
            {
                Title = config.Title ?? String.Empty,
                Content = stack,
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = config.OkText
            };

            if (config.InputType == InputType.Password)
                this.SetPasswordPrompt(dialog, stack, config);
            else
                this.SetDefaultPrompt(dialog, stack, config);

            if (config.IsCancellable)
            {
                dialog.SecondaryButtonText = config.CancelText;
                dialog.SecondaryButtonCommand = new Command(() =>
                {
                    config.OnAction?.Invoke(new PromptResult(false, String.Empty));
                    dialog.Hide();
                });
            }

            return this.DispatchAndDispose(
                //config.UwpSubmitOnEnterKey,
                //config.UwpCancelOnEscKey,
                () => dialog.ShowAsync(),
                dialog.Hide
            );
        }


        public override IDisposable Toast(ToastConfig config)
        {
            ToastPrompt toast = null;

            return this.DispatchAndDispose(
                () =>
                {
                    toast = new ToastPrompt
                    {
                        Message = config.Message,
                        //Stretch = Stretch.Fill,
                        TextWrapping = TextWrapping.WrapWholeWords,
                        MillisecondsUntilHidden = Convert.ToInt32(config.Duration.TotalMilliseconds)
                    };
                    if (config.Icon != null)
                        toast.ImageSource = new BitmapImage(new Uri(config.Icon));

                    if (config.MessageTextColor != null)
                        toast.Foreground = new SolidColorBrush(config.MessageTextColor.Value.ToNative());

                    if (config.BackgroundColor != null)
                        toast.Background = new SolidColorBrush(config.BackgroundColor.Value.ToNative());

                    toast.Show();
                },
                () => toast.Hide()
            );
        }


        #region Internals

        protected virtual Popup CreatePopup(UIElement element)
        {
            var popup = new Popup
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            // popup.LayoutUpdated += Popup_LayoutUpdated;
            // TODO: This is a workaround because sender is null when subscribing to the event
            popup.LayoutUpdated += (sender, e) =>
            {
                Popup_LayoutUpdated(popup, e);
            };
            if (element != null)
                popup.Child = element;

            return popup;
        }

        protected virtual DateTime GetDateForCalendar(CalendarView calendar)
            => calendar.SelectedDates.Any()
                ? calendar.SelectedDates.First().Date
                : DateTime.MinValue;


        protected virtual void SetPasswordPrompt(ContentDialog dialog, StackPanel stack, PromptConfig config)
        {
            var txt = new PasswordBox
            {
                PlaceholderText = config.Placeholder ?? String.Empty,
                Password = config.Text ?? String.Empty
            };
            if (config.MaxLength != null)
                txt.MaxLength = config.MaxLength.Value;

            stack.Children.Add(txt);
            dialog.PrimaryButtonCommand = new Command(() =>
            {
                config.OnAction?.Invoke(new PromptResult(true, txt.Password));
                dialog.Hide();
            });
            if (config.OnTextChanged == null)
                return;

            var args = new PromptTextChangedArgs { Value = txt.Password };
            config.OnTextChanged(args);
            dialog.IsPrimaryButtonEnabled = args.IsValid;

            txt.PasswordChanged += (sender, e) =>
            {
                args.IsValid = true; // reset
                args.Value = txt.Password;
                config.OnTextChanged(args);

                dialog.IsPrimaryButtonEnabled = args.IsValid;
                if (!args.Value.Equals(txt.Password))
                {
                    txt.Password = args.Value;
                }
            };
        }


        protected virtual void SetDefaultPrompt(ContentDialog dialog, StackPanel stack, PromptConfig config)
        {
            var txt = new TextBox
            {
                PlaceholderText = config.Placeholder ?? String.Empty,
                Text = config.Text ?? String.Empty
            };
            if (config.MaxLength != null)
                txt.MaxLength = config.MaxLength.Value;

            stack.Children.Add(txt);

            txt.SelectAll();

            dialog.PrimaryButtonCommand = new Command(() =>
            {
                config.OnAction?.Invoke(new PromptResult(true, txt.Text.Trim()));
                dialog.Hide();
            });

            if (config.OnTextChanged == null)
                return;

            var args = new PromptTextChangedArgs { Value = txt.Text };
            config.OnTextChanged(args);
            dialog.IsPrimaryButtonEnabled = args.IsValid;

            txt.TextChanged += (sender, e) =>
            {
                args.IsValid = true; // reset
                args.Value = txt.Text;
                config.OnTextChanged(args);
                dialog.IsPrimaryButtonEnabled = args.IsValid;

                if (!args.Value.Equals(txt.Text))
                {
                    txt.Text = args.Value;
                    txt.SelectionStart = Math.Max(0, txt.Text.Length);
                    txt.SelectionLength = 0;
                }
            };
        }


        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config) => new ProgressDialog(config, dispatcher);


        protected virtual IDisposable DispatchAndDispose(Action dispatch, Action dispose)
        {
            //TypedEventHandler<CoreWindow, KeyEventArgs> keyHandler = null;

            var disposer = new DisposableAction(() =>
            {
                try
                {
                    this.dispatcher.Invoke(dispose);
                }
                catch (Exception ex)
                {
                    Log.Error("Dismiss", "Error dismissing dialog - " + ex);
                }
                finally
                {
                    //if (keyHandler != null)
                    //    Window.Current.CoreWindow.KeyDown -= keyHandler;
                }
            });

            //keyHandler = (sender, args) =>
            //{
            //    switch (args.VirtualKey)
            //    {
            //        case VirtualKey.Escape:
            //            //if (escKey && vm.Cancel.CanExecute(null))
            //            //{
            //            //    dlg.Hide();
            //            //    vm.Cancel.Execute(null);
            //            //}
            //            break;

            //        case VirtualKey.Enter:
            //            //if (enterKey && vm.Login.CanExecute(null))
            //            //{
            //            //    dlg.Hide();
            //            //    vm.Login.Execute(null);
            //            //}
            //            break;
            //    }
            //};

            //if (enterKey || escKey)
            //    Window.Current.CoreWindow.KeyDown += keyHandler;

            this.dispatcher.Invoke(dispatch);
            return disposer;
        }
        #endregion

        #region Privates

        private static void Popup_LayoutUpdated(object sender, object e)
        {
            if (sender is Popup popup && popup.Child is Control control &&
                control.ActualWidth != 0 && control.ActualHeight != 0)
            {
                var newHorizontalOffset = (int)(Window.Current.Bounds.Width - control.ActualWidth) / 2;
                var newVerticalOffset = (int)(Window.Current.Bounds.Height - control.ActualHeight) / 2;

                if (popup.HorizontalOffset != newHorizontalOffset || popup.VerticalOffset != newVerticalOffset)
                {
                    popup.HorizontalOffset = newHorizontalOffset;
                    popup.VerticalOffset = newVerticalOffset;
                }
            }
        }
        #endregion
    }
}
