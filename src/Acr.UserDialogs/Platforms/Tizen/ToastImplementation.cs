using System;
using Xamarin.Forms;
using EPopup = ElmSharp.Popup;
using TForms = Xamarin.Forms.Platform.Tizen.Forms;

[assembly: Dependency(typeof(Acr.UserDialogs.ToastImplementation))]

namespace Acr.UserDialogs
{
	internal class ToastImplementation : IToast, IDisposable
	{
		static readonly string DefaultStyle = "toast";
		static readonly string DefaultPart = "default";

		int _duration = 3000;
		string _text = string.Empty;
		EPopup _control = null;
		bool _isDisposed = false;

		public int Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				_duration = value;
				UpdateDuration();
			}
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				UpdateText();
			}
		}

		public ToastImplementation()
		{
			_control = new EPopup(TForms.Context.MainWindow)
			{
				Style = DefaultStyle,
				AllowEvents = true,
			};

			UpdateText();
			UpdateDuration();
		}

		~ToastImplementation()
		{
			Dispose(false);
		}

		public void Show()
		{
			_control.Show();
		}

		public void Dismiss()
		{
			_control.Dismiss();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool isDisposing)
		{
			if (_isDisposed)
				return;

			if (isDisposing)
			{
				if (_control != null)
				{
					_control.Unrealize();
					_control = null;
				}
			}

			_isDisposed = true;
		}

		void UpdateDuration()
		{
			_control.Timeout = Duration / 1000.0;
		}

		void UpdateText()
		{
			_control.SetPartText(DefaultPart, Text);
		}
	}
}