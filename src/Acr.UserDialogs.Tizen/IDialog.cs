using System;
using Xamarin.Forms;

namespace Acr.UserDialogs
{
	internal interface IDialog
	{
		event EventHandler Hidden;

		event EventHandler OutsideClicked;

		event EventHandler Shown;

		event EventHandler BackButtonPressed;

		View Content { get; set; }

		Button Positive { get; set; }

		Button Neutral { get; set; }

		Button Negative { get; set; }

		string Title { get; set; }

		string Subtitle { get; set; }

		LayoutOptions HorizontalOption { get; set; }

		LayoutOptions VerticalOption { get; set; }

		void Show();

		void Hide();
	}
}