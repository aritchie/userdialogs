using System;
using Acr.UserDialogs;
using AppKit;
using Foundation;

namespace Acr.UserDialogs
{
    public class DatePickerController : NSViewController
    {
        public DatePickerController()
        {
        }

        public DatePickerController(NSViewController presentor)
        {
            this.Presentor = presentor;
        }

        public NSDatePickerElementFlags ElementFlags { get; set; } = NSDatePickerElementFlags.YearMonthDate;
        public DateTime SelectedDateTime { get; set; } = DateTime.Now;
        public DateTime? MaximumDateTime { get; set; }
        public DateTime? MinimumDateTime { get; set; }
        public int MinuteInterval { get; set; } = 1;
        public string OkText { get; set; }
        public bool Use24HourClock { get; set; }
        public Action<DatePickerController> Ok { get; set; }
        public string CancelText { get; set; }
        public Action<DatePickerController> Cancel { get; set; }

        public NSViewController Presentor { get; set; }

        public override void LoadView()
        {
            var view = new NSView();

            var lblTitle = new NSTextField
            {
                Editable = false,
                Hidden = string.IsNullOrEmpty(this.Title),
                Alignment = NSTextAlignment.Center,
                StringValue = this.Title ?? string.Empty
            };

            var datePicker = new NSDatePicker
            {
                DatePickerStyle = NSDatePickerStyle.ClockAndCalendar,
                DatePickerElements = this.ElementFlags,
                DateValue = this.SelectedDateTime.ToNSDate(),
                TimeInterval = this.MinuteInterval
            };
            if (this.Use24HourClock)
                datePicker.Locale = NSLocale.FromLocaleIdentifier("NL");

            if (this.MaximumDateTime != null)
                datePicker.MinDate = this.MaximumDateTime.Value.ToNSDate();

            if (this.MaximumDateTime != null)
                datePicker.MaxDate = this.MaximumDateTime.Value.ToNSDate();

            var okButton = new NSButton
            {
                Title = this.OkText
            };
            okButton.Activated += (sender, e) =>
            {
                this.SelectedDateTime = datePicker.DateValue.ToDateTime();
                this.Presentor.DismissViewController(this);
                this.Ok?.Invoke(this);
            };

            var cancelButton = new NSButton
            {
                Title = this.CancelText
            };
            cancelButton.Activated += (sender, e) =>
            {
                this.Presentor.DismissViewController(this);
                this.Cancel?.Invoke(this);
            };

            view.AggregateSubviews(lblTitle, datePicker, okButton, cancelButton);

            // Constraints
            Extensions.ActivateConstraints(
                lblTitle.TopAnchor.ConstraintEqualToAnchor(view.TopAnchor),
                lblTitle.LeadingAnchor.ConstraintEqualToAnchor(view.LeadingAnchor),
                lblTitle.TrailingAnchor.ConstraintEqualToAnchor(view.TrailingAnchor),

                datePicker.TopAnchor.ConstraintEqualToAnchor(lblTitle.BottomAnchor, 2),
                datePicker.LeadingAnchor.ConstraintEqualToAnchor(view.LeadingAnchor),
                datePicker.TrailingAnchor.ConstraintEqualToAnchor(view.TrailingAnchor),

                okButton.TopAnchor.ConstraintEqualToAnchor(datePicker.BottomAnchor, 2),
                okButton.TrailingAnchor.ConstraintEqualToAnchor(view.TrailingAnchor),
                okButton.BottomAnchor.ConstraintEqualToAnchor(view.BottomAnchor),

                cancelButton.TrailingAnchor.ConstraintEqualToAnchor(okButton.LeadingAnchor, 2),
                cancelButton.LeadingAnchor.ConstraintGreaterThanOrEqualToAnchor(view.LeadingAnchor),
                cancelButton.BottomAnchor.ConstraintEqualToAnchor(okButton.BottomAnchor)
            );

            this.View = view;
        }
    }
}
