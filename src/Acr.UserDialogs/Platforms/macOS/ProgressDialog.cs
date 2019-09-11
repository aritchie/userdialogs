using System;
using AppKit;
using CoreGraphics;

namespace Acr.UserDialogs
{
    public class ProgressDialog : IProgressDialog
    {
        readonly ProgressDialogConfig config;
        readonly NSPanel progressPanel;
        readonly NSProgressIndicator progressIndicator;
        readonly NSTextField txtTitle;
        readonly NSWindow mainWindow;
        
        public ProgressDialog(ProgressDialogConfig config)
        {
            this.config = config;
            this.title = config.Title;
            this.mainWindow = NSApplication.SharedApplication.KeyWindow;

            progressPanel = new NSPanel(new CGRect(0, 0, 100, 140), NSWindowStyle.DocModal, NSBackingStore.Buffered, true)
            {
                BackgroundColor = NSColor.White
            };

            var view = new NSView();

            txtTitle = new NSTextField
            {
                Editable = false,
                Hidden = string.IsNullOrEmpty(this.title),
                Alignment = NSTextAlignment.Center
            };

            progressIndicator = new NSProgressIndicator
            {
                Style =  NSProgressIndicatorStyle.Spinning,
                Indeterminate = !config.IsDeterministic,
                MinValue = 0,
                DoubleValue = 0,
                MaxValue = 100
            };

            view.AggregateSubviews(txtTitle, progressIndicator);

            NSButton cancelButton = null;
            if(config.OnCancel != null)
            {
                cancelButton = new NSButton
                {
                    Title = config.CancelText
                };
                cancelButton.Activated += (sender, e) =>
                {
                    Hide(true);
                };

                view.AggregateSubviews(cancelButton);
            }

            txtTitle.TopAnchor.ConstraintEqualToAnchor(view.TopAnchor).Active = true;
            txtTitle.LeadingAnchor.ConstraintEqualToAnchor(view.LeadingAnchor).Active = true;
            txtTitle.TrailingAnchor.ConstraintEqualToAnchor(view.TrailingAnchor).Active = true;

            progressIndicator.TopAnchor.ConstraintEqualToAnchor(txtTitle.BottomAnchor, 2).Active = true;
            progressIndicator.LeadingAnchor.ConstraintEqualToAnchor(view.LeadingAnchor).Active = true;
            progressIndicator.TrailingAnchor.ConstraintEqualToAnchor(view.TrailingAnchor).Active = true;
            progressIndicator.HeightAnchor.ConstraintEqualToConstant(100).Active = true;
            progressIndicator.WidthAnchor.ConstraintEqualToConstant(100).Active = true;

            if (cancelButton == null)
            {
                progressIndicator.BottomAnchor.ConstraintLessThanOrEqualToAnchor(view.BottomAnchor).Active = true;
            }
            else
            {
                cancelButton.TopAnchor.ConstraintEqualToAnchor(progressIndicator.BottomAnchor, 2).Active = true;
                cancelButton.CenterXAnchor.ConstraintEqualToAnchor(view.CenterXAnchor).Active = true;
                cancelButton.BottomAnchor.ConstraintLessThanOrEqualToAnchor(view.BottomAnchor).Active = true;
            }

            progressPanel.ContentView = view;
        }

        #region IProgressDialog Members

        string title;
        public virtual string Title
        {
            get { return this.title; }
            set
            {
                if (this.title == value)
                    return;

                this.title = value;
                this.Refresh();
            }
        }


        int percentComplete;
        public virtual int PercentComplete
        {
            get { return this.percentComplete; }
            set
            {
                if (this.percentComplete == value)
                    return;

                if (value > 100)
                    this.percentComplete = 100;
                else if (value < 0)
                    this.percentComplete = 0;
                else
                    this.percentComplete = value;
                this.Refresh();
            }
        }


        public virtual bool IsShowing { get; private set; }


        public virtual void Show()
        {
            this.Refresh();
            this.IsShowing = true;
        }

        public virtual void Hide()
        {
            Hide(false);
        }

        private void Hide(bool isCancelled)
        {
            this.IsShowing = false;
            NSApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                mainWindow.EndSheet(this.progressPanel, isCancelled ? NSModalResponse.Cancel : NSModalResponse.OK);
            });
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            this.Hide();
        }

        #endregion

        #region Internals

        protected virtual void Refresh()
        {
            txtTitle.StringValue = this.Title;
            if (this.config.IsDeterministic)
            {
                if (!String.IsNullOrWhiteSpace(txtTitle.StringValue))
                    txtTitle.StringValue += "... ";

                txtTitle.StringValue += this.PercentComplete + "%";
                progressIndicator.DoubleValue = this.PercentComplete;
            }
            else
                progressIndicator.StartAnimation(NSApplication.SharedApplication.KeyWindow);

            if (!this.IsShowing)
            {
                NSApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    mainWindow.BeginSheet(this.progressPanel, result =>
                    {
                        if (result == (int)NSModalResponse.Cancel)
                            config.OnCancel?.Invoke();
                    });
                });
            }
        }

        #endregion
    }
}