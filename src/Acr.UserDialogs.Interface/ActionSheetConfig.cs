using System;
using System.Collections.Generic;
using Splat;


namespace Acr.UserDialogs
{

    public class ActionSheetConfig : IAndroidStyleDialogConfig
    {
        public static int? DefaultAndroidStyleId { get; set; }

        public static string DefaultCancelText { get; set; } = "Cancel";
        public static string DefaultDestructiveText { get; set; } = "Remove";
        public static IBitmap DefaultItemIcon { get; set; }


        public string Title { get; set; }
        public ActionSheetOption Cancel { get; set; }
        public ActionSheetOption Destructive { get; set; }
        public IList<ActionSheetOption> Options { get; set; } = new List<ActionSheetOption>();
        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;

        /// <summary>
        /// This icon is applied to the list items, not to destructive or cancel
        /// </summary>
        public IBitmap ItemIcon { get; set; } = DefaultItemIcon;


        public ActionSheetConfig SetTitle(string title)
        {
            this.Title = title;
            return this;
        }


        public ActionSheetConfig SetCancel(string text = null, Action action = null)
        {
            this.Cancel = new ActionSheetOption(text ?? DefaultCancelText, action);
            return this;
        }


        public ActionSheetConfig SetDestructive(string text = null, Action action = null)
        {
            this.Destructive = new ActionSheetOption(text ?? DefaultDestructiveText, action);
            return this;
        }


        public ActionSheetConfig Add(string text, Action action = null, IBitmap icon = null)
        {
            this.Options.Add(new ActionSheetOption(text, action, icon));
            return this;
        }
    }
}
