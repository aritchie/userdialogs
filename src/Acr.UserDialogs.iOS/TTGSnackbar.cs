using System;
using Foundation;
using UIKit;

namespace TTG
{
    

	public enum TTGSnackbarAnimationType
	{
		FadeInFadeOut,
		SlideFromBottomToTop,
		SlideFromBottomBackToBottom,
		SlideFromLeftToRight,
		SlideFromRightToLeft,
		Flip,
	}

	public class TTGSnackbar : UIView
	{

		/// Snackbar action button max width.
		private const float snackbarActionButtonMaxWidth = 64;

		// Snackbar action button min width.
		private const float snackbarActionButtonMinWidth = 44;

		public Action<TTGSnackbar> ActionBlock { get; set; }
		public Action<TTGSnackbar> SecondActionBlock { get; set; }

        /// <summary>
        /// Snackbar display duration. Default is 3 seconds.
        /// </summary>
	    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(3);
        public TTGSnackbarAnimationType AnimationType = TTGSnackbarAnimationType.SlideFromBottomBackToBottom;

		// Show and hide animation duration. Default is 0.3
        public float AnimationDuration { get; set; } = 0.3f;

		//private float _cornerRadius = 4;
		public nfloat CornerRadius
		{
			get { return this.Layer.CornerRadius; }
			set
			{
				var _cornerRadius = value;
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
		//private float _leftMargin = 4;
		public nfloat LeftMargin
		{
			get { return leftMarginConstraint.Constant; }
			set { 
                leftMarginConstraint.Constant = value; 
                this.LayoutIfNeeded(); 
            }
		}

		//nfloat _rightMargin = 4;
		public nfloat RightMargin
		{
			get { return rightMarginConstraint.Constant; }
			set { 
                rightMarginConstraint.Constant = value; 
                this.LayoutIfNeeded(); 
            }
		}

		/// Bottom margin. Default is 4
		public nfloat BottomMargin
		{
			get { return bottomMarginConstraint.Constant; }
			set { 
                bottomMarginConstraint.Constant = value; 
                this.LayoutIfNeeded(); 
            }
		}

		//private nfloat _height = 44;
		public nfloat Height
		{
            get { return heightConstraint.Constant; }
			set { 
                heightConstraint.Constant = value; 
                this.LayoutIfNeeded(); 
            }
		}


		public string Message
		{
            get { return MessageLabel.Text; }
            set { this.MessageLabel.Text = value; }
        }


        string actionText;
        public string ActionText {
            get { return actionText; }
            set {
                actionText = value;
                this.ActionButton.SetTitle(value, UIControlState.Normal);
            }
        }


        string secondActionText;
        public string SecondActionText {
            get { return secondActionText; }
            set {
                secondActionText = value;
                this.SecondActionButton.SetTitle (value, UIControlState.Normal);
            }
        }


        public UILabel MessageLabel { get; }
        public UIButton ActionButton { get; set; }
        public UIButton SecondActionButton { get; set; }

		private UIView seperateView;

		// Timer to dismiss the snackbar.
		private NSTimer dismissTimer;

		// Constraints.
		private NSLayoutConstraint heightConstraint;
		private NSLayoutConstraint leftMarginConstraint;
		private NSLayoutConstraint rightMarginConstraint;
		private NSLayoutConstraint bottomMarginConstraint;
		private NSLayoutConstraint actionButtonWidthConstraint;
		private NSLayoutConstraint secondActionButtonWidthConstraint;


		public TTGSnackbar() : base(CoreGraphics.CGRect.FromLTRB(0, 0, 320, 44))
		{
            this.TranslatesAutoresizingMaskIntoConstraints = false;
            this.BackgroundColor = UIColor.DarkGray;
            this.Layer.CornerRadius = CornerRadius;
            this.Layer.MasksToBounds = true;

            this.MessageLabel = new UILabel {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextColor = UIColor.White,
                Font = UIFont.BoldSystemFontOfSize (14),
                BackgroundColor = UIColor.Clear,
                LineBreakMode = UILineBreakMode.CharacterWrap,
                Lines = 2,
                TextAlignment = UITextAlignment.Left
            };

            this.AddSubview (this.MessageLabel);

            this.ActionButton = new UIButton {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Clear
            };
            this.ActionButton.TitleLabel.Font = UIFont.BoldSystemFontOfSize (14);
            this.ActionButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            this.ActionButton.SetTitleColor (UIColor.White, UIControlState.Normal);
            this.ActionButton.TouchUpInside += (s, e) => {
                // there is a chance that user doesn't want to do anything here, he simply wants to dismiss
                ActionBlock?.Invoke (this);
                Dismiss ();
            };

            this.AddSubview (this.ActionButton);

            this.SecondActionButton = new UIButton {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Clear
            };
            this.SecondActionButton.TitleLabel.Font = UIFont.BoldSystemFontOfSize (14);
            this.SecondActionButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            //this.SecondActionButton.SetTitle (SecondActionText, UIControlState.Normal);
            this.SecondActionButton.SetTitleColor (UIColor.White, UIControlState.Normal);
            this.SecondActionButton.TouchUpInside += (s, e) => {
                SecondActionBlock?.Invoke (this);
                Dismiss ();
            };

            this.AddSubview (this.SecondActionButton);

            this.seperateView = new UIView {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Gray
            };

            this.AddSubview(this.seperateView);

            // Add constraints
            var hConstraints = NSLayoutConstraint.FromVisualFormat (
                "H:|-4-[messageLabel]-2-[seperateView(0.5)]-2-[actionButton]-0-[secondActionButton]-4-|",
                0, new NSDictionary (),
                NSDictionary.FromObjectsAndKeys (
                    new NSObject [] {
                        MessageLabel,
                        seperateView,
                        ActionButton,
                        SecondActionButton
                }, new NSObject [] {
                    new NSString("messageLabel"),
                    new NSString("seperateView"),
                    new NSString("actionButton"),
                    new NSString("secondActionButton")
                })
            );

            var vConstraintsForMessageLabel = NSLayoutConstraint.FromVisualFormat (
                "V:|-0-[messageLabel]-0-|", 0, new NSDictionary (), NSDictionary.FromObjectsAndKeys (new NSObject [] { MessageLabel }, new NSObject [] { new NSString ("messageLabel") })
            );

            var vConstraintsForSeperateView = NSLayoutConstraint.FromVisualFormat (
                "V:|-4-[seperateView]-4-|", 0, new NSDictionary (), NSDictionary.FromObjectsAndKeys (new NSObject [] { seperateView }, new NSObject [] { new NSString ("seperateView") })
            );

            var vConstraintsForActionButton = NSLayoutConstraint.FromVisualFormat (
                "V:|-0-[actionButton]-0-|", 0, new NSDictionary (), NSDictionary.FromObjectsAndKeys (new NSObject [] { ActionButton }, new NSObject [] { new NSString ("actionButton") })
            );

            var vConstraintsForSecondActionButton = NSLayoutConstraint.FromVisualFormat (
                "V:|-0-[secondActionButton]-0-|", 0, new NSDictionary (), NSDictionary.FromObjectsAndKeys (new NSObject [] { SecondActionButton }, new NSObject [] { new NSString ("secondActionButton") })
            );

            actionButtonWidthConstraint = NSLayoutConstraint.Create (ActionButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, TTGSnackbar.snackbarActionButtonMinWidth);

            secondActionButtonWidthConstraint = NSLayoutConstraint.Create (SecondActionButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, TTGSnackbar.snackbarActionButtonMinWidth);

            //var vConstraintsForActivityIndicatorView = NSLayoutConstraint.FromVisualFormat(
            //"V:|-2-[activityIndicatorView]-2-|", 0,new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { activityIndicatorView }, new NSObject[] { new NSString("activityIndicatorView") })
            //);

            //todo fix constraint
            //var hConstraintsForActivityIndicatorView = NSLayoutConstraint.FromVisualFormat(
            //  //"H:[activityIndicatorView(activityIndicatorWidth)]-2-|",
            //  "H:[activityIndicatorView]-2-|",
            //  0,
            //  new NSDictionary(),
            //  NSDictionary.FromObjectsAndKeys(
            //      new NSObject[] {  activityIndicatorView },
            //                 new NSObject[] {  new NSString("activityIndicatorView") })
            //  //NSDictionary.FromObjectsAndKeys(new NSObject[] { activityIndicatorView }, new NSObject[] {  })
            //);

            ActionButton.AddConstraint (actionButtonWidthConstraint);
            SecondActionButton.AddConstraint (secondActionButtonWidthConstraint);

            this.AddConstraints (hConstraints);
            this.AddConstraints (vConstraintsForMessageLabel);
            this.AddConstraints (vConstraintsForSeperateView);
            this.AddConstraints (vConstraintsForActionButton);
            this.AddConstraints (vConstraintsForSecondActionButton);
            //this.AddConstraints(vConstraintsForActivityIndicatorView);
            //this.AddConstraints(hConstraintsForActivityIndicatorView);
        }



		/**
		Show the snackbar.
		*/
		public void Show()
		{
			if (this.Superview != null)
				return;

			dismissTimer = NSTimer.CreateScheduledTimer(this.Duration, x => Dismiss());

            ActionButton.Hidden = this.ActionBlock == null;
            SecondActionButton.Hidden = this.SecondActionBlock == null;
			seperateView.Hidden = ActionButton.Hidden;

			actionButtonWidthConstraint.Constant = ActionButton.Hidden ? 0 : (SecondActionButton.Hidden ? TTGSnackbar.snackbarActionButtonMaxWidth : TTGSnackbar.snackbarActionButtonMinWidth);
			secondActionButtonWidthConstraint.Constant = SecondActionButton.Hidden ? 0 : (ActionButton.Hidden ? TTGSnackbar.snackbarActionButtonMaxWidth : TTGSnackbar.snackbarActionButtonMinWidth);

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
				showWithAnimation();
			}
			else
			{
				Console.WriteLine("TTGSnackbar needs a keyWindows to display.");
			}
		}


		/**
		Dismiss.

		- parameter animated: If dismiss with animation.
		*/
		public void Dismiss(bool animated = true)
		{
			dismissTimer?.Invalidate();
			dismissTimer = null;

			//activityIndicatorView.StopAnimating();

			nfloat superViewWidth = 0;

			if(Superview != null)
				superViewWidth = Superview.Frame.Width;

			if (!animated)
			{
				this.RemoveFromSuperview();
				return;
			}

			Action animationBlock = () => { };

			switch (AnimationType)
			{
				case TTGSnackbarAnimationType.FadeInFadeOut:
					animationBlock = () => { this.Alpha = 0; };
					break;
				case TTGSnackbarAnimationType.SlideFromBottomBackToBottom:
					animationBlock = () => { bottomMarginConstraint.Constant = Height; };
					break;
				case TTGSnackbarAnimationType.SlideFromBottomToTop:
					animationBlock = () => { this.Alpha = 0; bottomMarginConstraint.Constant = -Height - BottomMargin; };
					break;
				case TTGSnackbarAnimationType.SlideFromLeftToRight:
					animationBlock = () => { leftMarginConstraint.Constant = LeftMargin + superViewWidth; rightMarginConstraint.Constant = -RightMargin + superViewWidth; };
					break;
				case TTGSnackbarAnimationType.SlideFromRightToLeft:
					animationBlock = () =>
					{
						leftMarginConstraint.Constant = LeftMargin - superViewWidth;
						rightMarginConstraint.Constant = -RightMargin - superViewWidth;
					};
					break;
				case TTGSnackbarAnimationType.Flip:
					//todo animationBlock = () => { this.Layer.Transform = CAT(CGFloat(M_PI_2), 1, 0, 0);}
					break;
			};

			this.SetNeedsLayout();

			UIView.Animate(
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
		private void showWithAnimation()
		{
			Action animationBlock = () => { this.LayoutIfNeeded(); };
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
				case TTGSnackbarAnimationType.Flip:
					//todo animationBlock = () => { this.Layer.Transform = CAT(CGFloat(M_PI_2), 1, 0, 0);}
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
	}
}


