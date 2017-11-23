using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using TForms = Xamarin.Forms.Platform.Tizen.Forms;
using EPopup = ElmSharp.Popup;

[assembly: Dependency(typeof(Acr.UserDialogs.DialogImplementation))]

namespace Acr.UserDialogs
{
	class DialogImplementation : IDialog, IDisposable
	{
		EPopup _control;
		View _content;
		Button _positive;
		Button _neutral;
		Button _negative;
		string _title;
		string _subtitle;
		StackLayout _contentView;
		LayoutOptions _horizontalOption = LayoutOptions.Center;
		LayoutOptions _verticalOption = LayoutOptions.End;

		LayoutOptions _previousHorizontalOption = LayoutOptions.Center;

		bool _isDisposed = false;

		ElmSharp.Button _nativePositive;

		ElmSharp.Button _nativeNeutral;
		ElmSharp.Button _nativeNegative;
		ElmSharp.EvasObject _nativeContent;

		public event EventHandler Hidden;

		public event EventHandler OutsideClicked;

		public event EventHandler Shown;

		public event EventHandler BackButtonPressed;

		public DialogImplementation()
		{
			_control = new EPopup(TForms.Context.MainWindow);

			_control.ShowAnimationFinished += ShowAnimationFinishedHandler;
			_control.Dismissed += DismissedHandler;
			_control.OutsideClicked += OutsideClickedHandler;
			_control.BackButtonPressed += BackButtonPressedHandler;

			_contentView = new StackLayout();
		}

		~DialogImplementation()
		{
			Dispose(false);
		}

		public View Content
		{
			get
			{
				return _content;
			}
			set
			{
				_content = value;
				UpdateContent();
			}
		}

		public Button Positive
		{
			get
			{
				return _positive;
			}
			set
			{
				_positive = value;
				UpdatePositive();
			}
		}

		public Button Neutral
		{
			get
			{
				return _neutral;
			}
			set
			{
				_neutral = value;
				UpdateNeutral();
			}
		}

		public Button Negative
		{
			get
			{
				return _negative;
			}
			set
			{
				_negative = value;
				UpdateNegative();
			}
		}

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
				UpdateTitle();
			}
		}

		public string Subtitle
		{
			get
			{
				return _subtitle;
			}
			set
			{
				_subtitle = value;
				UpdateSubtitle();
			}
		}

		public LayoutOptions HorizontalOption
		{
			get
			{
				return _horizontalOption;
			}
			set
			{
				_horizontalOption = value;
				UpdateHorizontalOption();
			}
		}

		public LayoutOptions VerticalOption
		{
			get
			{
				return _verticalOption;
			}
			set
			{
				_verticalOption = value;
				UpdateVerticalOption();
			}
		}

		public void Show()
		{
			if (Application.Current.Platform == null)
			{
				throw new Exception("When the Application's Platform is null, can not show the Dialog.");
			}
			if (_contentView.Platform == null)
			{
				UpdateContent();
			}
			_control.Show();
		}

		public void Hide()
		{
			_control.Hide();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				if (_nativePositive != null)
				{
					_nativePositive.Unrealize();
					_nativePositive = null;
				}
				if (_nativeNeutral != null)
				{
					_nativeNeutral.Unrealize();
					_nativeNeutral = null;
				}
				if (_nativeNegative != null)
				{
					_nativeNegative.Unrealize();
					_nativeNegative = null;
				}
				if (_nativeContent != null)
				{
					_nativeContent.Unrealize();
					_nativeContent = null;
				}

				if (_control != null)
				{
					_control.ShowAnimationFinished -= ShowAnimationFinishedHandler;
					_control.Dismissed -= DismissedHandler;
					_control.OutsideClicked -= OutsideClickedHandler;
					_control.BackButtonPressed -= BackButtonPressedHandler;

					_control.Unrealize();
					_control = null;
				}
			}

			_isDisposed = true;
		}

		void ShowAnimationFinishedHandler(object sender, EventArgs e)
		{
			_nativeContent?.MarkChanged();
			Shown?.Invoke(this, EventArgs.Empty);
		}

		void DismissedHandler(object sender, EventArgs e)
		{
			Hidden?.Invoke(this, EventArgs.Empty);
		}

		void OutsideClickedHandler(object sender, EventArgs e)
		{
			OutsideClicked?.Invoke(this, EventArgs.Empty);
		}

		void BackButtonPressedHandler(object sender, EventArgs e)
		{
			BackButtonPressed?.Invoke(this, EventArgs.Empty);
		}

		void UpdateContent()
		{
			if (Application.Current.Platform == null)
				return;

			_contentView.Children.Clear();

			if (Content != null)
			{
				_contentView.Children.Add(Content);

				_contentView.Platform = Application.Current.Platform;

				var renderer = Platform.GetOrCreateRenderer(_contentView);
				(renderer as LayoutRenderer)?.RegisterOnLayoutUpdated();

				var sizeRequest = _contentView.Measure(TForms.Context.MainWindow.ScreenSize.Width, TForms.Context.MainWindow.ScreenSize.Height).Request.ToPixel();

				_nativeContent = renderer.NativeView;
				_nativeContent.MinimumHeight = sizeRequest.Height;

				_control.SetPartContent("default", _nativeContent, true);
			}
			else
			{
				_control.SetPartContent("default", null, true);
			}
		}

		void UpdatePositive()
		{
			_nativePositive?.Hide();

			if (Positive != null)
			{
				_nativePositive = (ElmSharp.Button)Platform.GetOrCreateRenderer(Positive).NativeView;
				_nativePositive.Style = "popup";
			}
			else
			{
				_nativePositive = null;
			}

			_control.SetPartContent("button1", _nativePositive, true);
		}

		void UpdateNeutral()
		{
			_nativeNeutral?.Hide();

			if (Neutral != null)
			{
				_nativeNeutral = (ElmSharp.Button)Platform.GetOrCreateRenderer(Neutral).NativeView;
				_nativeNeutral.Style = "popup";
			}
			else
			{
				_nativeNeutral = null;
			}

			_control.SetPartContent("button2", _nativeNeutral, true);
		}

		void UpdateNegative()
		{
			_nativeNegative?.Hide();

			if (Negative != null)
			{
				_nativeNegative = (ElmSharp.Button)Platform.GetOrCreateRenderer(Negative).NativeView;
				_nativeNegative.Style = "popup";
			}
			else
			{
				_nativeNegative = null;
			}

			_control.SetPartContent("button3", _nativeNegative, true);
		}

		void UpdateTitle()
		{
			_control.SetPartText("title,text", Title);
		}

		void UpdateSubtitle()
		{
			_control.SetPartText("subtitle,text", Subtitle);
		}

		void UpdateHorizontalOption()
		{
			switch (HorizontalOption.Alignment)
			{
				case LayoutAlignment.Start:
					_control.AlignmentX = 0.0;
					break;

				case LayoutAlignment.Center:
					_control.AlignmentX = 0.5;
					break;

				case LayoutAlignment.End:
					_control.AlignmentX = 1.0;
					break;

				case LayoutAlignment.Fill:
					_control.AlignmentX = -1;
					break;
			}
			if (HorizontalOption.Alignment == LayoutAlignment.Fill || _previousHorizontalOption.Alignment == LayoutAlignment.Fill)
			{
				UpdateContent();
				_previousHorizontalOption = HorizontalOption;
			}
		}

		void UpdateVerticalOption()
		{
			switch (VerticalOption.Alignment)
			{
				case LayoutAlignment.Start:
					_control.AlignmentY = 0.0;
					break;

				case LayoutAlignment.Center:
					_control.AlignmentY = 0.5;
					break;

				case LayoutAlignment.End:
					_control.AlignmentY = 1.0;
					break;

				case LayoutAlignment.Fill:
					_control.AlignmentY = -1;
					break;
			}
		}
	}
}