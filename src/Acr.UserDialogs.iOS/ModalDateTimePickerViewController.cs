// Slightly modified version of 
using System;
using UIKit;
using CoreGraphics;

namespace Acr.UserDialogs
{
    public class ModalDateTimePickerViewController : UIViewController
    {
        const float ToolbarHeight = 44;

        UILabel _headerLabel;
        UIButton _doneButton;
        UIButton _cancelButton;
        UIViewController _parent;
        UIView _internalView;


        public ModalDateTimePickerViewController(string headerText, UIViewController parent)
        {
            HeaderBackgroundColor = UIColor.White;
            HeaderTextColor = UIColor.Black;
            HeaderText = headerText;

            DoneButtonText = "Done";
            CancelButtonText = "Cancel";

            _parent = parent;
        }


        public UIColor HeaderBackgroundColor { get; set; }
        public UIColor HeaderTextColor { get; set; }
        public string HeaderText { get; set; }
        public string DoneButtonText { get; set; }
        public string CancelButtonText { get; set; }
        public UIDatePicker DatePicker { get; set; }
        public event EventHandler<bool> Dismissed;


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _internalView = new UIView();

            _headerLabel = new UILabel(new CGRect(0, 0, 320/2, 44));
            _headerLabel.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            _headerLabel.BackgroundColor = HeaderBackgroundColor;
            _headerLabel.TextColor = HeaderTextColor;
            _headerLabel.Text = HeaderText;
            _headerLabel.TextAlignment = UITextAlignment.Center;
            _internalView.AddSubview(_headerLabel);

            if (!String.IsNullOrWhiteSpace(this.CancelButtonText))
            {
                _cancelButton = UIButton.FromType(UIButtonType.System);
                _cancelButton.SetTitleColor(HeaderTextColor, UIControlState.Normal);
                _cancelButton.BackgroundColor = UIColor.Clear;
                _cancelButton.SetTitle(CancelButtonText, UIControlState.Normal);
                _cancelButton.TouchUpInside += CancelButtonTapped;
                _internalView.AddSubview (_cancelButton);
            }

            _doneButton = UIButton.FromType(UIButtonType.System);
            _doneButton.SetTitleColor(HeaderTextColor, UIControlState.Normal);
            _doneButton.BackgroundColor = UIColor.Clear;
            _doneButton.SetTitle(DoneButtonText, UIControlState.Normal);
            _doneButton.TouchUpInside += DoneButtonTapped;


            _internalView.AddSubview(DatePicker);
            _internalView.BackgroundColor = HeaderBackgroundColor;
            _internalView.AddSubview(_doneButton);

            View.BackgroundColor = UIColor.Clear;
            DatePicker.BackgroundColor = UIColor.White;

            this.Add(_internalView);
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            Show();
        }


        void Show(bool onRotate = false)
        {
            var buttonSize = new CGSize(71, 30);
            var width = _parent.View.Frame.Width;
            var internalViewSize = new CGSize(width, DatePicker.Frame.Height + ToolbarHeight);
            var internalViewFrame = CGRect.Empty;

            if (InterfaceOrientation == UIInterfaceOrientation.Portrait)
            {
                if (onRotate)
                {
                    internalViewFrame = new CGRect(
                        0, 
                        View.Frame.Height - internalViewSize.Height,
                        internalViewSize.Width, 
                        internalViewSize.Height
                    );
                }
                else
                {
                    internalViewFrame = new CGRect(
                        0, 
                        View.Bounds.Height - internalViewSize.Height,
                        internalViewSize.Width, 
                        internalViewSize.Height
                    );
                }
            }
            else
            {
                if (onRotate)
                {
                    internalViewFrame = new CGRect(0, View.Bounds.Height - internalViewSize.Height,
                        internalViewSize.Width, internalViewSize.Height);
                }
                else
                {
                    internalViewFrame = new CGRect(0, View.Frame.Height - internalViewSize.Height,
                        internalViewSize.Width, internalViewSize.Height);
                }
            }
            _internalView.Frame = internalViewFrame;
            this.DatePicker.Frame = new CGRect(
                DatePicker.Frame.X, 
                ToolbarHeight, 
                _internalView.Frame.Width,
                DatePicker.Frame.Height
            );
                    
            _headerLabel.Frame = new CGRect(20 + buttonSize.Width, 4, _parent.View.Frame.Width - (40+2*buttonSize.Width), 35);
            _doneButton.Frame = new CGRect(internalViewFrame.Width - buttonSize.Width - 10, 7, buttonSize.Width, buttonSize.Height);
            _cancelButton.Frame = new CGRect(10, 7, buttonSize.Width, buttonSize.Height);
        }


        void DoneButtonTapped (object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
            this.Dismissed?.Invoke(this, true);
        }


        void CancelButtonTapped (object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
            this.Dismissed?.Invoke(this, false);
        }


        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);

            if (InterfaceOrientation == UIInterfaceOrientation.Portrait ||
                InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft ||
                InterfaceOrientation == UIInterfaceOrientation.LandscapeRight)
            {
                Show(true);
                View.SetNeedsDisplay();
            }
        }
    }
}