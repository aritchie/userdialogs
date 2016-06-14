using System;
using CoreGraphics;
using Foundation;
using UIKit;


namespace Acr.UserDialogs
{

    public enum TTGSnackbarAnimationType
    {
        FadeInFadeOut,
        SlideFromBottomToTop,
        SlideFromBottomBackToBottom,
        SlideFromLeftToRight,
        SlideFromRightToLeft
    }

    public class TTGSnackbar : UIView
    {

        public TTGSnackbar() : base(CGRect.FromLTRB(0, 0, 320, 44))
        {
            this.TranslatesAutoresizingMaskIntoConstraints = false;
            this.BackgroundColor = UIColor.DarkGray;
            this.Layer.MasksToBounds = true;

            messageLabel = new UILabel();
            messageLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            messageLabel.TextColor = UIColor.White;
            messageLabel.Font = MessageTextFont;
            messageLabel.BackgroundColor = UIColor.Clear;
            messageLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
            messageLabel.Lines = 2;
            messageLabel.TextAlignment = UITextAlignment.Left;
            messageLabel.Text = Message;

            this.AddSubview(messageLabel);

            actionButton = new UIButton
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Clear
            };
            actionButton.TitleLabel.Font = ActionTextFont;
            actionButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            actionButton.SetTitle(ActionText, UIControlState.Normal);
            actionButton.SetTitleColor(ActionTextColor, UIControlState.Normal);
            actionButton.TouchUpInside += (s, e) => this.DoAction(actionButton);
            this.AddSubview(actionButton);

            secondActionButton = new UIButton
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Clear
            };
            secondActionButton.TitleLabel.Font = SecondActionTextFont;
            secondActionButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            secondActionButton.SetTitle(SecondActionText, UIControlState.Normal);
            secondActionButton.SetTitleColor(SecondActionTextColor, UIControlState.Normal);
            secondActionButton.TouchUpInside += (s, e) => this.DoAction(secondActionButton);
            this.AddSubview(secondActionButton);

            seperateView = new UIView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Gray
            };
            this.AddSubview(seperateView);

            activityIndicatorView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
            activityIndicatorView.TranslatesAutoresizingMaskIntoConstraints = false;
            activityIndicatorView.StopAnimating();

            this.AddSubview(activityIndicatorView);

            // Add constraints
            var hConstraints = NSLayoutConstraint.FromVisualFormat(
                "H:|-4-[messageLabel]-2-[seperateView(0.5)]-2-[actionButton]-0-[secondActionButton]-4-|",
                0, new NSDictionary(),
                NSDictionary.FromObjectsAndKeys(
                    new NSObject[] {
                        messageLabel,
                        seperateView,
                        actionButton,
                        secondActionButton
                }, new NSObject[] {
                    new NSString("messageLabel"),
                    new NSString("seperateView"),
                    new NSString("actionButton"),
                    new NSString("secondActionButton")
                })
            );

            var vConstraintsForMessageLabel = NSLayoutConstraint.FromVisualFormat(
                "V:|-0-[messageLabel]-0-|", 0, new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { messageLabel }, new NSObject[] { new NSString("messageLabel") })
            );

            var vConstraintsForSeperateView = NSLayoutConstraint.FromVisualFormat(
                "V:|-4-[seperateView]-4-|", 0, new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { seperateView }, new NSObject[] { new NSString("seperateView") })
            );

            var vConstraintsForActionButton = NSLayoutConstraint.FromVisualFormat(
                "V:|-0-[actionButton]-0-|", 0, new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { actionButton }, new NSObject[] { new NSString("actionButton") })
            );

            var vConstraintsForSecondActionButton = NSLayoutConstraint.FromVisualFormat(
                "V:|-0-[secondActionButton]-0-|", 0, new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { secondActionButton }, new NSObject[] { new NSString("secondActionButton") })
            );

            actionButtonWidthConstraint = NSLayoutConstraint.Create(actionButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, TTGSnackbar.snackbarActionButtonMinWidth);

            secondActionButtonWidthConstraint = NSLayoutConstraint.Create(secondActionButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, TTGSnackbar.snackbarActionButtonMinWidth);

            var vConstraintsForActivityIndicatorView = NSLayoutConstraint.FromVisualFormat(
                "V:|-2-[activityIndicatorView]-2-|", 0, new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { activityIndicatorView }, new NSObject[] { new NSString("activityIndicatorView") })
            );

            //todo fix constraint
            var hConstraintsForActivityIndicatorView = NSLayoutConstraint.FromVisualFormat(
                //"H:[activityIndicatorView(activityIndicatorWidth)]-2-|",
                "H:[activityIndicatorView]-2-|",
                0,
                new NSDictionary(),
                NSDictionary.FromObjectsAndKeys(
                    new NSObject[] { activityIndicatorView },
                    new NSObject[] { new NSString("activityIndicatorView") })
            //NSDictionary.FromObjectsAndKeys(new NSObject[] { activityIndicatorView }, new NSObject[] {  })
            );

            actionButton.AddConstraint(actionButtonWidthConstraint);
            secondActionButton.AddConstraint(secondActionButtonWidthConstraint);

            this.AddConstraints(hConstraints);
            this.AddConstraints(vConstraintsForMessageLabel);
            this.AddConstraints(vConstraintsForSeperateView);
            this.AddConstraints(vConstraintsForActionButton);
            this.AddConstraints(vConstraintsForSecondActionButton);
            this.AddConstraints(vConstraintsForActivityIndicatorView);
            this.AddConstraints(hConstraintsForActivityIndicatorView);
        }


        /// Snackbar action button max width.
        const float snackbarActionButtonMaxWidth = 64;

        // Snackbar action button min width.
        const float snackbarActionButtonMinWidth = 44;

        public Action<TTGSnackbar> ActionBlock { get; set; }
        public Action<TTGSnackbar> SecondActionBlock { get; set; }
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(3);
        public TTGSnackbarAnimationType AnimationType { get; set; } = TTGSnackbarAnimationType.SlideFromBottomBackToBottom;
        public float AnimationDuration { get; set; } = 0.3f;


        float _cornerRadius = 4;
        public float CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = value;
                if (_cornerRadius > Height)
                {
                    _cornerRadius = Height / 2;
                }
                if (_cornerRadius < 0)
                    _cornerRadius = 0;

                this.Layer.CornerRadius = _cornerRadius;
                this.Layer.MasksToBounds = true;
            }
        }

        /// Left margin. Default is 4
        private float _leftMargin = 4;
        public float LeftMargin
        {
            get { return _leftMargin; }
            set { _leftMargin = value; leftMarginConstraint.Constant = _leftMargin; this.LayoutIfNeeded(); }
        }

        private float _rightMargin = 4;
        public float RightMargin
        {
            get { return _rightMargin; }
            set { _rightMargin = value; rightMarginConstraint.Constant = _leftMargin; this.LayoutIfNeeded(); }
        }

        /// Bottom margin. Default is 4
        private float _bottomMargin = 4;
        public float BottomMargin
        {
            get { return _bottomMargin; }
            set { _bottomMargin = value; bottomMarginConstraint.Constant = _bottomMargin; this.LayoutIfNeeded(); }
        }

        private float _height = 44;
        public float Height
        {
            get { return _height; }
            set { _height = value; heightConstraint.Constant = _height; this.LayoutIfNeeded(); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; if (this.messageLabel != null) { this.messageLabel.Text = _message; } }
        }

        private UIColor _messageTextColor = UIColor.White;
        public UIColor MessageTextColor
        {
            get { return _messageTextColor; }
            set { _messageTextColor = value; this.messageLabel.TextColor = _messageTextColor; }
        }

        private UIFont _messageTextFont = UIFont.BoldSystemFontOfSize(14);
        public UIFont MessageTextFont
        {
            get { return _messageTextFont; }
            set { _messageTextFont = value; this.messageLabel.Font = _messageTextFont; }
        }

        private UITextAlignment _messageTextAlign;
        public UITextAlignment MessageTextAlign
        {
            get { return _messageTextAlign; }
            set { _messageTextAlign = value; this.messageLabel.TextAlignment = _messageTextAlign; }
        }

        private string _actionText;
        public string ActionText
        {
            get { return _actionText; }
            set { _actionText = value; if (this.actionButton != null) { this.actionButton.SetTitle(_actionText, UIControlState.Normal); } }
        }

        private string _secondActionText;
        public string SecondActionText
        {
            get { return _secondActionText; }
            set { _secondActionText = value; if (this.secondActionButton != null) { this.secondActionButton.SetTitle(_secondActionText, UIControlState.Normal); } }
        }

        // Action button title color. Default is white.
        private UIColor _actionTextColor = UIColor.White;
        public UIColor ActionTextColor
        {
            get { return _actionTextColor; }
            set { _actionTextColor = value; this.actionButton.SetTitleColor(_actionTextColor, UIControlState.Normal); }
        }

        // Second action button title color. Default is white.
        private UIColor _secondActionTextColor = UIColor.White;
        public UIColor SecondActionTextColor
        {
            get { return _secondActionTextColor; }
            set { _secondActionTextColor = value; this.secondActionButton.SetTitleColor(_secondActionTextColor, UIControlState.Normal); }
        }

        // First action text font. Default is Bold system font (14).
        private UIFont _actionTextFont = UIFont.BoldSystemFontOfSize(14);
        public UIFont ActionTextFont
        {
            get { return _actionTextFont; }
            set { _actionTextFont = value; this.actionButton.TitleLabel.Font = _actionTextFont; }
        }

        // First action text font. Default is Bold system font (14).
        private UIFont _secondActionTextFont = UIFont.BoldSystemFontOfSize(14);
        public UIFont SecondActionTextFont
        {
            get { return _secondActionTextFont; }
            set { _secondActionTextFont = value; this.secondActionButton.TitleLabel.Font = _secondActionTextFont; }
        }

        readonly UILabel messageLabel;
        readonly UIView seperateView;
        readonly UIButton actionButton;
        readonly UIButton secondActionButton;
        readonly UIActivityIndicatorView activityIndicatorView;

        NSTimer dismissTimer;

        // Constraints.
        NSLayoutConstraint heightConstraint;
        NSLayoutConstraint leftMarginConstraint;
        NSLayoutConstraint rightMarginConstraint;
        NSLayoutConstraint bottomMarginConstraint;
        readonly NSLayoutConstraint actionButtonWidthConstraint;
        readonly NSLayoutConstraint secondActionButtonWidthConstraint;




        /**
		Show the snackbar.
		*/
        public void Show()
        {
            // Only show once
            if (this.Superview != null)
                return;

            // Create dismiss timer
            dismissTimer = NSTimer.CreateScheduledTimer(Duration.TotalMilliseconds, x => this.Dismiss());

            // Show or hide action button

            if (ActionBlock == null)
            {
                ActionText = String.Empty;
                actionButton.Hidden = true;
            }

            if (secondActionButton == null)
            {
                SecondActionText = String.Empty;
                secondActionButton.Hidden = true;
            }

            seperateView.Hidden = actionButton.Hidden;

            actionButtonWidthConstraint.Constant = actionButton.Hidden ? 0 : (secondActionButton.Hidden ? TTGSnackbar.snackbarActionButtonMaxWidth : TTGSnackbar.snackbarActionButtonMinWidth);
            secondActionButtonWidthConstraint.Constant = secondActionButton.Hidden ? 0 : (actionButton.Hidden ? TTGSnackbar.snackbarActionButtonMaxWidth : TTGSnackbar.snackbarActionButtonMinWidth);

            this.LayoutIfNeeded();

            var localSuperView = UIApplication.SharedApplication.KeyWindow;
            if (localSuperView != null)
            {
                localSuperView.AddSubview(this);

                heightConstraint = NSLayoutConstraint.Create(
                    this,
                    NSLayoutAttribute.Height,
                    NSLayoutRelation.Equal,
                    null,
                    NSLayoutAttribute.NoAttribute,
                    1,
                    Height);

                leftMarginConstraint = NSLayoutConstraint.Create(
                    this,
                    NSLayoutAttribute.Left,
                    NSLayoutRelation.Equal,
                    localSuperView,
                    NSLayoutAttribute.Left,
                    1,
                    LeftMargin);

                rightMarginConstraint = NSLayoutConstraint.Create(
                    this,
                    NSLayoutAttribute.Right,
                    NSLayoutRelation.Equal,
                    localSuperView,
                    NSLayoutAttribute.Right,
                    1,
                    -RightMargin);

                bottomMarginConstraint = NSLayoutConstraint.Create(
                    this,
                    NSLayoutAttribute.Bottom,
                    NSLayoutRelation.Equal,
                    localSuperView,
                    NSLayoutAttribute.Bottom,
                    1,
                    -BottomMargin);

                // Avoid the "UIView-Encapsulated-Layout-Height" constraint conflicts
                // http://stackoverflow.com/questions/25059443/what-is-nslayoutconstraint-uiview-encapsulated-layout-height-and-how-should-i
                leftMarginConstraint.Priority = 999;
                rightMarginConstraint.Priority = 999;

                this.AddConstraint(heightConstraint);
                localSuperView.AddConstraint(leftMarginConstraint);
                localSuperView.AddConstraint(rightMarginConstraint);
                localSuperView.AddConstraint(bottomMarginConstraint);

                // Show
                this.ShowWithAnimation();
            }
            else
            {
                Console.WriteLine("TTGSnackbar needs a keyWindows to display.");
            }
        }


        public void Dismiss(bool animated = true)
        {
            dismissTimer?.Invalidate();
            dismissTimer = null;

            activityIndicatorView.StopAnimating();

            nfloat superViewWidth = 0;

            if (Superview != null)
                superViewWidth = Superview.Frame.Width;

            //if (!animated)
            //{
            //    this.ActionBlock?.Invoke(this);
            //    this.RemoveFromSuperview();
            //    return;
            //}

            Action animationBlock = null;

            switch (AnimationType)
            {
                case TTGSnackbarAnimationType.FadeInFadeOut:
                    animationBlock = () => this.Alpha = 0;
                    break;

                case TTGSnackbarAnimationType.SlideFromBottomBackToBottom:
                    animationBlock = () => bottomMarginConstraint.Constant = Height;
                    break;

                case TTGSnackbarAnimationType.SlideFromBottomToTop:
                    animationBlock = () =>
                    {
                        this.Alpha = 0;
                        this.bottomMarginConstraint.Constant = -Height - BottomMargin;
                    };
                    break;

                case TTGSnackbarAnimationType.SlideFromLeftToRight:
                    animationBlock = () =>
                    {
                        this.leftMarginConstraint.Constant = LeftMargin + superViewWidth;
                        this.rightMarginConstraint.Constant = -RightMargin + superViewWidth;
                    };
                    break;

                case TTGSnackbarAnimationType.SlideFromRightToLeft:
                    animationBlock = () =>
                    {
                        this.leftMarginConstraint.Constant = LeftMargin - superViewWidth;
                        this.rightMarginConstraint.Constant = -RightMargin - superViewWidth;
                    };
                    break;
            };

            this.SetNeedsLayout();

            UIView.Animate (
                AnimationDuration,
                0,
                UIViewAnimationOptions.CurveEaseIn,
                animationBlock,
                this.RemoveFromSuperview
            );
        }



        /**
		 * Show.
		*/
        void ShowWithAnimation()
        {
            Action animationBlock = this.LayoutIfNeeded;
            var superViewWidth = Superview.Frame.Width;

            switch (AnimationType)
            {
                case TTGSnackbarAnimationType.FadeInFadeOut:
                    this.Alpha = 0;
                    this.SetNeedsLayout();
                    animationBlock = () => { this.Alpha = 1; };
                    break;

                case TTGSnackbarAnimationType.SlideFromBottomBackToBottom:
                case TTGSnackbarAnimationType.SlideFromBottomToTop:
                    bottomMarginConstraint.Constant = -BottomMargin;
                    this.LayoutIfNeeded();
                    break;

                case TTGSnackbarAnimationType.SlideFromLeftToRight:
                    leftMarginConstraint.Constant = LeftMargin - superViewWidth;
                    rightMarginConstraint.Constant = -RightMargin - superViewWidth;
                    bottomMarginConstraint.Constant = -BottomMargin;
                    this.LayoutIfNeeded();
                    break;

                case TTGSnackbarAnimationType.SlideFromRightToLeft:
                    leftMarginConstraint.Constant = LeftMargin + superViewWidth;
                    rightMarginConstraint.Constant = -RightMargin + superViewWidth;
                    bottomMarginConstraint.Constant = -BottomMargin;
                    this.LayoutIfNeeded();
                    break;
            };

            // Final state
            bottomMarginConstraint.Constant = -BottomMargin;
            leftMarginConstraint.Constant = LeftMargin;
            rightMarginConstraint.Constant = -RightMargin;

            UIView.AnimateNotify(
                    AnimationDuration,
                    0,
                    0.7f,
                    5f,
                    UIViewAnimationOptions.CurveEaseInOut,
                      animationBlock,
                    null
                );
        }


        void DoAction(UIButton button)
        {
            if (button.Equals(actionButton))
            {
                ActionBlock?.Invoke(this);
            }
            else if (button.Equals(secondActionButton))
            {
                SecondActionBlock?.Invoke(this);
            }

            this.Dismiss(true);
            //if (!this.actionButton.Hidden)
            //{
            //    actionButton.Hidden = true;
            //    secondActionButton.Hidden = true;

            //    seperateView.Hidden = true;

            //    activityIndicatorView.Hidden = false;

            //    activityIndicatorView.StartAnimating();
            //}
            //else
            //{
            //    this.Dismiss();
            //}
        }
    }
}
