using System;
using Acr.UserDialogs;
using Foundation;
using UIKit;

namespace AI
{
    [Register ("AIDatePickerController")]
	public class AIDatePickerController : UIViewController, IUIViewControllerAnimatedTransitioning, IUIViewControllerTransitioningDelegate
	{
        private UIEdgeInsets safeAreaInsets;

        public double AnimatedTransitionDuration { get; set; } = 0.4;
#if __IOS__
		public UIDatePickerMode Mode { get; set; } = UIDatePickerMode.Date;
#endif
		public iOSPickerStyle PickerStyle { get; set; }
		public UIColor BackgroundColor { get; set; }
        public DateTime SelectedDateTime { get; set; } = DateTime.Now;
        public DateTime? MaximumDateTime { get; set; }
        public DateTime? MinimumDateTime { get; set; }
	    public int MinuteInterval { get; set; } = 1;
        public string OkText { get; set; }
        public bool? Use24HourClock { get; set; }
        public Action<AIDatePickerController> Ok { get; set; }
        public string CancelText { get; set; }
        public Action<AIDatePickerController> Cancel { get; set; }

	    public float FontSize { get; set; } = 16;
		public NSDateFormatter DateFormatter { get; set; } = new NSDateFormatter();

	    UIView dimmedView;


        public AIDatePickerController()
        {
            SetTheme();
            //this.ModalPresentationStyle = UIModalPresentationStyle.Custom;
            this.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            this.TransitioningDelegate = this;
            this.TransitioningDelegate = this;

            this.SetupSafeAreaInsets();
        }


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            this.View.BackgroundColor = UIColor.Clear;
#if __IOS__
			var datePicker = new UIDatePicker
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Date = this.SelectedDateTime.ToNSDate(),
				BackgroundColor = BackgroundColor,
				Mode = Mode,
                MinuteInterval = MinuteInterval
			};

			SetPreferredDatePickerStyle(ref datePicker,PickerStyle);

            if (Use24HourClock == true)
                datePicker.Locale = NSLocale.FromLocaleIdentifier("NL");

		    if (MinimumDateTime != null)
		        datePicker.MinimumDate = MinimumDateTime.Value.ToNSDate();

		    if (MaximumDateTime != null)
		        datePicker.MaximumDate = MaximumDateTime.Value.ToNSDate();
#elif __TVOS__
            var datePicker = new UIControl();
            //TODO: Fake Date picker on tvOS
#endif
            dimmedView = new UIView(this.View.Bounds)
			{
			    AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
                TintAdjustmentMode = UIViewTintAdjustmentMode.Dimmed,
                BackgroundColor = UIColor.Black,
                Alpha = 0.7F
			};


			var dismissButton = new UIButton
			{
			    TranslatesAutoresizingMaskIntoConstraints = false,
                UserInteractionEnabled = true
			};
			dismissButton.TouchUpInside += async (s, e) =>
			{
                await this.DismissViewControllerAsync(true);
				this.Cancel?.Invoke(this);
			};
			this.View.AddSubview(dismissButton);

			var containerView = new UIView
			{
                ClipsToBounds = true,
                BackgroundColor = BackgroundColor,
			    TranslatesAutoresizingMaskIntoConstraints = false
			};
			containerView.Layer.CornerRadius = 5.0f;
			this.View.AddSubview(containerView);

			containerView.AddSubview(datePicker);

			var buttonContainerView = new UIView
			{
			    TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = BackgroundColor
			};
			buttonContainerView.Layer.CornerRadius = 5.0f;
			this.View.AddSubview(buttonContainerView);

			var buttonDividerView = new UIView
			{
			    TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.FromRGBA(205 / 255, 205 / 255, 205 / 255, 1)
			};
			this.View.AddSubview(buttonDividerView);

			var cancelButton = new UIButton();
			cancelButton.TranslatesAutoresizingMaskIntoConstraints = false;
			cancelButton.SetTitle(this.CancelText, UIControlState.Normal);
			cancelButton.SetTitleColor(UIColor.Red, UIControlState.Normal);

			cancelButton.TitleLabel.Font = UIFont.SystemFontOfSize(this.FontSize);
			cancelButton.TouchUpInside += async (s, e) =>
			{
                await this.DismissViewControllerAsync(true);
				this.Cancel?.Invoke(this);
			};
			buttonContainerView.AddSubview(cancelButton);

			var button = new UIButton(UIButtonType.System);
			button.TranslatesAutoresizingMaskIntoConstraints = false;
            button.TitleLabel.Font = UIFont.BoldSystemFontOfSize(this.FontSize);
			button.SetTitle(this.OkText, UIControlState.Normal);
			button.TouchUpInside += async (s, e) =>
			{
#if __IOS__
                this.SelectedDateTime = datePicker.Date.ToDateTime();
#endif
                await this.DismissViewControllerAsync (true);
				Ok?.Invoke(this);
			};
			buttonContainerView.AddSubview(button);

			var views = NSDictionary.FromObjectsAndKeys(
				new NSObject[]
                {
                    dismissButton,
                    containerView,
                    datePicker,
                    buttonContainerView,
                    buttonDividerView,
                    cancelButton,
                    button
                },
				new NSObject[]
                {
					new NSString("DismissButton"),
                    new NSString("DatePickerContainerView"),
                    new NSString("datePicker"),
					new NSString("ButtonContainerView"),
                    new NSString("ButtonDividerView"),
                    new NSString("CancelButton"),
					new NSString("SelectButton")
				}
			);

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[CancelButton][ButtonDividerView(0.5)][SelectButton(CancelButton)]|", 0, null, views));

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[CancelButton]|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[ButtonDividerView]|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[SelectButton]|", 0, null, views));

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[datePicker]|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[datePicker]|", 0, null, views));

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[DismissButton]|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-5-[DatePickerContainerView]-5-|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-5-[ButtonContainerView]-5-|", 0, null, views));

            this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat($"V:|[DismissButton][DatePickerContainerView]-10-[ButtonContainerView(40)]-{5 + safeAreaInsets.Bottom}-|", 0, null, views));
		}

        private void SetupSafeAreaInsets()
        {
            if(UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                safeAreaInsets = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;
            }
            else
            {
                safeAreaInsets = new UIEdgeInsets();
            }
        }

		public double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
		{
			return AnimatedTransitionDuration;
		}

		public void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
		{
			var fromViewController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
			var toViewController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
			var containerView = transitionContext.ContainerView;

			//if we are presenting
			if (toViewController.View == this.View)
			{
				fromViewController.View.UserInteractionEnabled = false;

				containerView.AddSubview(dimmedView);
				containerView.AddSubview(toViewController.View);

				var frame = toViewController.View.Frame;
				frame.Y = toViewController.View.Bounds.Height;
				toViewController.View.Frame = frame;

				this.dimmedView.Alpha = 0f;

				UIView.Animate(AnimatedTransitionDuration, 0, UIViewAnimationOptions.CurveEaseIn,
				   () =>
					{
						this.dimmedView.Alpha = 0.7f;
						frame = toViewController.View.Frame;
						frame.Y = 0f;
						toViewController.View.Frame = frame;
					},
				   () =>
					{
						transitionContext.CompleteTransition(!transitionContext.TransitionWasCancelled);
					}
				  );

			}
			else
			{
				toViewController.View.UserInteractionEnabled = true;
				UIView.Animate(AnimatedTransitionDuration, 0.1f, UIViewAnimationOptions.CurveEaseIn,
				   () =>
					{
						this.dimmedView.Alpha = 0f;
						var frame = fromViewController.View.Frame;
						frame.Y = fromViewController.View.Bounds.Height;
						fromViewController.View.Frame = frame;
					},
				   () =>
					{
						transitionContext.CompleteTransition(!transitionContext.TransitionWasCancelled);
					}
				  );
			}
		}

		[Export("animationControllerForPresentedController:presentingController:sourceController:")]
		public IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
		{
			return this;
		}

		[Export("animationControllerForDismissedController:")]
		public IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
		{
			return this;
		}

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);

            if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0) && previousTraitCollection != null)
            {
                if (this.TraitCollection.UserInterfaceStyle != previousTraitCollection.UserInterfaceStyle)
                {
                    SetTheme();
                }
            }
        }

        private void SetTheme()
        {
            this.BackgroundColor = GetBackgroundColor();
        }

        private UIColor GetBackgroundColor()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
            {
                if (this.TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
                {
                    return UIColor.Black;
                }
#if __IOS__
                else if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    return UIColor.TertiarySystemBackgroundColor;
                }
#endif
                else
                {
                    return UIColor.White;
                }
            }
            else
            {
                return UIColor.White;
            }
        }

#if __IOS__
		private void SetPreferredDatePickerStyle(ref UIDatePicker datePicker, iOSPickerStyle? style)
        {
			if (!UIDevice.CurrentDevice.CheckSystemVersion(13, 4) ||
				datePicker == null ||
				!style.HasValue)
			{
				return;
			}

			if (UIDevice.CurrentDevice.CheckSystemVersion(14, 0) && style.Value == iOSPickerStyle.Compact)
			{
				datePicker.PreferredDatePickerStyle = UIDatePickerStyle.Compact;
				return;
			}

			switch (style.Value)
            {
				case iOSPickerStyle.Auto: datePicker.PreferredDatePickerStyle = UIDatePickerStyle.Automatic; return;
				case iOSPickerStyle.Inline: datePicker.PreferredDatePickerStyle = UIDatePickerStyle.Inline; return;
				case iOSPickerStyle.Wheels: datePicker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels; return;
			}

			datePicker.PreferredDatePickerStyle = UIDatePickerStyle.Automatic;
		}
#endif
	}
}


