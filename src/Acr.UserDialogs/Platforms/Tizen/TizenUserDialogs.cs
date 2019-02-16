using System;
using System.Diagnostics;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using EWindow = ElmSharp.Window;
using XButton = Xamarin.Forms.Button;
using XLable = Xamarin.Forms.Label;
using Tizen.Applications;


namespace Acr.UserDialogs
{
	public class TizenUserDialogs : AbstractUserDialogs
	{
	    public static void RegisterSingleton(EWindow window)
	    {
            UserDialogs.Instance = new TizenUserDialogs(window);
	    }


		EWindow window;

		public TizenUserDialogs(EWindow win)
		{
			window = win;
		}

		public override IDisposable Alert(AlertConfig config)
		{
			XButton positive = new XButton()
			{
				Text = config.OkText
			};
			XLable content = new XLable()
			{
				Text = config.Message
			};
			var layout = new StackLayout
			{
				Children = {
					content,
				},
				Padding = 30
			};
			var dialog = new Dialog()
			{
				Title = config.Title,
				//Subtitle = config.Message,
				Content = layout,
				HorizontalOption = LayoutOptions.Center,
				VerticalOption = LayoutOptions.Center,
				Positive = positive
			};
			dialog.OutsideClicked += (s, e) =>
			{
				dialog.Hide();
			};
			positive.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.OnAction?.Invoke();
			};
			return Show(dialog);
		}

		public override IDisposable Confirm(ConfirmConfig config)
		{
			XButton positive = new XButton()
			{
				Text = config.OkText
			};
			XButton negative = new XButton()
			{
				Text = config.CancelText
			};
			XLable content = new XLable()
			{
				Text = config.Message
			};
			var layout = new StackLayout
			{
				Children = {
					content,
				},
				Padding = 30
			};
			var dialog = new Dialog()
			{
				Title = config.Title,
				//Subtitle = config.Message,
				Content = layout,
				HorizontalOption = LayoutOptions.Center,
				VerticalOption = LayoutOptions.Center,
				Negative = negative,
				Positive = positive
			};
			dialog.OutsideClicked += (s, e) =>
			{
				dialog.Hide();
			};
			positive.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.OnAction?.Invoke(true);
			};
			negative.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.OnAction?.Invoke(false);
			};
			return Show(dialog);
		}

		public override IDisposable DatePrompt(DatePromptConfig config)
		{
			var date = new DateTimeView()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				MaximumDate = config.MaximumDate ?? new DateTime(2030, 12, 31),
				MinimumDate = config.MinimumDate ?? new DateTime(2017, 1, 1),
				DateTime = config.SelectedDate ?? DateTime.Now,
				DisplayFormat = "%F",
			};
			XButton positive = new XButton()
			{
				Text = config.OkText
			};
			XButton negative = new XButton()
			{
				Text = config.CancelText
			};
			var layout = new StackLayout
			{
				Children = {
					date,
				},
			};
			var dialog = new Dialog()
			{
				Title = config.Title,
				//Subtitle = config.Message,
				Content = layout,
				HorizontalOption = LayoutOptions.Center,
				VerticalOption = LayoutOptions.Center,
				Negative = negative,
				Positive = positive
			};
			dialog.OutsideClicked += (s, e) =>
			{
				dialog.Hide();
			};
			positive.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.OnAction?.Invoke(new DatePromptResult(true, date.DateTime));
			};
			negative.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.OnAction?.Invoke(new DatePromptResult(false, date.DateTime));
			};
			return Show(dialog);
		}

		public override IDisposable TimePrompt(TimePromptConfig config)
		{
			var time = new DateTimeView()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				DateTime = config.SelectedTime != null ? DateTime.Today.Add((TimeSpan)config.SelectedTime) : DateTime.Now,
			};
			var positive = new XButton
			{
				Text = config.OkText
			};
			var negative = new XButton
			{
				Text = config.CancelText
			};
			var layout = new StackLayout
			{
				Children =
				{
					time
				},
				Padding = 30
			};
			if (config.Use24HourClock == null)
				time.DisplayFormat = "%T";
			else
			{
				if (config.Use24HourClock.Value)
					time.DisplayFormat = "%p %T";
				else
					time.DisplayFormat = "%T";
			}
			var dialog = new Dialog()
			{
				Title = config.Title,
				//Subtitle = config.Message,
				Content = layout,
				HorizontalOption = LayoutOptions.Center,
				VerticalOption = LayoutOptions.Center,
				Negative = negative,
				Positive = positive
			};
			dialog.OutsideClicked += (s, e) =>
			{
				dialog.Hide();
			};
			positive.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.OnAction?.Invoke(new TimePromptResult(true, time.DateTime.TimeOfDay));
			};
			negative.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.OnAction?.Invoke(new TimePromptResult(false, time.DateTime.TimeOfDay));
			};
			return Show(dialog);
		}

		public override IDisposable Prompt(PromptConfig config)
		{
			var positive = new XButton
			{
				Text = config.OkText
			};
			var negative = new XButton
			{
				Text = config.CancelText
			};
			var txt = new Entry()
			{
				Placeholder = config.Placeholder ?? String.Empty,
				Text = config.Text ?? String.Empty,
			};
			var layout = new StackLayout
			{
				Children = {
					txt,
				},
				Padding = 30
			};
			var dialog = new Dialog()
			{
				Title = config.Title ?? String.Empty,
				Subtitle = config.Message,
				Content = layout,
				HorizontalOption = LayoutOptions.Center,
				VerticalOption = LayoutOptions.Center,
			};
			if (config.IsCancellable)
			{
				dialog.Negative = negative;
			}
			if (config.OkText != null)
			{
				positive.Text = config.OkText;
				dialog.Positive = positive;
			}
			this.SetInputType(txt, config.InputType);
			if (config.MaxLength != null)
			{
				txt.TextChanged += (s, e) =>
				{
					var entry = (Entry)s;

					if (entry.Text.Length > config.MaxLength)
					{
						string entryText = entry.Text;
						entryText = entryText.Remove(entryText.Length - 1);
						entry.Text = entryText;
					}
				};
			}
			if (config.OnTextChanged != null)
			{
				if (config.InputType == InputType.Password)
				{
					txt.IsPassword = true;
					txt.TextChanged += (sender, e) =>
					{
						txt.IsPassword = true;
					};
				}

				var args = new PromptTextChangedArgs { Value = txt.Text };
				config.OnTextChanged(args);
				positive.IsEnabled = args.IsValid;
				txt.TextChanged += (s, e) =>
				{
					args.IsValid = true;
					args.Value = txt.Text;
					config.OnTextChanged(args);
					positive.IsEnabled = args.IsValid;
					if (!args.Value.Equals(txt.Text))
					{
						txt.Text = args.Value;
						//txt.SelectionStart = Math.Max(0, txt.Text.Length);
						//txt.SelectionLength = 0;
					}
				};
			}
			dialog.OutsideClicked += (s, e) =>
			{
				dialog.Hide();
			};
			positive.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.OnAction?.Invoke(new PromptResult(true, txt.Text.Trim()));
			};
			negative.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.OnAction?.Invoke(new PromptResult(false, txt.Text));
			};
			return Show(dialog);
		}


		public override IDisposable ActionSheet(ActionSheetConfig config)
		{
			var positive = new XButton();
			var negative = new XButton();

			var template = new DataTemplate(typeof(TextCell));
			template.SetBinding(TextCell.TextProperty, "Text");
			template.SetBinding(ImageCell.ImageSourceProperty, "ItemIcon");
			ListView lv = new ListView()
			{
				ItemsSource = config.Options,
				ItemTemplate = template,
				HeightRequest = 144 * config.Options.Count
			};
			var layout = new StackLayout
			{
				Children = {
					lv,
				},
				Padding = 30
			};
			var dialog = new Dialog()
			{
				Title = config.Title,
				Subtitle = config.Message,
				Content = layout,
				HorizontalOption = LayoutOptions.Center,
				VerticalOption = LayoutOptions.Center,
			};
			if (config.Destructive?.Text != null)
			{
				positive.Text = config.Destructive?.Text;
				dialog.Positive = positive;
			}
			if (config.Cancel?.Text != null)
			{
				negative.Text = config.Cancel?.Text;
				dialog.Negative = negative;
			}
			lv.ItemSelected += (s, e) =>
			{
				if (e.SelectedItem != null)
				{
					dialog.Hide();
					config.Options[config.Options.IndexOf(e.SelectedItem)]?.Action?.Invoke();
				}
			};
			dialog.OutsideClicked += (s, e) =>
			{
				dialog.Hide();
			};
			positive.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.Destructive?.Action?.Invoke();
			};
			negative.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.Cancel?.Action?.Invoke();
			};
			dialog.Show();
			return new DisposableAction(() =>
			{
				dialog.Hide();
				config.Cancel?.Action?.Invoke();
			});
		}

		public override IDisposable Login(LoginConfig config)
		{
			Entry textUser = new Entry()
			{
				Placeholder = config.LoginPlaceholder,
				Text = config.LoginValue ?? String.Empty,
			};
			Entry textPass = new Entry
			{
				Placeholder = config.PasswordPlaceholder,
				IsPassword = true,
			};
			var layout = new StackLayout
			{
				Children = {
					textUser,
					textPass,
				},
				Padding = 30
			};
			var positive = new XButton
			{
				Text = config.OkText
			};
			var negative = new XButton
			{
				Text = config.CancelText
			};
			var dialog = new Dialog()
			{
				Title = config.Title,
				Subtitle = config.Message,
				Content = layout,
				HorizontalOption = LayoutOptions.Center,
				VerticalOption = LayoutOptions.Center,
				Negative = negative,
				Positive = positive
			};
			dialog.OutsideClicked += (s, e) =>
			{
				dialog.Hide();
			};
			positive.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.OnAction?.Invoke(new LoginResult(true, textUser.Text, textPass.Text));
			};
			negative.Clicked += (s, e) =>
			{
				dialog.Hide();
				config.OnAction?.Invoke(new LoginResult(false, textUser.Text, textPass.Text));
			};
			return Show(dialog);
		}


		public override IDisposable Toast(ToastConfig config)
		{
			ToastMessage toast = new ToastMessage
			{
				Message = config.Message,
			};
			toast.Post();
			return new DisposableAction(()=> { });
		}

		//public override void ShowError(string message, int timeoutMillis)
		//{
		//	this.Toast(new ToastConfig(message));
		//}

		//public override void ShowSuccess(string message, int timeoutMillis)
		//{
		//	this.Toast(new ToastConfig(message));
		//}

		//public override void ShowImage(IBitmap image, string message, int timeoutMillis)
		//{
		//	this.Toast(new ToastConfig(message));
		//}

		protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config)
			 => new ProgressDialog(config);


		protected virtual IDisposable Show(Dialog dialog)
		{
			dialog.Show();
			return new DisposableAction(() =>
				dialog.Hide()
			);
		}


		protected virtual void SetInputType(Entry txt, InputType inputType)
		{
			switch (inputType)
			{
				case InputType.DecimalNumber:
					txt.Keyboard = Keyboard.Numeric;
					break;

				case InputType.Email:
					txt.Keyboard = Keyboard.Email;
					break;

				case InputType.Name:
					break;

				case InputType.Number:
					txt.Keyboard = Keyboard.Numeric;
					break;

				case InputType.NumericPassword:
					txt.IsPassword = true;
					txt.Keyboard = Keyboard.Numeric;
					break;

				case InputType.Password:
					txt.IsPassword = true;
					break;

				case InputType.Phone:
					txt.Keyboard = Keyboard.Telephone;
					break;

				case InputType.Url:
					txt.Keyboard = Keyboard.Url;
					break;
			}
		}
	}
}
