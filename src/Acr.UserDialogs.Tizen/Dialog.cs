using System;
using Xamarin.Forms;

namespace Acr.UserDialogs
{
	/// <summary>
	/// The dialog widget displays its content with buttons and title.
	/// </summary>
	/// <example>
	/// <code>
	/// var dialog = new Dialog();
	/// dialog.Title = "Dialog"
	///
	/// var positive = new Button()
	/// {
	///     Text = "OK"
	/// }
	/// var negative = new Button()
	/// {
	///     Text = "Cancel"
	/// }
	/// negative.Clicked += (s,e)=>
	/// {
	///     dialog.Hide();
	/// }
	///
	/// dialog.Positive = positive;
	/// dialog.Negative = negative;
	///
	/// var label = new Label()
	/// {
	///     Text = "New Dialog"
	/// }
	///
	/// dialog.Content = label;
	///
	/// dialog.Show();
	///
	/// </code>
	/// </example>
	public class Dialog : BindableObject
	{
		/// <summary>
		/// BindableProperty. Identifies the content bindable property.
		/// </summary>
		public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(Dialog), null);

		/// <summary>
		/// BindableProperty. Identifies the positive bindable property.
		/// </summary>
		public static readonly BindableProperty PositiveProperty = BindableProperty.Create(nameof(Positive), typeof(Button), typeof(Dialog), null);

		/// <summary>
		/// BindableProperty. Identifies the neutral bindable property.
		/// </summary>
		public static readonly BindableProperty NeutralProperty = BindableProperty.Create(nameof(Neutral), typeof(Button), typeof(Dialog), null);

		/// <summary>
		/// BindableProperty. Identifies the negative bindable property.
		/// </summary>
		public static readonly BindableProperty NegativeProperty = BindableProperty.Create(nameof(Negative), typeof(Button), typeof(Dialog), null);

		/// <summary>
		/// BindableProperty. Identifies the title bindable property.
		/// </summary>
		public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(Dialog), null);

		/// <summary>
		/// BindableProperty. Identifies the subtitle bindable property.
		/// </summary>
		public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(Dialog), null);

		/// <summary>
		/// BindableProperty. Identifies the HorizontalOption bindable property.
		/// </summary>
		public static readonly BindableProperty HorizontalOptionProperty = BindableProperty.Create(nameof(HorizontalOption), typeof(LayoutOptions), typeof(Dialog), LayoutOptions.Center);

		/// <summary>
		/// BindableProperty. Identifies the VerticalOption bindable property.
		/// </summary>
		public static readonly BindableProperty VerticalOptionProperty = BindableProperty.Create(nameof(VerticalOption), typeof(LayoutOptions), typeof(Dialog), LayoutOptions.End);

		IDialog _dialog = null;

		/// <summary>
		/// Occurs when the dialog is hidden.
		/// </summary>
		public event EventHandler Hidden;

		/// <summary>
		/// Occurs when outside of the dialog is clicked.
		/// </summary>
		public event EventHandler OutsideClicked;

		/// <summary>
		/// Occurs when the dialog is shown on the display.
		/// </summary>
		public event EventHandler Shown;

		/// <summary>
		/// Occurs when the device's back button is pressed.
		/// </summary>
		public event EventHandler BackButtonPressed;

		public Dialog()
		{
			_dialog = DependencyService.Get<IDialog>(DependencyFetchTarget.NewInstance);
			if (_dialog == null)
			{
				throw new Exception("Object reference not set to an instance of a Dialog.");
			}

			_dialog.Hidden += (s, e) =>
			{
				Hidden?.Invoke(this, EventArgs.Empty);
			};

			_dialog.OutsideClicked += (s, e) =>
			{
				OutsideClicked?.Invoke(this, EventArgs.Empty);
			};

			_dialog.Shown += (s, e) =>
			{
				Shown?.Invoke(this, EventArgs.Empty);
			};

			_dialog.BackButtonPressed += (s, e) =>
			{
				BackButtonPressed?.Invoke(this, EventArgs.Empty);
			};

			SetBinding(ContentProperty, new Binding(nameof(Content), mode: BindingMode.OneWayToSource, source: _dialog));
			SetBinding(PositiveProperty, new Binding(nameof(Positive), mode: BindingMode.OneWayToSource, source: _dialog));
			SetBinding(NeutralProperty, new Binding(nameof(Neutral), mode: BindingMode.OneWayToSource, source: _dialog));
			SetBinding(NegativeProperty, new Binding(nameof(Negative), mode: BindingMode.OneWayToSource, source: _dialog));
			SetBinding(TitleProperty, new Binding(nameof(Title), mode: BindingMode.OneWayToSource, source: _dialog));
			SetBinding(SubtitleProperty, new Binding(nameof(Subtitle), mode: BindingMode.OneWayToSource, source: _dialog));
			SetBinding(HorizontalOptionProperty, new Binding(nameof(HorizontalOption), mode: BindingMode.OneWayToSource, source: _dialog));
			SetBinding(VerticalOptionProperty, new Binding(nameof(VerticalOption), mode: BindingMode.OneWayToSource, source: _dialog));
		}

		/// <summary>
		/// Gets or sets content view of the dialog.
		/// </summary>
		public View Content
		{
			get { return (View)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		/// <summary>
		/// Gets or sets positive button of the dialog.
		/// This button is on the left.
		/// When used alone, it is variable in size (can increase to the size of dialog).
		/// Dialog's all buttons style is bottom
		/// </summary>
		public Button Positive
		{
			get { return (Button)GetValue(PositiveProperty); }
			set { SetValue(PositiveProperty, value); }
		}

		/// <summary>
		/// Gets or sets neutral button of the dialog.
		/// This button is at the center.
		/// When used alone or used with positive, its size is half the size of the dialog and is on the right.
		/// </summary>
		public Button Neutral
		{
			get { return (Button)GetValue(NeutralProperty); }
			set { SetValue(NeutralProperty, value); }
		}

		/// <summary>
		/// Gets or sets negative button of the dialog.
		/// This button is always on the right and is displayed at a fixed size.
		/// </summary>
		public Button Negative
		{
			get { return (Button)GetValue(NegativeProperty); }
			set { SetValue(NegativeProperty, value); }
		}

		/// <summary>
		/// Gets or sets title of the dialog.
		/// </summary>
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		/// <summary>
		/// Gets or sets subtitle of the dialog.
		/// When title property value is null, subtitle is not displayed.
		/// </summary>
		public string Subtitle
		{
			get { return (string)GetValue(SubtitleProperty); }
			set { SetValue(SubtitleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the LayoutOptions that define how the dialog gets laid in a layout cycle.
		/// The default is center.
		/// </summary>
		public LayoutOptions HorizontalOption
		{
			get { return (LayoutOptions)GetValue(HorizontalOptionProperty); }
			set { SetValue(HorizontalOptionProperty, value); }
		}

		/// <summary>
		/// Gets or sets the LayoutOptions that define how the dialog gets laid in a layout cycle.
		/// The default is end.
		/// </summary>
		public LayoutOptions VerticalOption
		{
			get { return (LayoutOptions)GetValue(VerticalOptionProperty); }
			set { SetValue(VerticalOptionProperty, value); }
		}

		/// <summary>
		/// Shows the dialog.
		/// </summary>
		public void Show()
		{
			_dialog.Show();
		}

		/// <summary>
		/// Hides the dialog.
		/// </summary>
		public void Hide()
		{
			_dialog.Hide();
		}
	}
}