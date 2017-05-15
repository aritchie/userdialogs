using System;
using System.Collections.Generic;
using System.Linq;

using CoreGraphics;

using Foundation;

using UIKit;

namespace Acr.UserDialogs
{
    // The Main Class
    // https://github.com/vikmeup/SCLAlertView-Swift/blob/master/SCLAlertView/SCLAlertView.swift
    // https://github.com/dogo/SCLAlertView
    public class InteractiveAlertView : UIViewController
    {
        // Animation Styles
        public enum SCLAnimationStyle
        {
            NoAnimation, TopToBottom, BottomToTop, LeftToRight, RightToLeft
        }

        // Action Types
        public enum SCLActionType
        {
            None, Closure
        }

        private static readonly nfloat kCircleHeightBackground = 62.0f;

        private readonly SCLAppearance appearance;

        private InteractiveAlertView selfReference;

        private NSObject keyboardWillShowToken;

        private NSObject keyboardWillHideToken;

        public class SCLAppearance
        {
            public nfloat DefaultShadowOpacity { get; set; } = 0.7f;
            public nfloat CircleTopPosition { get; set; } = 0.0f;
            public nfloat CircleBackgroundTopPosition { get; set; } = 6.0f;
            public nfloat CircleHeight { get; set; } = 56.0f;
            public nfloat CircleIconHeight { get; set; } = 20.0f;
            public nfloat TitleTop { get; set; } = 30.0f;
            public nfloat TitleHeight { get; set; } = 25.0f;
            public nfloat TitleMinimumScaleFactor { get; set; } = 1.0f;
            public nfloat WindowWidth { get; set; } = 240.0f;
            public nfloat WindowHeight { get; set; } = 178.0f;
            public nfloat TextHeight { get; set; } = 90.0f;
            public nfloat TextFieldHeight { get; set; } = 45.0f;
            public nfloat TextViewdHeight { get; set; } = 80.0f;
            public nfloat ButtonHeight { get; set; } = 45.0f;
            public UIColor CircleBackgroundColor { get; set; } = UIColor.White;
            public UIColor ContentViewColor { get; set; } = UIColor.White;
            // 0xCCCCCC
            public UIColor ContentViewBorderColor { get; set; } = UIColor.FromRGB(204, 204, 204);
            // 0x4D4D4D
            public UIColor TitleColor { get; set; } = UIColor.FromRGB(77, 77, 77);

            // Fonts
            public UIFont TitleFont { get; set; } = UIFont.SystemFontOfSize(20);
            public UIFont TextFont { get; set; } = UIFont.SystemFontOfSize(14);
            public UIFont ButtonFont { get; set; } = UIFont.SystemFontOfSize(14);

            // UI Options
            public bool DisableTapGesture { get; set; } = false;
            public bool ShowCloseButton { get; set; } = true;
            public bool ShowCircularIcon { get; set; } = true;
            // Set this false to 'Disable' Auto hideView when SCLButton is tapped
            public bool ShouldAutoDismiss { get; set; } = true;
            public nfloat ContentViewCornerRadius { get; set; } = 5.0f;
            public nfloat FieldCornerRadius { get; set; } = 3.0f;
            public nfloat ButtonCornerRadius { get; set; } = 3.0f;
            public bool DynamicAnimatorActive { get; set; } = false;

            // Actions
            public bool HideWhenBackgroundViewIsTapped { get; set; } = false;

            public void SetWindowHeight(nfloat kWindowHeight)
            {
                this.WindowHeight = kWindowHeight;
            }

            public void SetTextHeight(nfloat kTextHeight)
            {
                this.TextHeight = kTextHeight;
            }
        }

        // UI Colour
        public UIColor ViewColor { get; set; }

        // UI Options
        public UIColor IconTintColor { get; set; }
        public UIView CustomSubview { get; set; }

        // Members declaration
        private UIView baseView = new UIView();
        private UILabel labelTitle = new UILabel();
        private UITextView viewText = new UITextView();
        private UIView contentView = new UIView();
        private UIView circleBG = new UIView(new CGRect(0, 0, kCircleHeightBackground, kCircleHeightBackground));
        private UIView circleView = new UIView();
        private UIView circleIconView;
        private double duration;
        private NSTimer durationStatusTimer;
        private NSTimer durationTimer;
        private Action dismissBlock;
        private List<UITextField> inputs { get; set; } = new List<UITextField>();
        private List<UITextView> input { get; set; } = new List<UITextView>();
        private List<SCLButton> buttons { get; set; } = new List<SCLButton>();
        private CGPoint? tmpContentViewFrameOrigin;
        private CGPoint? tmpCircleViewFrameOrigin;
        private bool keyboardHasBeenShown;
        // DynamicAnimator function
        private UIDynamicAnimator animator;
        private UISnapBehavior snapBehavior;

        public InteractiveAlertView(SCLAppearance appearance)
            : base(null, null)
        {
            this.appearance = appearance;
            this.Setup();
        }

        public InteractiveAlertView(string nibNameOrNil, NSBundle bundle, SCLAppearance appearance) : base(nibNameOrNil, bundle)
        {
            this.appearance = appearance;
            this.Setup();
        }

        public InteractiveAlertView(string nibNameOrNil, NSBundle bundle) : this(nibNameOrNil, bundle, new SCLAppearance())
        {

        }

        public InteractiveAlertView() : this(new SCLAppearance())
        {

        }

        private void Setup()
        {
            // Set up main view
            this.View.Frame = UIScreen.MainScreen.Bounds;
            this.View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            this.View.BackgroundColor = new UIColor(red: 0, green: 0, blue: 0, alpha: appearance.DefaultShadowOpacity);
            this.View.AddSubview(baseView);
            // Base View
            baseView.Frame = this.View.Frame;
            baseView.AddSubview(contentView);
            // Content View
            contentView.Layer.CornerRadius = appearance.ContentViewCornerRadius;
            contentView.Layer.MasksToBounds = true;
            contentView.Layer.BorderWidth = 0.5f;
            contentView.AddSubview(labelTitle);
            contentView.AddSubview(viewText);
            // Circle View
            circleBG.BackgroundColor = appearance.CircleBackgroundColor;
            circleBG.Layer.CornerRadius = circleBG.Frame.Size.Height / 2f;
            baseView.AddSubview(circleBG);
            circleBG.AddSubview(circleView);
            var x = (kCircleHeightBackground - appearance.CircleHeight) / 2f;
            circleView.Frame = new CGRect(x, x + appearance.CircleTopPosition, appearance.CircleHeight, appearance.CircleHeight);
            circleView.Layer.CornerRadius = circleView.Frame.Size.Height / 2f;
            // Title
            labelTitle.Lines = 0;
            labelTitle.TextAlignment = UITextAlignment.Center;
            labelTitle.Font = appearance.TitleFont;
            if (appearance.TitleMinimumScaleFactor < 1)
            {
                labelTitle.MinimumScaleFactor = appearance.TitleMinimumScaleFactor;
                labelTitle.AdjustsFontSizeToFitWidth = true;
            }
            labelTitle.Frame = new CGRect(12, appearance.TitleTop, appearance.WindowWidth - 24, appearance.TitleHeight);
            // View text
            viewText.Editable = false;
            viewText.TextAlignment = UITextAlignment.Center;
            viewText.TextContainerInset = UIEdgeInsets.Zero;
            viewText.TextContainer.LineFragmentPadding = 0;
            viewText.Font = appearance.TextFont;
            // Colours
            contentView.BackgroundColor = appearance.ContentViewColor;
            viewText.BackgroundColor = appearance.ContentViewColor;
            labelTitle.TextColor = appearance.TitleColor;
            viewText.TextColor = appearance.TitleColor;
            contentView.Layer.BorderColor = appearance.ContentViewBorderColor.CGColor;
            //Gesture Recognizer for tapping outside the textinput
            if (appearance.DisableTapGesture == false)
            {
                var tapGesture = new UITapGestureRecognizer(this.Tapped);
                tapGesture.NumberOfTapsRequired = 1;
                this.View.AddGestureRecognizer(tapGesture);
            }
        }

        public void SetTitle(string title)
        {
            this.labelTitle.Text = title;
        }

        public void SetSubTitle(string subTitle)
        {
            this.viewText.Text = subTitle;
        }

        public void SetDismissBlock(Action dismissBlock)
        {
            this.dismissBlock = dismissBlock;
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            var rv = UIApplication.SharedApplication.KeyWindow;
            var sz = rv.Frame.Size;
            var frame = rv.Frame;
            frame.Width = sz.Width;
            frame.Height = sz.Height;

            // Set background frame
            this.View.Frame = frame;
            nfloat hMargin = 12f;

            // get actual height of title text
            nfloat titleActualHeight = 0f;
            if (!string.IsNullOrEmpty(labelTitle.Text))
            {
                titleActualHeight = SCLAlertViewExtension.heightWithConstrainedWidth(labelTitle.Text, appearance.WindowWidth - hMargin * 2f, labelTitle.Font) + 10f;
                // get the larger height for the title text
                titleActualHeight = (titleActualHeight > appearance.TitleHeight ? titleActualHeight : appearance.TitleHeight);
            }

            // computing the right size to use for the textView
            var maxHeight = sz.Height - 100f; // max overall height
            nfloat consumedHeight = 0f;
            consumedHeight += (titleActualHeight > 0 ? appearance.TitleTop + titleActualHeight : hMargin);
            consumedHeight += 14;
            consumedHeight += appearance.ButtonHeight * buttons.Count;
            consumedHeight += appearance.TextFieldHeight * inputs.Count;
            consumedHeight += appearance.TextViewdHeight * input.Count;
            var maxViewTextHeight = maxHeight - consumedHeight;
            var viewTextWidth = appearance.WindowWidth - hMargin * 2f;
            var viewTextHeight = appearance.TextHeight;

            // Check if there is a custom subview and add it over the textview
            if (CustomSubview != null)
            {
                viewTextHeight = (nfloat)Math.Min(CustomSubview.Frame.Height, maxViewTextHeight);
                viewText.Text = string.Empty;
                viewText.AddSubview(CustomSubview);
            }
            else
            {
                // computing the right size to use for the textView
                var suggestedViewTextSize = viewText.SizeThatFits(new CGSize(viewTextWidth, nfloat.MaxValue));
                viewTextHeight = (nfloat)Math.Min(suggestedViewTextSize.Height, maxViewTextHeight);
                // scroll management
                if (suggestedViewTextSize.Height > maxViewTextHeight)
                {
                    viewText.ScrollEnabled = true;
                }
                else
                {
                    viewText.ScrollEnabled = false;
                }
            }

            var windowHeight = consumedHeight + viewTextHeight;
            // Set frames
            var x = (sz.Width - appearance.WindowWidth) / 2f;
            var y = (sz.Height - windowHeight - (appearance.CircleHeight / 8)) / 2f;
            contentView.Frame = new CGRect(x, y, appearance.WindowWidth, windowHeight);
            contentView.Layer.CornerRadius = appearance.ContentViewCornerRadius;
            y -= kCircleHeightBackground * 0.6f;
            x = (sz.Width - kCircleHeightBackground) / 2f;
            circleBG.Frame = new CGRect(x, y + appearance.CircleBackgroundTopPosition, kCircleHeightBackground, kCircleHeightBackground);

            //adjust Title frame based on circularIcon show/hide flag
            var titleOffset = appearance.ShowCircularIcon ? 0.0f : -12.0f;
            labelTitle.Frame.Offset(0, titleOffset);

            // Subtitle
            y = titleActualHeight > 0f ? appearance.TitleTop + titleActualHeight + titleOffset : hMargin;
            viewText.Frame = new CGRect(hMargin, y, appearance.WindowWidth - hMargin * 2f, appearance.TextHeight);
            viewText.Frame = new CGRect(hMargin, y, viewTextWidth, viewTextHeight);
            // Text fields
            y += viewTextHeight + 14.0f;
            foreach (var txt in inputs)
            {
                txt.Frame = new CGRect(hMargin, y, appearance.WindowWidth - hMargin * 2, 30);
                txt.Layer.CornerRadius = appearance.FieldCornerRadius;
                y += appearance.TextFieldHeight;
            }
            foreach (var txt in input)
            {
                txt.Frame = new CGRect(hMargin, y, appearance.WindowWidth - hMargin * 2f, 70);
                //txt.layer.cornerRadius = fieldCornerRadius
                y += appearance.TextViewdHeight;
            }

            // Buttons
            foreach (var btn in buttons)
            {
                btn.Frame = new CGRect(hMargin, y, appearance.WindowWidth - hMargin * 2, 35);
                btn.Layer.CornerRadius = appearance.ButtonCornerRadius;
                y += appearance.ButtonHeight;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.keyboardWillShowToken = UIKeyboard.Notifications.ObserveWillShow(this.KeyboardWillShow);
            this.keyboardWillHideToken = UIKeyboard.Notifications.ObserveWillHide(this.KeyboardWillHide);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            this.keyboardWillShowToken?.Dispose();
            this.keyboardWillHideToken?.Dispose();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            if (evt.TouchesForView(this.View)?.Count > 0)
            {
                this.View.EndEditing(true);
            }
        }

        public UITextField AddTextField(string title)
        {
            // Update view height
            appearance.SetWindowHeight(appearance.WindowHeight + appearance.TextFieldHeight);
            // Add text field
            var txt = new UITextField();
            txt.BorderStyle = UITextBorderStyle.RoundedRect;
            txt.Font = appearance.TextFont;
            txt.AutocapitalizationType = UITextAutocapitalizationType.Words;
            txt.ClearButtonMode = UITextFieldViewMode.WhileEditing;
            txt.Layer.MasksToBounds = true;
            txt.Layer.BorderWidth = 1.0f;
            if (!string.IsNullOrEmpty(title))
            {
                txt.Placeholder = title;
            }
            this.contentView.AddSubview(txt);
            this.inputs.Add(txt);
            return txt;
        }

        public UITextView AddTextView()
        {
            // Update view height
            appearance.SetWindowHeight(appearance.WindowHeight + appearance.TextViewdHeight);
            // Add text view
            var txt = new UITextView();
            // No placeholder with UITextView but you can use KMPlaceholderTextView library 
            txt.Font = appearance.TextFont;
            //txt.autocapitalizationType = UITextAutocapitalizationType.Words
            //txt.clearButtonMode = UITextFieldViewMode.WhileEditing
            txt.Layer.MasksToBounds = true;
            txt.Layer.BorderWidth = 1.0f;
            txt.Layer.CornerRadius = 4f;
            contentView.AddSubview(txt);
            input.Add(txt);
            return txt;
        }

        public SCLButton AddButton(string title, Action action, UIColor backgroundColor = null, UIColor textColor = null, bool showDurationStatus = false)
        {
            var btn = this.AddButton(title, backgroundColor, textColor, showDurationStatus);
            btn.ActionType = SCLActionType.Closure;
            btn.Action = action;
            btn.AddTarget(this.ButtonTapped, UIControlEvent.TouchUpInside);
            btn.AddTarget(this.ButtonTapDown, UIControlEvent.TouchDown | UIControlEvent.TouchDragEnter);
            btn.AddTarget(this.ButtonRelease,
                          UIControlEvent.TouchUpInside |
                          UIControlEvent.TouchUpOutside |
                          UIControlEvent.TouchCancel |
                          UIControlEvent.TouchDragOutside);

            return btn;
        }

        public SCLButton AddButton(string title, UIColor backgroundColor = null, UIColor textColor = null, bool showDurationStatus = false)
        {
            // Update view height
            appearance.SetWindowHeight(appearance.WindowHeight + appearance.ButtonHeight);
            // Add button
            var btn = new SCLButton();
            btn.Layer.MasksToBounds = true;
            btn.SetTitle(title, UIControlState.Normal);
            btn.TitleLabel.Font = appearance.ButtonFont;
            btn.CustomBackgroundColor = backgroundColor;
            btn.CustomTextColor = textColor;
            btn.InitialTitle = title;
            btn.ShowDurationStatus = showDurationStatus;
            contentView.AddSubview(btn);
            buttons.Add(btn);

            return btn;
        }

        private void ButtonTapped(object sender, EventArgs args)
        {
            var btn = (SCLButton)sender;
            if (btn.ActionType == SCLActionType.Closure)
            {
                btn.Action?.Invoke();
            }
            else
            {
                Console.WriteLine("Unknow action type for button");
            }

            if (this.View.Alpha != 0 && appearance.ShouldAutoDismiss)
            {
                this.HideView();
            }
        }

        private void ButtonTapDown(object sender, EventArgs args)
        {
            var btn = (SCLButton)sender;
            nfloat hue = 0f;
            nfloat saturation = 0;
            nfloat brightness = 0;
            nfloat alpha = 0;
            nfloat pressBrightnessFactor = 0.85f;
            btn.BackgroundColor?.GetHSBA(out hue, out saturation, out brightness, out alpha);
            brightness = brightness * pressBrightnessFactor;
            btn.BackgroundColor = UIColor.FromHSBA(hue, saturation, brightness, alpha);
        }

        private void ButtonRelease(object sender, EventArgs args)
        {
            var btn = (SCLButton)sender;
            btn.BackgroundColor = btn.CustomBackgroundColor ?? ViewColor ?? btn.BackgroundColor;
        }

        private void KeyboardWillShow(object sender, UIKeyboardEventArgs args)
        {
            if (this.keyboardHasBeenShown)
            {
                return;
            }

            this.keyboardHasBeenShown = true;
            var endKeyBoardFrame = args.FrameEnd.GetMinY();

            if (tmpContentViewFrameOrigin == null)
            {
                tmpContentViewFrameOrigin = this.contentView.Frame.Location;
            }

            if (tmpCircleViewFrameOrigin == null)
            {
                // todo location replace origin 
                tmpCircleViewFrameOrigin = this.circleBG.Frame.Location;
            }

            var newContentViewFrameY = this.contentView.Frame.GetMaxY() - endKeyBoardFrame;
            if (newContentViewFrameY < 0)
            {
                newContentViewFrameY = 0;
            }

            var newBallViewFrameY = this.circleBG.Frame.Y - newContentViewFrameY;
            UIView.AnimateNotify(args.AnimationDuration, 0, ConvertToAnimationOptions(args.AnimationCurve), () =>
            {
                var contentViewFrame = this.contentView.Frame;
                contentViewFrame.Y -= newContentViewFrameY;
                this.contentView.Frame = contentViewFrame;

                var circleBGFrame = circleBG.Frame;
                circleBGFrame.Y = newBallViewFrameY;
                this.circleBG.Frame = circleBGFrame;
            }, null);
        }

        private void KeyboardWillHide(object sender, UIKeyboardEventArgs args)
        {
            if (this.keyboardHasBeenShown)
            {
                UIView.AnimateNotify(args.AnimationDuration, 0, ConvertToAnimationOptions(args.AnimationCurve), () =>
                {
                    //This could happen on the simulator (keyboard will be hidden)
                    if (this.tmpContentViewFrameOrigin.HasValue)
                    {
                        var contentViewFrame = this.contentView.Frame;
                        contentViewFrame.Y = this.tmpContentViewFrameOrigin.Value.Y;
                        this.contentView.Frame = contentViewFrame;
                        this.tmpContentViewFrameOrigin = null;
                    }
                    if (this.tmpCircleViewFrameOrigin.HasValue)
                    {
                        var circleBGFrame = this.circleBG.Frame;
                        circleBGFrame.Y = this.tmpCircleViewFrameOrigin.Value.Y;
                        this.circleBG.Frame = circleBGFrame;
                        this.tmpCircleViewFrameOrigin = null;
                    }
                }, null);
            }

            this.keyboardHasBeenShown = false;
        }

        //Dismiss keyboard when tapped outside textfield & close SCLAlertView when hideWhenBackgroundViewIsTapped
        private void Tapped(UITapGestureRecognizer gestureRecognizer)
        {
            this.View.EndEditing(true);
            if (gestureRecognizer.View.HitTest(gestureRecognizer.LocationInView(gestureRecognizer.View), null) == baseView && appearance.HideWhenBackgroundViewIsTapped)
            {
                this.HideView();
            }
        }

        public SCLAlertViewResponder ShowCustom(string title,
                                                string subTitle,
                                                UIColor color,
                                                UIImage icon,
                                                string closeButtonTitle = null,
                                                double duration = 0.0,
                                                UIColor colorStyle = null,
                                                UIColor colorTextButton = null,
                                                InteractiveAlertStyle style = InteractiveAlertStyle.Success,
                                                SCLAnimationStyle animationStyle = SCLAnimationStyle.TopToBottom)
        {

            colorStyle = colorStyle ?? GetDefaultColorStyle(style);
            colorTextButton = colorTextButton ?? GetDefaultColorTextButton(style) ?? UIColor.White;
            return ShowTitle(title, subTitle, duration, closeButtonTitle, style, color, colorTextButton, icon, animationStyle);
        }

        public SCLAlertViewResponder ShowAlert(InteractiveAlertStyle style,
                                               string title,
                                               string subTitle,
                                               string closeButtonTitle = null,
                                               double duration = 0.0,
                                               UIColor colorStyle = null,
                                               UIColor colorTextButton = null,
                                               UIImage circleIconImage = null,
                                               SCLAnimationStyle animationStyle = SCLAnimationStyle.TopToBottom)
        {
            colorStyle = colorStyle ?? GetDefaultColorStyle(style);
            colorTextButton = colorTextButton ?? GetDefaultColorTextButton(style) ?? UIColor.White;

            return this.ShowTitle(title, subTitle, duration, closeButtonTitle, style, colorStyle, colorTextButton, circleIconImage, animationStyle);
        }

        public SCLAlertViewResponder ShowTitle(string title,
                                               string subTitle,
                                               double duration,
                                               string completeText,
                                               InteractiveAlertStyle style,
                                               UIColor colorStyle = null,
                                               UIColor colorTextButton = null,
                                               UIImage circleIconImage = null,
                                               SCLAnimationStyle animationStyle = SCLAnimationStyle.TopToBottom)
        {
            colorStyle = colorStyle ?? UIColor.Black;
            colorTextButton = colorTextButton ?? UIColor.White;
            this.selfReference = this;
            this.View.Alpha = 0;
            var rv = UIApplication.SharedApplication.KeyWindow;
            rv.AddSubview(this.View);
            this.View.Frame = rv.Bounds;
            baseView.Frame = rv.Bounds;

            // Alert colour/icon
            UIImage iconImage = null;
            // Icon style
            switch (style)
            {
                case InteractiveAlertStyle.Success:
                    iconImage = checkCircleIconImage(circleIconImage, SCLAlertViewStyleKit.imageOfCheckmark());
                    break;
                case InteractiveAlertStyle.Error:
                    iconImage = checkCircleIconImage(circleIconImage, SCLAlertViewStyleKit.imageOfCross());
                    break;
                case InteractiveAlertStyle.Warning:
                    iconImage = checkCircleIconImage(circleIconImage, SCLAlertViewStyleKit.imageOfWarning());
                    break;
                case InteractiveAlertStyle.Wait:
                    iconImage = null;
                    break;
                    //case InteractiveAlertStyle.Notice:
                    //iconImage = checkCircleIconImage(circleIconImage, SCLAlertViewStyleKit.imageOfNotice());
                    //break;
                    //case InteractiveAlertStyle.Info:
                    //iconImage = checkCircleIconImage(circleIconImage, SCLAlertViewStyleKit.imageOfInfo());
                    //break;
                    //case InteractiveAlertStyle.Edit:
                    //iconImage = checkCircleIconImage(circleIconImage, SCLAlertViewStyleKit.imageOfEdit());
                    //break;
                    //case InteractiveAlertStyle.Question:
                    //iconImage = checkCircleIconImage(circleIconImage, SCLAlertViewStyleKit.imageOfQuestion());
                    //break;
            }

            // Title
            if (!string.IsNullOrEmpty(title))
            {
                this.labelTitle.Text = title;
                var actualHeight = SCLAlertViewExtension.heightWithConstrainedWidth(title, appearance.WindowWidth - 24, this.labelTitle.Font);
                this.labelTitle.Frame = new CGRect(12, appearance.TitleTop, appearance.WindowWidth - 24, actualHeight);
            }

            // Subtitle
            if (!string.IsNullOrEmpty(subTitle))
            {
                this.viewText.Text = subTitle;
                // Adjust text view size, if necessary
                var str = new NSString(subTitle);
                var font = viewText.Font;
                var attr = new UIStringAttributes { Font = viewText.Font };
                var sz = new CGSize(appearance.WindowWidth - 24, 90);
                var r = str.GetBoundingRect(sz, NSStringDrawingOptions.UsesLineFragmentOrigin, attr, null);
                var ht = (nfloat)Math.Ceiling(r.Size.Height);
                if (ht < appearance.TextHeight)
                {
                    appearance.WindowHeight -= appearance.TextHeight - ht;
                    appearance.SetTextHeight(ht);
                }
            }

            if

            // Done button
            (appearance.ShowCloseButton)
            {
                title = completeText ?? "Done";
                this.AddButton(title, this.HideView);
            }

            //hidden/show circular view based on the ui option
            circleView.Hidden = !appearance.ShowCircularIcon;
            circleBG.Hidden = !appearance.ShowCircularIcon;

            // Alert view colour and images
            circleView.BackgroundColor = colorStyle;
            // Spinner / icon
            if (style == InteractiveAlertStyle.Wait)
            {
                var indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
                indicator.StartAnimating();
                circleIconView = indicator;
            }
            else
            {
                if (IconTintColor != null)
                {
                    circleIconView = new UIImageView(iconImage?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate));
                    circleIconView.TintColor = IconTintColor;
                }
                else
                {
                    circleIconView = new UIImageView(iconImage);
                }
            }

            circleView.AddSubview(circleIconView);
            var x = (appearance.CircleHeight - appearance.CircleIconHeight) / 2f;
            circleIconView.Frame = new CGRect(x, x, appearance.CircleIconHeight, appearance.CircleIconHeight);
            circleIconView.Layer.CornerRadius = circleIconView.Bounds.Height / 2f;
            circleIconView.Layer.MasksToBounds = true;

            foreach (var txt in inputs)
            {
                txt.Layer.BorderColor = colorStyle.CGColor;
            }

            foreach (var txt in input)
            {
                txt.Layer.BorderColor = colorStyle.CGColor;
            }

            foreach (var btn in buttons)
            {
                btn.BackgroundColor = btn.CustomBackgroundColor ?? colorStyle;
                btn.SetTitleColor(btn.CustomTextColor ?? colorTextButton ?? UIColor.White, UIControlState.Normal);
            }

            // Adding duration
            if (duration > 0)
            {
                this.duration = duration;
                durationTimer?.Invalidate();
                durationTimer = NSTimer.CreateScheduledTimer(this.duration, false, obj => { this.HideView(); });
                durationStatusTimer?.Invalidate();
                durationStatusTimer = NSTimer.CreateScheduledTimer(1.0d, true, obj => { this.UpdateDurationStatus(); });
            }

            // Animate in the alert view
            this.ShowAnimation(animationStyle);

            // Chainable objects
            return new SCLAlertViewResponder(this);
        }

        // Show animation in the alert view
        private void ShowAnimation(
            SCLAnimationStyle animationStyle = SCLAnimationStyle.TopToBottom,
            float animationStartOffset = -400.0f,
            float boundingAnimationOffset = 15.0f,
            double animationDuration = 0.2f)
        {

            var rv = UIApplication.SharedApplication.KeyWindow;
            var animationStartOrigin = this.baseView.Frame;
            CGPoint animationCenter = rv.Center;
            switch (animationStyle)
            {
                case SCLAnimationStyle.NoAnimation:
                    this.View.Alpha = 1.0f;
                    break;
                case SCLAnimationStyle.TopToBottom:
                    animationStartOrigin.Location = new CGPoint(animationStartOrigin.X, this.baseView.Frame.Y + animationStartOffset);
                    animationCenter = new CGPoint(animationCenter.X, animationCenter.Y + boundingAnimationOffset);
                    break;
                case SCLAnimationStyle.BottomToTop:
                    animationStartOrigin.Location = new CGPoint(animationStartOrigin.X, this.baseView.Frame.Y - animationStartOffset);
                    animationCenter = new CGPoint(animationCenter.X, animationCenter.Y - boundingAnimationOffset);
                    break;
                case SCLAnimationStyle.LeftToRight:
                    animationStartOrigin.Location = new CGPoint(this.baseView.Frame.X + animationStartOffset, animationStartOrigin.Y);
                    animationCenter = new CGPoint(animationCenter.X + boundingAnimationOffset, animationCenter.Y);
                    break;
                case SCLAnimationStyle.RightToLeft:
                    animationStartOrigin.Location = new CGPoint(this.baseView.Frame.X - animationStartOffset, animationStartOrigin.Y);
                    animationCenter = new CGPoint(animationCenter.X - boundingAnimationOffset, animationCenter.Y);
                    break;
            }

            var baseViewFrame = this.baseView.Frame;
            baseViewFrame = animationStartOrigin;
            this.baseView.Frame = baseViewFrame;

            if (this.appearance.DynamicAnimatorActive)
            {
                UIView.AnimateNotify(animationDuration, () =>
                {
                    this.View.Alpha = 1;
                }, null);

                this.Animate(this.baseView, rv.Center);
            }
            else
            {
                UIView.AnimateNotify(animationDuration, () =>
                {
                    this.View.Alpha = 1;
                    this.baseView.Center = animationCenter;
                }, completion =>
                {
                    UIView.AnimateNotify(animationDuration, () =>
                    {
                        this.View.Alpha = 1;
                        this.baseView.Center = rv.Center;
                    }, null);
                });
            }
        }

        private void Animate(UIView item, CGPoint center)
        {
            if (this.snapBehavior != null)
            {
                this.animator?.RemoveBehavior(snapBehavior);
            }

            this.animator = new UIDynamicAnimator(this.View);
            var tempSnapBehavior = new UISnapBehavior(item, center);
            this.animator?.AddBehavior(tempSnapBehavior);
            this.snapBehavior = tempSnapBehavior;
        }

        private void UpdateDurationStatus()
        {
            duration = duration - 1;
            foreach (var btn in buttons.Where(x => x.ShowDurationStatus))
            {
                var txt = $"{btn.InitialTitle} {duration}";
                btn.SetTitle(txt, UIControlState.Normal);
            }
        }

        // Close SCLAlertView
        public void HideView()
        {
            UIView.AnimateNotify(0.2, () =>
            {
                this.View.Alpha = 0;
            }, completion =>
            {
                //Stop durationTimer so alertView does not attempt to hide itself and fire it's dimiss block a second time when close button is tapped
                this.durationTimer?.Invalidate();
                // Stop StatusTimer
                this.durationStatusTimer?.Invalidate();
                // Call completion handler when the alert is dismissed
                this.dismissBlock?.Invoke();

                // This is necessary for SCLAlertView to be de-initialized, preventing a strong reference cycle with the viewcontroller calling SCLAlertView.
                foreach (var button in this.buttons)
                {
                    button.Action = null;
                }

                this.View.RemoveFromSuperview();
                this.selfReference = null;
            });
        }

        protected static UIImage checkCircleIconImage(UIImage circleIconImage, UIImage defaultImage) => circleIconImage ?? defaultImage;

        private static UIViewAnimationOptions ConvertToAnimationOptions(UIViewAnimationCurve curve)
        {
            // Looks like a hack. But it is correct.
            // UIViewAnimationCurve and UIViewAnimationOptions are shifted by 16 bits
            // http://stackoverflow.com/questions/18870447/how-to-use-the-default-ios7-uianimation-curve/18873820#18873820
            return (UIViewAnimationOptions)((int)curve << 16);
        }

        private static UIColor GetDefaultColorTextButton(InteractiveAlertStyle style)
        {
            switch (style)
            {
                case InteractiveAlertStyle.Success:
                case InteractiveAlertStyle.Error:
                //case InteractiveAlertStyle.Notice:
                //case InteractiveAlertStyle.Info:
                case InteractiveAlertStyle.Wait:
                    //case InteractiveAlertStyle.Edit:
                    //case InteractiveAlertStyle.Question:
                    return UIColor.White;
                case InteractiveAlertStyle.Warning:
                    return UIColor.Black;
                default:
                    return UIColor.White;
            }
        }

        private static UIColor GetDefaultColorStyle(InteractiveAlertStyle style)
        {
            switch (style)
            {
                case InteractiveAlertStyle.Success:
                    // 0x22B573
                    return UIColor.FromRGB(34, 181, 115);
                case InteractiveAlertStyle.Error:
                    // 0xC1272D
                    return UIColor.FromRGB(193, 39, 45);
                //case InteractiveAlertStyle.Notice:
                //// 0x727375
                //return UIColor.FromRGB(114, 115, 117);
                case InteractiveAlertStyle.Warning:
                    // 0xFFD110
                    return UIColor.FromRGB(255, 209, 16);
                //case InteractiveAlertStyle.Info:
                //// 0x2866BF
                //return UIColor.FromRGB(40, 102, 191);
                //case InteractiveAlertStyle.Edit:
                //// 0xA429FF
                //return UIColor.FromRGB(164, 41, 255);
                case InteractiveAlertStyle.Wait:
                    // 0xD62DA5
                    return UIColor.FromRGB(204, 45, 165);
                //case InteractiveAlertStyle.Question:
                //// 0x727375
                //return UIColor.FromRGB(114, 115, 117);
                default:
                    return UIColor.White;
            }
        }

        // Button sub-class
        public class SCLButton : UIButton
        {
            public SCLActionType ActionType { get; set; } = SCLActionType.None;
            public UIColor CustomBackgroundColor { get; set; }
            public UIColor CustomTextColor { get; set; }
            public string InitialTitle { get; set; }
            public bool ShowDurationStatus { get; set; } = false;
            public Action Action { get; set; }

            public SCLButton() : base(CGRect.Empty)
            {

            }

            public SCLButton(CGRect rect) : base(rect)
            {

            }
        }

        protected static class SCLAlertViewExtension
        {
            public static nfloat heightWithConstrainedWidth(string text, nfloat width, UIFont font)
            {
                var constraintRect = new CGSize(width, nfloat.MaxValue);
                var boundingBox = new NSString(text).GetBoundingRect(constraintRect, NSStringDrawingOptions.UsesLineFragmentOrigin, new UIStringAttributes { Font = font }, null);

                return boundingBox.Height;
            }
        }

        // ------------------------------------
        // Icon drawing
        // Code generated by PaintCode
        // ------------------------------------
        protected class SCLAlertViewStyleKit : NSObject
        {
            // Cache
            protected static class Cache
            {
                public static UIImage imageOfCheckmarkImage;
                public static UIImage imageOfCrossImage;
                public static UIImage imageOfNoticeImage;
                public static UIImage imageOfWarningImage;
                public static UIImage imageOfInfoImage;
                public static UIImage imageOfEditImage;
                public static UIImage imageOfQuestionImage;
            }

            public static UIImage imageOfCheckmark() => RendererandCacheImage(drawCheckmark, ref Cache.imageOfCheckmarkImage);

            public static UIImage imageOfCross() => RendererandCacheImage(drawCross, ref Cache.imageOfCrossImage);

            public static UIImage imageOfNotice() => RendererandCacheImage(drawNotice, ref Cache.imageOfNoticeImage);

            public static UIImage imageOfWarning() => RendererandCacheImage(drawWarning, ref Cache.imageOfWarningImage);

            public static UIImage imageOfInfo() => RendererandCacheImage(drawInfo, ref Cache.imageOfInfoImage);

            public static UIImage imageOfEdit() => RendererandCacheImage(drawEdit, ref Cache.imageOfEditImage);

            public static UIImage imageOfQuestion() => RendererandCacheImage(drawQuestion, ref Cache.imageOfQuestionImage);

            private static UIImage RendererandCacheImage(Action rendererAction, ref UIImage image)
            {
                if (image != null)
                {
                    return image;
                }

                UIGraphics.BeginImageContextWithOptions(new CGSize(80, 80), false, 0);
                rendererAction.Invoke();
                image = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                return image;
            }

            // Drawing Methods
            private static void drawCheckmark()
            {
                // Checkmark Shape Drawing
                var checkmarkShapePath = new UIBezierPath();
                checkmarkShapePath.MoveTo(new CGPoint(x: 73.25, y: 14.05));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 64.51, y: 13.86), new CGPoint(x: 70.98, y: 11.44), new CGPoint(x: 66.78, y: 11.26));
                checkmarkShapePath.AddLineTo(new CGPoint(x: 27.46, y: 52));
                checkmarkShapePath.AddLineTo(new CGPoint(x: 15.75, y: 39.54));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 6.84, y: 39.54), new CGPoint(x: 13.48, y: 36.93), new CGPoint(x: 9.28, y: 36.93));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 6.84, y: 49.02), new CGPoint(x: 4.39, y: 42.14), new CGPoint(x: 4.39, y: 46.42));
                checkmarkShapePath.AddLineTo(new CGPoint(x: 22.91, y: 66.14));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 27.28, y: 68), new CGPoint(x: 24.14, y: 67.44), new CGPoint(x: 25.71, y: 68));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 31.65, y: 66.14), new CGPoint(x: 28.86, y: 68), new CGPoint(x: 30.43, y: 67.26));
                checkmarkShapePath.AddLineTo(new CGPoint(x: 73.08, y: 23.35));
                checkmarkShapePath.AddCurveToPoint(new CGPoint(x: 73.25, y: 14.05), new CGPoint(x: 75.52, y: 20.75), new CGPoint(x: 75.7, y: 16.65));
                checkmarkShapePath.ClosePath();
                checkmarkShapePath.MiterLimit = 4;

                UIColor.White.SetFill();
                checkmarkShapePath.Fill();
            }

            private static void drawCross()
            {
                // Cross Shape Drawing
                var crossShapePath = new UIBezierPath();
                crossShapePath.MoveTo(new CGPoint(x: 10, y: 70));
                crossShapePath.AddLineTo(new CGPoint(x: 70, y: 10));
                crossShapePath.MoveTo(new CGPoint(x: 10, y: 10));
                crossShapePath.AddLineTo(new CGPoint(x: 70, y: 70));
                crossShapePath.LineCapStyle = CGLineCap.Round;
                crossShapePath.LineJoinStyle = CGLineJoin.Round;
                UIColor.White.SetStroke();
                crossShapePath.LineWidth = 14;
                crossShapePath.Stroke();
            }

            private static void drawNotice()
            {
                // Notice Shape Drawing
                var noticeShapePath = new UIBezierPath();
                noticeShapePath.MoveTo(new CGPoint(x: 72, y: 48.54));
                noticeShapePath.AddLineTo(new CGPoint(x: 72, y: 39.9));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 66.38, y: 34.01), new CGPoint(x: 72, y: 36.76), new CGPoint(x: 69.48, y: 34.01));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 61.53, y: 35.97), new CGPoint(x: 64.82, y: 34.01), new CGPoint(x: 62.69, y: 34.8));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 60.36, y: 35.78), new CGPoint(x: 61.33, y: 35.97), new CGPoint(x: 62.3, y: 35.78));
                noticeShapePath.AddLineTo(new CGPoint(x: 60.36, y: 33.22));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 54.16, y: 26.16), new CGPoint(x: 60.36, y: 29.3), new CGPoint(x: 57.65, y: 26.16));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 48.73, y: 29.89), new CGPoint(x: 51.64, y: 26.16), new CGPoint(x: 50.67, y: 27.73));
                noticeShapePath.AddLineTo(new CGPoint(x: 48.73, y: 28.71));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 43.49, y: 21.64), new CGPoint(x: 48.73, y: 24.78), new CGPoint(x: 46.98, y: 21.64));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 39.03, y: 25.37), new CGPoint(x: 40.97, y: 21.64), new CGPoint(x: 39.03, y: 23.01));
                noticeShapePath.AddLineTo(new CGPoint(x: 39.03, y: 9.07));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 32.24, y: 2), new CGPoint(x: 39.03, y: 5.14), new CGPoint(x: 35.73, y: 2));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 25.45, y: 9.07), new CGPoint(x: 28.56, y: 2), new CGPoint(x: 25.45, y: 5.14));
                noticeShapePath.AddLineTo(new CGPoint(x: 25.45, y: 41.47));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 24.29, y: 43.44), new CGPoint(x: 25.45, y: 42.45), new CGPoint(x: 24.68, y: 43.04));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 9.55, y: 43.04), new CGPoint(x: 16.73, y: 40.88), new CGPoint(x: 11.88, y: 40.69));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 8, y: 46.58), new CGPoint(x: 8.58, y: 43.83), new CGPoint(x: 8, y: 45.2));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 14.4, y: 55.81), new CGPoint(x: 8.19, y: 50.31), new CGPoint(x: 12.07, y: 53.84));
                noticeShapePath.AddLineTo(new CGPoint(x: 27.2, y: 69.56));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 42.91, y: 77.8), new CGPoint(x: 30.5, y: 74.47), new CGPoint(x: 35.73, y: 77.21));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 43.88, y: 77.8), new CGPoint(x: 43.3, y: 77.8), new CGPoint(x: 43.68, y: 77.8));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 47.18, y: 78), new CGPoint(x: 45.04, y: 77.8), new CGPoint(x: 46.01, y: 78));
                noticeShapePath.AddLineTo(new CGPoint(x: 48.34, y: 78));
                noticeShapePath.AddLineTo(new CGPoint(x: 48.34, y: 78));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 71.61, y: 52.08), new CGPoint(x: 56.48, y: 78), new CGPoint(x: 69.87, y: 75.05));
                noticeShapePath.AddCurveToPoint(new CGPoint(x: 72, y: 48.54), new CGPoint(x: 71.81, y: 51.29), new CGPoint(x: 72, y: 49.72));
                noticeShapePath.ClosePath();
                noticeShapePath.MiterLimit = 4;

                UIColor.White.SetFill();
                noticeShapePath.Fill();
            }

            private static void drawWarning()
            {
                // Color Declarations
                var greyColor = new UIColor(red: 0.236f, green: 0.236f, blue: 0.236f, alpha: 1.000f);

                // Warning Group
                // Warning Circle Drawing
                var warningCirclePath = new UIBezierPath();
                warningCirclePath.MoveTo(new CGPoint(x: 40.94, y: 63.39));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 36.03, y: 65.55), new CGPoint(x: 39.06, y: 63.39), new CGPoint(x: 37.36, y: 64.18));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 34.14, y: 70.45), new CGPoint(x: 34.9, y: 66.92), new CGPoint(x: 34.14, y: 68.49));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 36.22, y: 75.54), new CGPoint(x: 34.14, y: 72.41), new CGPoint(x: 34.9, y: 74.17));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 40.94, y: 77.5), new CGPoint(x: 37.54, y: 76.91), new CGPoint(x: 39.06, y: 77.5));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 45.86, y: 75.35), new CGPoint(x: 42.83, y: 77.5), new CGPoint(x: 44.53, y: 76.72));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 47.93, y: 70.45), new CGPoint(x: 47.18, y: 74.17), new CGPoint(x: 47.93, y: 72.41));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 45.86, y: 65.35), new CGPoint(x: 47.93, y: 68.49), new CGPoint(x: 47.18, y: 66.72));
                warningCirclePath.AddCurveToPoint(new CGPoint(x: 40.94, y: 63.39), new CGPoint(x: 44.53, y: 64.18), new CGPoint(x: 42.83, y: 63.39));
                warningCirclePath.ClosePath();
                warningCirclePath.MiterLimit = 4;

                greyColor.SetFill();
                warningCirclePath.Fill();


                // Warning Shape Drawing
                var warningShapePath = new UIBezierPath();
                warningShapePath.MoveTo(new CGPoint(x: 46.23, y: 4.26));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 40.94, y: 2.5), new CGPoint(x: 44.91, y: 3.09), new CGPoint(x: 43.02, y: 2.5));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 34.71, y: 4.26), new CGPoint(x: 38.68, y: 2.5), new CGPoint(x: 36.03, y: 3.09));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 31.5, y: 8.77), new CGPoint(x: 33.01, y: 5.44), new CGPoint(x: 31.5, y: 7.01));
                warningShapePath.AddLineTo(new CGPoint(x: 31.5, y: 19.36));
                warningShapePath.AddLineTo(new CGPoint(x: 34.71, y: 54.44));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 40.38, y: 58.16), new CGPoint(x: 34.9, y: 56.2), new CGPoint(x: 36.41, y: 58.16));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 45.67, y: 54.44), new CGPoint(x: 44.34, y: 58.16), new CGPoint(x: 45.67, y: 56.01));
                warningShapePath.AddLineTo(new CGPoint(x: 48.5, y: 19.36));
                warningShapePath.AddLineTo(new CGPoint(x: 48.5, y: 8.77));
                warningShapePath.AddCurveToPoint(new CGPoint(x: 46.23, y: 4.26), new CGPoint(x: 48.5, y: 7.01), new CGPoint(x: 47.74, y: 5.44));
                warningShapePath.ClosePath();
                warningShapePath.MiterLimit = 4;

                greyColor.SetFill();
                warningShapePath.Fill();
            }

            private static void drawInfo()
            {
                // Color Declarations
                var color0 = new UIColor(red: 1.000f, green: 1.000f, blue: 1.000f, alpha: 1.000f);

                // Info Shape Drawing
                var infoShapePath = new UIBezierPath();
                infoShapePath.MoveTo(new CGPoint(x: 45.66, y: 15.96));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 45.66, y: 5.22), new CGPoint(x: 48.78, y: 12.99), new CGPoint(x: 48.78, y: 8.19));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 34.34, y: 5.22), new CGPoint(x: 42.53, y: 2.26), new CGPoint(x: 37.47, y: 2.26));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 34.34, y: 15.96), new CGPoint(x: 31.22, y: 8.19), new CGPoint(x: 31.22, y: 12.99));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 45.66, y: 15.96), new CGPoint(x: 37.47, y: 18.92), new CGPoint(x: 42.53, y: 18.92));
                infoShapePath.ClosePath();
                infoShapePath.MoveTo(new CGPoint(x: 48, y: 69.41));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 40, y: 77), new CGPoint(x: 48, y: 73.58), new CGPoint(x: 44.4, y: 77));
                infoShapePath.AddLineTo(new CGPoint(x: 40, y: 77));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 32, y: 69.41), new CGPoint(x: 35.6, y: 77), new CGPoint(x: 32, y: 73.58));
                infoShapePath.AddLineTo(new CGPoint(x: 32, y: 35.26));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 40, y: 27.67), new CGPoint(x: 32, y: 31.08), new CGPoint(x: 35.6, y: 27.67));
                infoShapePath.AddLineTo(new CGPoint(x: 40, y: 27.67));
                infoShapePath.AddCurveToPoint(new CGPoint(x: 48, y: 35.26), new CGPoint(x: 44.4, y: 27.67), new CGPoint(x: 48, y: 31.08));
                infoShapePath.AddLineTo(new CGPoint(x: 48, y: 69.41));
                infoShapePath.ClosePath();
                color0.SetFill();
                infoShapePath.Fill();
            }

            private static void drawEdit()
            {
                // Color Declarations
                var color = new UIColor(red: 1.0f, green: 1.0f, blue: 1.0f, alpha: 1.0f);

                // Edit shape Drawing
                var editPathPath = new UIBezierPath();
                editPathPath.MoveTo(new CGPoint(x: 71, y: 2.7));
                editPathPath.AddCurveToPoint(new CGPoint(x: 71.9, y: 15.2), new CGPoint(x: 74.7, y: 5.9), new CGPoint(x: 75.1, y: 11.6));
                editPathPath.AddLineTo(new CGPoint(x: 64.5, y: 23.7));
                editPathPath.AddLineTo(new CGPoint(x: 49.9, y: 11.1));
                editPathPath.AddLineTo(new CGPoint(x: 57.3, y: 2.6));
                editPathPath.AddCurveToPoint(new CGPoint(x: 69.7, y: 1.7), new CGPoint(x: 60.4, y: -1.1), new CGPoint(x: 66.1, y: -1.5));
                editPathPath.AddLineTo(new CGPoint(x: 71, y: 2.7));
                editPathPath.AddLineTo(new CGPoint(x: 71, y: 2.7));
                editPathPath.ClosePath();
                editPathPath.MoveTo(new CGPoint(x: 47.8, y: 13.5));
                editPathPath.AddLineTo(new CGPoint(x: 13.4, y: 53.1));
                editPathPath.AddLineTo(new CGPoint(x: 15.7, y: 55.1));
                editPathPath.AddLineTo(new CGPoint(x: 50.1, y: 15.5));
                editPathPath.AddLineTo(new CGPoint(x: 47.8, y: 13.5));
                editPathPath.AddLineTo(new CGPoint(x: 47.8, y: 13.5));
                editPathPath.ClosePath();
                editPathPath.MoveTo(new CGPoint(x: 17.7, y: 56.7));
                editPathPath.AddLineTo(new CGPoint(x: 23.8, y: 62.2));
                editPathPath.AddLineTo(new CGPoint(x: 58.2, y: 22.6));
                editPathPath.AddLineTo(new CGPoint(x: 52, y: 17.1));
                editPathPath.AddLineTo(new CGPoint(x: 17.7, y: 56.7));
                editPathPath.AddLineTo(new CGPoint(x: 17.7, y: 56.7));
                editPathPath.ClosePath();
                editPathPath.MoveTo(new CGPoint(x: 25.8, y: 63.8));
                editPathPath.AddLineTo(new CGPoint(x: 60.1, y: 24.2));
                editPathPath.AddLineTo(new CGPoint(x: 62.3, y: 26.1));
                editPathPath.AddLineTo(new CGPoint(x: 28.1, y: 65.7));
                editPathPath.AddLineTo(new CGPoint(x: 25.8, y: 63.8));
                editPathPath.AddLineTo(new CGPoint(x: 25.8, y: 63.8));
                editPathPath.ClosePath();
                editPathPath.MoveTo(new CGPoint(x: 25.9, y: 68.1));
                editPathPath.AddLineTo(new CGPoint(x: 4.2, y: 79.5));
                editPathPath.AddLineTo(new CGPoint(x: 11.3, y: 55.5));
                editPathPath.AddLineTo(new CGPoint(x: 25.9, y: 68.1));
                editPathPath.ClosePath();
                editPathPath.MiterLimit = 4;
                editPathPath.UsesEvenOddFillRule = true;
                color.SetFill();
                editPathPath.Fill();
            }

            private static void drawQuestion()
            {
                // Color Declarations
                var color = new UIColor(red: 1.0f, green: 1.0f, blue: 1.0f, alpha: 1.0f);
                // Questionmark Shape Drawing
                var questionShapePath = new UIBezierPath();
                questionShapePath.MoveTo(new CGPoint(x: 33.75, y: 54.1));
                questionShapePath.AddLineTo(new CGPoint(x: 44.15, y: 54.1));
                questionShapePath.AddLineTo(new CGPoint(x: 44.15, y: 47.5));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 51.85, y: 37.2), new CGPoint(x: 44.15, y: 42.9), new CGPoint(x: 46.75, y: 41.2));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 61.95, y: 19.9), new CGPoint(x: 59.05, y: 31.6), new CGPoint(x: 61.95, y: 28.5));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 41.45, y: 2.8), new CGPoint(x: 61.95, y: 7.6), new CGPoint(x: 52.85, y: 2.8));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 25.05, y: 5.8), new CGPoint(x: 34.75, y: 2.8), new CGPoint(x: 29.65, y: 3.8));
                questionShapePath.AddLineTo(new CGPoint(x: 25.05, y: 14.4));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 38.15, y: 12.3), new CGPoint(x: 29.15, y: 13.2), new CGPoint(x: 32.35, y: 12.3));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 49.65, y: 20.8), new CGPoint(x: 45.65, y: 12.3), new CGPoint(x: 49.65, y: 14.4));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 43.65, y: 31.7), new CGPoint(x: 49.65, y: 26), new CGPoint(x: 47.95, y: 28.4));
                questionShapePath.AddCurveToPoint(new CGPoint(x: 33.75, y: 46.6), new CGPoint(x: 37.15, y: 36.9), new CGPoint(x: 33.75, y: 39.7));
                questionShapePath.AddLineTo(new CGPoint(x: 33.75, y: 54.1));
                questionShapePath.ClosePath();
                questionShapePath.MoveTo(new CGPoint(x: 33.15, y: 75.4));
                questionShapePath.AddLineTo(new CGPoint(x: 45.35, y: 75.4));
                questionShapePath.AddLineTo(new CGPoint(x: 45.35, y: 63.7));
                questionShapePath.AddLineTo(new CGPoint(x: 33.15, y: 63.7));
                questionShapePath.AddLineTo(new CGPoint(x: 33.15, y: 75.4));
                questionShapePath.ClosePath();
                color.SetFill();
                questionShapePath.Fill();
            }
        }
    }

    // Allow alerts to be closed/renamed in a chainable manner
    // Example: SCLAlertView().showSuccess(self, title: "Test", subTitle: "Value").close()
    public class SCLAlertViewResponder
    {
        // Initialisation and Title/Subtitle/Close functions
        public SCLAlertViewResponder(InteractiveAlertView alertview)
        {
            this.Alertview = alertview;
        }

        protected InteractiveAlertView Alertview { get; }

        public void SetTitle(string title)
        {
            this.Alertview.SetTitle(title);
        }

        public void SetSubTitle(string subTitle)
        {
            this.Alertview.SetSubTitle(subTitle);
        }

        public void Close()
        {
            this.Alertview.HideView();
        }

        public void SetDismissBlock(Action dismissBlock)
        {
            this.Alertview.SetDismissBlock(dismissBlock);
        }
    }
}