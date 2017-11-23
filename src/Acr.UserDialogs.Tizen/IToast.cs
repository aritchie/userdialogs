namespace Acr.UserDialogs
{
	/// <summary>
	/// This interface, which defines the ability to display simple text, is used internally.
	/// </summary>
	internal interface IToast
	{
		/// <summary>
		/// Gets or sets the duration.
		/// </summary>
		int Duration { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		string Text { get; set; }

		/// <summary>
		/// Shows the view for the specified duration.
		/// </summary>
		void Show();

		/// <summary>
		/// Dismisses the specified view.
		/// </summary>
		void Dismiss();
	}
}