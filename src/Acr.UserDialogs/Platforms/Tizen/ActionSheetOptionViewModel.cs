﻿using Splat;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Acr.UserDialogs
{
	public class ActionSheetOptionViewModel
	{
		public ActionSheetOptionViewModel(string text, Action action, IBitmap image = null)
		{
			this.Text = text;
			this.Action = new Command(action);
			//this.Visible = visible ? Visibility.Visible : Visibility.Collapsed;
			this.ItemIcon = image;
		}

		//public Visibility Visible { get; }
		public string Text { get; }
		public ICommand Action { get; }
		public IBitmap ItemIcon { get; }
	}
}
