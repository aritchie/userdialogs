using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;


namespace Acr.UserDialogs
{
    public class DatePickerController : UIViewController
    {
        protected DateTimePromptConfig Config { get; }
        protected UIDatePicker DatePicker { get; set; }
        protected UIToolbar Toolbar { get; set; }


        public DatePickerController(DateTimePromptConfig config)
        {
            this.Config = config;
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = this.Config.Title;
            this.View.BackgroundColor = UIColor.White;
            this.DatePicker = this.CreateDatePicker();
            this.Toolbar = this.CreateToolbar();

            this.View.AddSubview(this.Toolbar);
            this.View.AddSubview(this.DatePicker);
        }


        protected virtual UIToolbar CreateToolbar()
        {
            var items = new List<UIBarButtonItem>();

            if (this.Config.IsCancellable)
            {
                items.Add(this.CreateButton(this.Config.CancelText, () =>
                {
                    var date = this.FromNsDate(this.DatePicker.Date);
                    var result = new DateTimePromptResult(false, date);
                    this.DismissViewController(true, () => this.Config.OnResult?.Invoke(result));
                }));
            }

            var centerItem = String.IsNullOrWhiteSpace(this.Config.Title)
                ? new UIBarButtonItem { Title = this.Config.Title }
                : new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

            items.Add(centerItem);

            items.Add(this.CreateButton(this.Config.OkText, () =>
            {
                var date = this.FromNsDate(this.DatePicker.Date);
                var result = new DateTimePromptResult(false, date);
                this.DismissViewController(true, () => this.Config.OnResult?.Invoke(result));
            }));

            return new UIToolbar(new CGRect(0, 0, this.View.Frame.Width, 44)) { Items = items.ToArray() };
        }


        protected virtual UIBarButtonItem CreateButton(string text, Action action)
        {
            var btn = new UIBarButtonItem { Title = text };
            btn.Clicked += (sender, args) => action();
            return btn;
        }


        protected virtual UIDatePicker CreateDatePicker()
        {
            //new CGRect(0, 0, 200, 100)
            var picker = new UIDatePicker(new CGRect(0, 44, this.View.Frame.Width, 200))
            {
                Mode = (UIDatePickerMode)Enum.Parse(typeof(UIDatePickerMode), this.Config.Mode.ToString(), true),
                MinuteInterval = this.Config.MinuteInterval
            };
            if (this.Config.MinimumDate != null)
                picker.MinimumDate = (NSDate)this.Config.MinimumDate;

            if (this.Config.MaximumDate != null)
                picker.MaximumDate = (NSDate)this.Config.MaximumDate;

            if (this.Config.SelectedDateTime != null)
                picker.SetDate((NSDate)this.Config.SelectedDateTime, false);

            //this.View = new UIView(new CGRect(0, 0, 200, 100))
            //{
            //    BackgroundColor = UIColor.White
            //};
            //this.View.Alpha = 0.7f;

            return picker;
        }


        protected virtual DateTime FromNsDate(NSDate date)
        {
            return ((DateTime) date).ToLocalTime();
        }
    }
}

