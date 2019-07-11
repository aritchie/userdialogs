namespace Acr.UserDialogs
{
	/// <summary>
	/// The Toast class provides properties that show simple types of messages.
	/// </summary>
	/// <example>
	/// <code>
	/// Toast.DisplayText("Hello World", 3000)
	/// </code>
	/// </example>
	public sealed class Toast
	{
		/// <summary>
		/// It shows the simplest form of the message.
		/// </summary>
		/// <param name="text">The body text of the toast.</param>
		/// <param name="duration">How long to display the text in milliseconds.</param>
		public static void DisplayText(string text, int duration = 3000)
		{
			new ToastProxy
			{
				Text = text,
				Duration = duration,
			}.Show();
		}
	}
}