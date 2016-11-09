using System;
using System.Drawing;


namespace Acr.UserDialogs
{
    public class DialogButton
    {
        public DialogButton(DialogChoice choice, string text, Color? textColor, bool visible)
        {
            this.Choice = choice;
            this.Text = text;
            this.TextColor = textColor;
            this.IsVisible = visible;
        }


        public DialogChoice Choice { get; }
        public string Text { get; set; }
        public Color? TextColor { get; set; }
        public bool IsVisible { get; set; }
    }
}
