using System;
using Acr.UserDialogs;
using Foundation;
using UIKit;

namespace AI
{
    using System.Collections.Generic;

    [Register ("AIPickerController")]
	public class AIPickerController : UIViewController, IUIViewControllerAnimatedTransitioning, IUIViewControllerTransitioningDelegate
	{
        public double AnimatedTransitionDuration { get; set; } = 0.4;
	    public UIColor BackgroundColor { get; set; } = UIColor.White;
	    public PickerPromptConfig config { get; set; }
        public string OkText { get; set; }
        public Action<AIPickerController> Ok { get; set; }
        public string CancelText { get; set; }
        public Action<AIPickerController> Cancel { get; set; }
        public IList<int> SelectedItems { get; set; } = new List<int>();

	    public float FontSize { get; set; } = 16;
		public NSDateFormatter DateFormatter { get; set; } = new NSDateFormatter();

	    UIView dimmedView;


        public AIPickerController(PickerPromptConfig ppc) 
        {
            this.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            this.TransitioningDelegate = this;
            config = ppc;
        }


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            this.View.BackgroundColor = UIColor.Clear;

			var picker = new UIPickerView()
			{
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = BackgroundColor,
                Model = new PickerViewModel(config)
			};

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
                SelectedItems = ((PickerViewModel)picker.Model).SelectedItems;
                await this.DismissViewControllerAsync(true);
				this.Cancel?.Invoke(this);
			};
			this.View.AddSubview(dismissButton);

			var containerView = new UIView
			{
                ClipsToBounds = true,
                BackgroundColor = UIColor.White,
			    TranslatesAutoresizingMaskIntoConstraints = false
			};
			containerView.Layer.CornerRadius = 5.0f;
			this.View.AddSubview(containerView);

			containerView.AddSubview(picker);

		    for (int i = 0; i < config.SelectedItemIndex.Count; i++)
		    {
		        var v = config.SelectedItemIndex[i];

                picker.Select(v, i, false);
		        SelectedItems.Add(v);
		    }
		    ((PickerViewModel) picker.Model).SelectedItems = SelectedItems;

		    var buttonContainerView = new UIView
			{
			    TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White
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
                SelectedItems = ((PickerViewModel)picker.Model).SelectedItems;
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
			    SelectedItems = ((PickerViewModel)picker.Model).SelectedItems;
                await this.DismissViewControllerAsync (true);
				Ok?.Invoke(this);
			};
			buttonContainerView.AddSubview(button);

			var views = NSDictionary.FromObjectsAndKeys(
				new NSObject[] 
                { 
                    dismissButton, 
                    containerView, 
                    picker, 
                    buttonContainerView, 
                    buttonDividerView, 
                    cancelButton, 
                    button 
                },
				new NSObject[] 
                {
					new NSString("DismissButton"),
                    new NSString("PickerContainerView"),
                    new NSString("Picker"),
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

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[Picker]|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[Picker]|", 0, null, views));

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[DismissButton]|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-5-[PickerContainerView]-5-|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-5-[ButtonContainerView]-5-|", 0, null, views));

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[DismissButton][PickerContainerView]-10-[ButtonContainerView(40)]-5-|", 0, null, views));
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

        public class PickerViewModel : UIPickerViewModel
        {
            private IList<IList<string>> _collections;
            public IList<int> SelectedItems;
            public PickerViewModel(PickerPromptConfig ppc)
            {
                _collections = ppc.PickerCollections;
            }
            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {
                return _collections[(int)component].Count;
            }

            public override string GetTitle(UIPickerView pickerView, nint row, nint component)
            {
                return _collections[(int)component][(int)row];
            }

            public override nint GetComponentCount(UIPickerView pickerView)
            {

                return _collections.Count;
            }

            public override void Selected(UIPickerView picker, nint row, nint component)
            {
                SelectedItems[(int)component] = (int)row;
            }
        }
    }

}


