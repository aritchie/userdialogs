﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Coding4Fun.Toolkit.Controls;
using Splat;


namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        public override void Alert(AlertConfig config)
        {
            var dialog = new MessageDialog(config.Message, config.Title ?? String.Empty);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnOk?.Invoke()));
            this.Dispatch(() => dialog.ShowAsync());
        }


        public override void ActionSheet(ActionSheetConfig config)
        {
            var dlg = new ActionSheetContentDialog();

            var vm = new ActionSheetViewModel
            {
                Title = config.Title,
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
                    }, x.ItemIcon != null ? x.ItemIcon : config.ItemIcon))
                    .ToList()
            };

            dlg.DataContext = vm;
            this.Dispatch(() => dlg.ShowAsync());
        }


        public override void Confirm(ConfirmConfig config)
        {
            var dialog = new MessageDialog(config.Message, config.Title);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnConfirm(true)));
            dialog.DefaultCommandIndex = 0;

            dialog.Commands.Add(new UICommand(config.CancelText, x => config.OnConfirm(false)));
            dialog.CancelCommandIndex = 1;
            this.Dispatch(() => dialog.ShowAsync());
        }


        public override void DatePrompt(DatePromptConfig config)
        {
#if WINDOWS_PHONE_APP
            throw new NotImplementedException();
#else
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
                    config.OnResult?.Invoke(result);
                    popup.IsOpen = false;
                };
            }

            picker.OkButton.Content = config.OkText;
            picker.OkButton.Click += (sender, args) =>
            {
                var result = new DatePromptResult(true, this.GetDateForCalendar(picker.DatePicker));
                config.OnResult?.Invoke(result);
                popup.IsOpen = false;
            };
            if (config.SelectedDate != null)
            {
                picker.DatePicker.SelectedDates.Add(config.SelectedDate.Value);
                picker.DatePicker.SetDisplayDate(config.SelectedDate.Value);
            }
            popup.IsOpen = true;
#endif
        }


        public override void TimePrompt(TimePromptConfig config)
        {
#if WINDOWS_PHONE_APP
            throw new NotImplementedException();
#else
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
                    config.OnResult?.Invoke(result);
                    popup.IsOpen = false;
                };
            }

            picker.OkButton.Content = config.OkText;
            picker.OkButton.Click += (sender, args) =>
            {
                var result = new TimePromptResult(true, picker.TimePicker.Time);
                config.OnResult?.Invoke(result);
                popup.IsOpen = false;
            };
            if (config.SelectedTime != null)
            {
                picker.TimePicker.Time = config.SelectedTime.Value;
            }
            popup.IsOpen = true;
#endif
        }


        public override void Login(LoginConfig config)
        {
            var vm = new LoginViewModel
            {
                LoginText = config.OkText,
                Title = config.Title,
                Message = config.Message,
                UserName = config.LoginValue,
                UserNamePlaceholder = config.LoginPlaceholder,
                PasswordPlaceholder = config.PasswordPlaceholder,
                CancelText = config.CancelText
            };
            vm.Login = new Command(() =>
                config.OnResult?.Invoke(new LoginResult(vm.UserName, vm.Password, true))
            );
            vm.Cancel = new Command(() =>
                config.OnResult?.Invoke(new LoginResult(vm.UserName, vm.Password, false))
            );
            var dlg = new LoginContentDialog
            {
                DataContext = vm
            };
            this.Dispatch(() => dlg.ShowAsync());
        }


        public override void Prompt(PromptConfig config)
        {
            var stack = new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = config.Message }
                }
            };
            var dialog = new ContentDialog
            {
                Title = config.Title,
                Content = stack,
                PrimaryButtonText = config.OkText
            };

			// PromptTwoInputs added by Lee Bettridge
			if (config.ShowSecondInput == false)
			{
				if (config.InputType == InputType.Password)
					this.SetPasswordPrompt(dialog, stack, config);
				else
					this.SetDefaultPrompt(dialog, stack, config);
			}
			else
			{
				this.SetDualPrompts(dialog, stack, config);
			}

            if (config.IsCancellable)
            {
                dialog.SecondaryButtonText = config.CancelText;
                dialog.SecondaryButtonCommand = new Command(() =>
                {
                    config.OnResult?.Invoke(new PromptResult(false, String.Empty));
                    dialog.Hide();
                });
            }

            this.Dispatch(() => dialog.ShowAsync());
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis)
        {
            this.Show(image, message, ToastConfig.SuccessBackgroundColor, timeoutMillis);
        }


        public override void ShowError(string message, int timeoutMillis)
        {
            this.Show(null, message, ToastConfig.ErrorBackgroundColor, timeoutMillis);
        }


        public override void ShowSuccess(string message, int timeoutMillis)
        {
            this.Show(null, message, ToastConfig.SuccessBackgroundColor, timeoutMillis);
        }


        public override void Toast(ToastConfig config)
        {
            var toast = new ToastPrompt
            {
                Background = new SolidColorBrush(config.BackgroundColor.ToNative()),
                Foreground = new SolidColorBrush(config.TextColor.ToNative()),
                Title = config.Title,
                Message = config.Description,
                ImageSource = config.Icon?.ToNative(),
                Stretch = Stretch.Fill,
                MillisecondsUntilHidden = Convert.ToInt32(config.Duration.TotalMilliseconds)
            };
            //toast.Completed += (sender, args) => {
            //    if (args.PopUpResult == PopUpResult.Ok)
            //        config.Action?.Invoke();
            //};
            this.Dispatch(toast.Show);
        }


#region Internals

#if WINDOWS_UWP

        protected virtual Popup CreatePopup(UIElement element)
        {
            var popup = new Popup
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            if (element != null)
                popup.Child = element;

            return popup;
        }


        protected virtual DateTime GetDateForCalendar(CalendarView calendar)
        {
            return calendar.SelectedDates.Any()
                ? calendar.SelectedDates.First().Date
                : DateTime.MinValue;
        }


#endif

		// PromptTwoInputs added by Lee Bettridge
		protected virtual void SetDualPrompts(ContentDialog dialog, StackPanel stack, PromptConfig config)
		{
			PasswordBox pwd1 = null, pwd2 = null;
			TextBox txt1 = null, txt2 = null;

			if (config.InputType == InputType.Password)
			{
				pwd1 = new PasswordBox
				{
					PlaceholderText = config.Placeholder,
					Password = config.Text ?? String.Empty
				};
				stack.Children.Add(pwd1);
			}
			else
			{
				txt1 = new TextBox
				{
					PlaceholderText = config.Placeholder,
					Text = config.Text ?? String.Empty
				};
				stack.Children.Add(txt1);
			}

			if (config.SecondInputType == InputType.Password)
			{
				pwd2 = new PasswordBox
				{
					PlaceholderText = config.Placeholder,
					Password = config.Text ?? String.Empty
				};
				stack.Children.Add(pwd2);
			}
			else
			{
				txt2 = new TextBox
				{
					PlaceholderText = config.Placeholder,
					Text = config.Text ?? String.Empty
				};
				stack.Children.Add(txt2);
			}

			dialog.PrimaryButtonCommand = new Command(() =>
			{
				config.OnResult?.Invoke(new PromptResult(true, (config.InputType == InputType.Password) ? pwd1.Password : txt1.Text.Trim(), (config.SecondInputType == InputType.Password) ? pwd2.Password : txt2.Text.Trim()));
				dialog.Hide();
			});
		}

		protected virtual void SetPasswordPrompt(ContentDialog dialog, StackPanel stack, PromptConfig config)
        {
            var txt = new PasswordBox
            {
                PlaceholderText = config.Placeholder,
                Password = config.Text ?? String.Empty
            };
            stack.Children.Add(txt);

			dialog.PrimaryButtonCommand = new Command(() =>
			{
				config.OnResult?.Invoke(new PromptResult(true, txt.Password));
				dialog.Hide();
			});
        }


        protected virtual void SetDefaultPrompt(ContentDialog dialog, StackPanel stack, PromptConfig config)
        {
            var txt = new TextBox
            {
                PlaceholderText = config.Placeholder,
                Text = config.Text ?? String.Empty
            };
            stack.Children.Add(txt);

			dialog.PrimaryButtonCommand = new Command(() =>
			{
				config.OnResult?.Invoke(new PromptResult(true, txt.Text.Trim()));
				dialog.Hide();
			});
        }


        protected virtual void Show(IBitmap image, string message, Color bgColor, int timeoutMillis)
        {
            var stack = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 0.7
            };
            if (image != null)
            {
                var source = image.ToNative();
                stack.Children.Add(new Image { Source = source });
            }
            stack.Children.Add(new TextBlock
            {
                Text = message,
                FontSize = 24f,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold
            });

            var cd = new ContentDialog
            {
                Background = new SolidColorBrush(bgColor.ToNative()),
                BorderBrush = new SolidColorBrush(bgColor.ToNative()),
                Content = stack
            };
            stack.Tapped += (sender, args) => cd.Hide();

            this.Dispatch(() => cd.ShowAsync());
            Task.Delay(TimeSpan.FromMilliseconds(timeoutMillis))
                .ContinueWith(x => this.Dispatch(cd.Hide));
        }


        protected override IProgressDialog CreateDialogInstance()
        {
            return new ProgressDialog();
        }


        protected virtual void Dispatch(Action action)
        {
            //this.UiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }

#endregion
    }
}