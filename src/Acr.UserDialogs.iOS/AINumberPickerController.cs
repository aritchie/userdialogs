using System;
using Acr.UserDialogs;
using Foundation;
using UIKit;

namespace AI
{
    [Register("AINumberPickerController")]
    public class AINumberPickerController : UIViewController, IUIViewControllerAnimatedTransitioning, IUIViewControllerTransitioningDelegate
    {
        public double AnimatedTransitionDuration { get; set; } = 0.4;
        public UIColor BackgroundColor { get; set; } = UIColor.White;
        public int SelectedNumber { get; set; } = 0;
        public int? MinNumber { get; set; } = 0;
        public int? MaxNumber { get; set; } = 10;

        public string OkText { get; set; }

        public Action<AINumberPickerController> Ok { get; set; }
        public string CancelText { get; set; }
        public Action<AINumberPickerController> Cancel { get; set; }

        public float FontSize { get; set; } = 16;

        UIView dimmedView;


        public AINumberPickerController()
        {
            this.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            this.TransitioningDelegate = this;
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.BackgroundColor = UIColor.Clear;

            //picker
            UIPickerView pickerView = new UIPickerView{
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = BackgroundColor,
            };

            NumberModelPicker model = new NumberModelPicker(MinNumber.Value, MaxNumber.Value, SelectedNumber);
            pickerView.Model = model;

            pickerView.Select(model.SelectedIndex, 0, true);

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
                BackgroundColor = UIColor.White,
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            containerView.Layer.CornerRadius = 5.0f;
            this.View.AddSubview(containerView);

            containerView.AddSubview(pickerView);

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
                this.SelectedNumber = model.SelectedItem;
                await this.DismissViewControllerAsync(true);
                Ok?.Invoke(this);
            };
            buttonContainerView.AddSubview(button);

            var views = NSDictionary.FromObjectsAndKeys(
                new NSObject[]
                {
                    dismissButton,
                    containerView,
                    pickerView,
                    buttonContainerView,
                    buttonDividerView,
                    cancelButton,
                    button
                },
                new NSObject[]
                {
                    new NSString("DismissButton"),
                    new NSString("PickerContainerView"),
                    new NSString("pickerView"),
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

            this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[pickerView]|", 0, null, views));
            this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[pickerView]|", 0, null, views));

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
    }
}
