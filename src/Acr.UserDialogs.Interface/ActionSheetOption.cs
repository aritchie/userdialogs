using System;
using Splat;


namespace Acr.UserDialogs 
{

    public class ActionSheetOption 
    {
        public ActionSheetOption(string text, Action action = null, IBitmap icon = null) 
        {
            this.Text = text;
            this.Action = action;
            this.ItemIcon = icon;
        }


        public string Text { get; set; }
        public Action Action { get; set; }
        public IBitmap ItemIcon { get; set; }
    }
}
