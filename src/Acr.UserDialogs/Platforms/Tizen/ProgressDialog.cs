using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Acr.UserDialogs
{
	class ProgressDialog : IProgressDialog
	{
		readonly ProgressDialogConfig config;
		Dialog dialog;
		Button positive;
		ProgressBar progress;
		ActivityIndicator loading;

		public ProgressDialog(ProgressDialogConfig config)
		{
			this.config = config;
			this.title = config.Title;
		}

		#region IProgressDialog Members

		string title;
		public virtual string Title
		{
			get { return this.title; }
			set
			{
				if (this.title == value)
					return;

				this.title = value;
				this.Refresh();
			}
		}

		int percentComplete;
		public virtual int PercentComplete
		{
			get { return this.percentComplete; }
			set
			{
				if (this.percentComplete == value)
					return;

				if (value > 100)
					this.percentComplete = 100;
				else if (value < 0)
					this.percentComplete = 0;
				else
					this.percentComplete = value;
				this.Refresh();
			}
		}

		public virtual bool IsShowing { get; private set; }

		public void Hide()
		{
			this.IsShowing = false;
			this.dialog.Hide();
		}

		public void Dispose()
		{
			this.Hide();
		}

		public void Show()
		{
			positive = new Button()
			{
				Text = config.CancelText
			};
			var layout = new StackLayout()
			{
				Padding = 30
			};
			if (config.IsDeterministic)
			{
				progress = new ProgressBar();
				layout.Children.Add(progress);
			}
			else
			{
				loading = new ActivityIndicator { Color = Color.Blue };
				loading.IsRunning = true;
				layout.Children.Add(loading);
			}
			
			if (this.IsShowing)
				return;

			this.IsShowing = true;

			if (this.dialog == null)
			{
				this.dialog = new Dialog
				{
					Title = config.Title,
					Content = layout,
					HorizontalOption = LayoutOptions.Center,
					VerticalOption = LayoutOptions.Center,
				};
			}
			if (this.config.OnCancel != null)
			{
				dialog.Positive = positive;
			}
			positive.Clicked += (s, e) =>
			{
				if (this.config.OnCancel == null)
					return;
				dialog.Hide();
				config.OnCancel();
			};
			dialog.OutsideClicked += (s, e) =>
			{
				dialog.Hide();
			};
			this.dialog.Show();
		}

		#endregion

		#region Internals

		protected virtual void Refresh()
		{
			if (!this.IsShowing)
				return;

			var txt = this.Title;
			float p = -1;
			if (this.config.IsDeterministic)
			{
				p = (float)this.PercentComplete / 100;
				progress.Progress = p;
				if (!String.IsNullOrWhiteSpace(txt))
				{
					txt += "... ";
				}
				txt += this.PercentComplete + "%";
				dialog.Title = txt;
			}
		}

		#endregion
	}
}
