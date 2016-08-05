// BTProgressHUD - port of SVProgressHUD
//
//  https://github.com/nicwise/BTProgressHUD
// 
//  Ported by Nic Wise - 
//  Copyright 2013 Nic Wise. MIT license.
// 
//  SVProgressHUD.m
//
//  Created by Sam Vermette on 27.03.11.
//  Copyright 2011 Sam Vermette. All rights reserved.
//
//  https://github.com/samvermette/SVProgressHUD
//
//  Version 1.6.1
using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using CoreAnimation;
using CoreGraphics;
using ObjCRuntime;


namespace BigTed
{
	public class ProgressHUD : UIView
	{
		static Class clsUIPeripheralHostView = null;
		static Class clsUIKeyboard = null;
		static Class clsUIInputSetContainerView = null;
		static Class clsUIInputSetHostView = null;

		static ProgressHUD ()
		{
			//initialize static fields used for input view detection
			var ptrUIPeripheralHostView = Class.GetHandle("UIPeripheralHostView");
			if (ptrUIPeripheralHostView != IntPtr.Zero)
				clsUIPeripheralHostView = new Class (ptrUIPeripheralHostView);
			var ptrUIKeyboard = Class.GetHandle("UIKeyboard");
			if (ptrUIKeyboard != IntPtr.Zero)
				clsUIKeyboard = new Class (ptrUIKeyboard);
			var ptrUIInputSetContainerView = Class.GetHandle("UIInputSetContainerView");
			if (ptrUIInputSetContainerView != IntPtr.Zero)
				clsUIInputSetContainerView = new Class (ptrUIInputSetContainerView);
			var ptrUIInputSetHostView = Class.GetHandle("UIInputSetHostView");
			if (ptrUIInputSetHostView != IntPtr.Zero)
				clsUIInputSetHostView = new Class (ptrUIInputSetHostView);
		}

		public ProgressHUD () : this (UIScreen.MainScreen.Bounds)
		{
		}

		public ProgressHUD (CGRect frame) : base (frame)
		{
			UserInteractionEnabled = false;
			BackgroundColor = UIColor.Clear;
			Alpha = 0;
			AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			SetOSSpecificLookAndFeel ();

		}

		public void SetOSSpecificLookAndFeel ()
		{

			if (IsiOS7ForLookAndFeel) {
				HudBackgroundColour = UIColor.White.ColorWithAlpha (0.8f);
				HudForegroundColor = UIColor.FromWhiteAlpha (0.0f, 0.8f);
				HudStatusShadowColor = UIColor.FromWhiteAlpha (200f / 255f, 0.8f);
				_ringThickness = 1f;

			} else {
				HudBackgroundColour = UIColor.FromWhiteAlpha (0.0f, 0.8f);
				HudForegroundColor = UIColor.White;
				HudStatusShadowColor = UIColor.Black;
				_ringThickness = 6f;
			}
		}

		public enum MaskType
		{
			None = 1,
			Clear,
			Black,
			Gradient
		}

		public enum ToastPosition
		{
			Bottom = 1,
			Center,
			Top
		}

		public UIColor HudBackgroundColour = UIColor.FromWhiteAlpha (0.0f, 0.8f);
		public UIColor HudForegroundColor = UIColor.White;
		public UIColor HudStatusShadowColor = UIColor.Black;
		public UIColor HudToastBackgroundColor = UIColor.Clear;
		public UIFont HudFont = UIFont.BoldSystemFontOfSize (16f);
		public UITextAlignment HudTextAlignment = UITextAlignment.Center;
		public Ring Ring = new Ring ();
		static NSObject obj = new NSObject ();

		public void Show (string status = null, float progress = -1, MaskType maskType = MaskType.None, double timeoutMs = 1000)
		{
			obj.InvokeOnMainThread (() => ShowProgressWorker (progress, status, maskType, timeoutMs: timeoutMs));
		}

		public void Show (string cancelCaption, Action cancelCallback, string status = null, 
		                  float progress = -1, MaskType maskType = MaskType.None, double timeoutMs = 1000)
		{
			// Making cancelCaption optional hides the method via the overload
			if (string.IsNullOrEmpty (cancelCaption)) {
				cancelCaption = "Cancel";
			}
			obj.InvokeOnMainThread (() => ShowProgressWorker (progress, status, maskType, 
				cancelCaption: cancelCaption, cancelCallback: cancelCallback, timeoutMs: timeoutMs));
		}

		public void ShowContinuousProgress (string status = null, MaskType maskType = MaskType.None, double timeoutMs = 1000, UIImage img = null)
		{
			obj.InvokeOnMainThread (() => ShowProgressWorker (0, status, maskType, false, ToastPosition.Center, null, null, timeoutMs, true, img));
		}

		public void ShowContinuousProgressTest (string status = null, MaskType maskType = MaskType.None, double timeoutMs = 1000)
		{
			obj.InvokeOnMainThread (() => ShowProgressWorker (0, status, maskType, false, ToastPosition.Center, null, null, timeoutMs, true));
		}

		public void ShowToast (string status, MaskType maskType = MaskType.None, ToastPosition toastPosition = ToastPosition.Center, double timeoutMs = 1000)
		{
			obj.InvokeOnMainThread (() => ShowProgressWorker (status: status, textOnly: true, toastPosition: toastPosition, timeoutMs: timeoutMs, maskType: maskType));
		}

		public void SetStatus (string status)
		{
			obj.InvokeOnMainThread (() => SetStatusWorker (status));
		}

		public void ShowSuccessWithStatus (string status, double timeoutMs = 1000)
		{
			ShowImage (SuccessImage, status, timeoutMs);
		}

		public void ShowErrorWithStatus (string status, double timeoutMs = 1000)
		{
			ShowImage (ErrorImage, status, timeoutMs);
		}

		public void ShowImage (UIImage image, string status, double timeoutMs = 1000)
		{
			
			obj.InvokeOnMainThread (() => ShowImageWorker (image, status, TimeSpan.FromMilliseconds (timeoutMs)));
		}

		public void Dismiss ()
		{
			obj.InvokeOnMainThread (DismissWorker);
		}

		public UIImage ErrorImage {
			get {
				return (IsiOS7ForLookAndFeel ? UIImage.FromBundle ("error_7.png") : UIImage.FromBundle ("error.png"));
			}
		}

		public UIImage SuccessImage {
			get {
				return (IsiOS7ForLookAndFeel ? UIImage.FromBundle ("success_7.png") : UIImage.FromBundle ("success.png"));
			}
		}

		public bool IsVisible {
			get {
				return Alpha == 1;
			}
		}

		static ProgressHUD sharedHUD = null;

		public static ProgressHUD Shared {
			get {
				if (sharedHUD == null) {
					UIApplication.EnsureUIThread ();
					sharedHUD = new ProgressHUD (UIScreen.MainScreen.Bounds);
				}
				return sharedHUD;
			}
		}

		float _ringRadius = 14f;
		float _ringThickness = 6f;
		MaskType _maskType;
		NSTimer _fadeoutTimer;
		UIView _overlayView;
		UIView _hudView;
		UILabel _stringLabel;
		UIImageView _imageView;
		UIActivityIndicatorView _spinnerView;
		UIButton _cancelHud;
		NSTimer _progressTimer;
		float _progress;
		CAShapeLayer _backgroundRingLayer;
		CAShapeLayer _ringLayer;
		List<NSObject> _eventListeners;
		bool _displayContinuousImage;

		public float RingRadius {
			get { return _ringRadius; }
			set { _ringRadius = value; }
		}

		public float RingThickness {
			get { return _ringThickness; }
			set { _ringThickness = value; }
		}

		public override void Draw (CGRect rect)
		{
			using (var context = UIGraphics.GetCurrentContext ()) {
				switch (_maskType) {
				case MaskType.Black:
					UIColor.FromWhiteAlpha (0f, 0.5f).SetColor ();
					context.FillRect (Bounds);
					break;
				case MaskType.Gradient:
					nfloat[] colors = new nfloat[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.75f };
					nfloat[] locations = new nfloat[] { 0.0f, 1.0f };
					using (var colorSpace = CGColorSpace.CreateDeviceRGB ()) {
						using (var gradient = new CGGradient (colorSpace, colors, locations)) {
							var center = new CGPoint (Bounds.Size.Width / 2, Bounds.Size.Height / 2);
							float radius = Math.Min ((float)Bounds.Size.Width, (float)Bounds.Size.Height);
							context.DrawRadialGradient (gradient, center, 0, center, radius, CGGradientDrawingOptions.DrawsAfterEndLocation);
						}
					}

					break;
				}
			}
		}

		void ShowProgressWorker (float progress = -1, string status = null, MaskType maskType = MaskType.None, bool textOnly = false, 
		                         ToastPosition toastPosition = ToastPosition.Center, string cancelCaption = null, Action cancelCallback = null, 
		                         double timeoutMs = 1000, bool showContinuousProgress = false, UIImage displayContinuousImage = null)
		{

			Ring.ResetStyle(IsiOS7ForLookAndFeel, (IsiOS7ForLookAndFeel ? TintColor : UIColor.White));


			if (OverlayView.Superview == null) {
				var windows = UIApplication.SharedApplication.Windows;
				Array.Reverse (windows);
				foreach (UIWindow window in windows) {
					if (window.WindowLevel == UIWindowLevel.Normal && !window.Hidden) {
						window.AddSubview (OverlayView);
						break;
					}
				}
			}

		
			if (Superview == null)
				OverlayView.AddSubview (this);
			
			_fadeoutTimer = null;
			ImageView.Hidden = true;
			_maskType = maskType;
			_progress = progress;
			
			StringLabel.Text = status;

			if (!string.IsNullOrEmpty (cancelCaption)) {
				CancelHudButton.SetTitle (cancelCaption, UIControlState.Normal);
				CancelHudButton.TouchUpInside += delegate {
					Dismiss ();
					if (cancelCallback != null) {
						obj.InvokeOnMainThread (() => cancelCallback.DynamicInvoke (null));
						//cancelCallback.DynamicInvoke(null);
					}
				};
			}

			UpdatePosition (textOnly);

			if (showContinuousProgress) {
				if (displayContinuousImage != null) {
					_displayContinuousImage = true;
					ImageView.Image = displayContinuousImage;
					ImageView.Hidden = false;
				}

				RingLayer.StrokeEnd = 0.0f;
				StartProgressTimer (TimeSpan.FromMilliseconds (Ring.ProgressUpdateInterval));
			} else {
				if (progress >= 0) {
					ImageView.Image = null;
					ImageView.Hidden = false;

					SpinnerView.StopAnimating ();
					RingLayer.StrokeEnd = progress;
				} else if (textOnly) {
					CancelRingLayerAnimation ();
					SpinnerView.StopAnimating ();
				} else {
					CancelRingLayerAnimation ();
					SpinnerView.StartAnimating ();
				}
			}

			bool cancelButtonVisible = _cancelHud != null && _cancelHud.IsDescendantOfView (_hudView);

			// intercept user interaction with the underlying view
			if (maskType != MaskType.None || cancelButtonVisible) {
				OverlayView.UserInteractionEnabled = true;
				//AccessibilityLabel = status;
				//IsAccessibilityElement = true;
			} else {
				OverlayView.UserInteractionEnabled = false;
				//hudView.IsAccessibilityElement = true;
			}

			OverlayView.Hidden = false;
			this.toastPosition = toastPosition;
			PositionHUD (null);

		
			if (Alpha != 1) {
				RegisterNotifications ();
				HudView.Transform.Scale (1.3f, 1.3f);

				if (isClear) {
					Alpha = 1f;
					HudView.Alpha = 0f;
				}

				UIView.Animate (0.15f, 0, 
					UIViewAnimationOptions.AllowUserInteraction | UIViewAnimationOptions.CurveEaseOut | UIViewAnimationOptions.BeginFromCurrentState,
					delegate {
						HudView.Transform.Scale ((float)1 / 1.3f, (float)1f / 1.3f);
						if (isClear) {
							HudView.Alpha = 1f;
						} else {
							Alpha = 1f;
						}
					}, delegate {
					//UIAccessibilityPostNotification(UIAccessibilityScreenChangedNotification, string);

					if (textOnly)
						StartDismissTimer (TimeSpan.FromMilliseconds (timeoutMs));
				});

				SetNeedsDisplay ();
			}
		}

		void ShowImageWorker (UIImage image, string status, TimeSpan duration)
		{
			_progress = -1;
			CancelRingLayerAnimation ();

			//this should happen when Dismiss is called, but it happens AFTER the animation ends
			// so sometimes, the cancel button is left on :(
			if (_cancelHud != null) {
				_cancelHud.RemoveFromSuperview ();
				_cancelHud = null;
			}

			if (!IsVisible)
				Show ();

			ImageView.Image = image;
			ImageView.Hidden = false;
			StringLabel.Text = status;
			UpdatePosition ();
			SpinnerView.StopAnimating ();

			StartDismissTimer (duration);
		}

		void StartDismissTimer (TimeSpan duration)
		{
			#if __UNIFIED__
			_fadeoutTimer = NSTimer.CreateTimer (duration, timer => DismissWorker ());
			#else
			_fadeoutTimer = NSTimer.CreateTimer(duration, DismissWorker);
			#endif
			NSRunLoop.Main.AddTimer (_fadeoutTimer, NSRunLoopMode.Common);
		}

		void StartProgressTimer (TimeSpan duration)
		{

			if (_progressTimer == null) {
				#if __UNIFIED__
				_progressTimer = NSTimer.CreateRepeatingTimer (duration, timer => UpdateProgress ());
				#else
				_progressTimer = NSTimer.CreateRepeatingTimer(duration, UpdateProgress);
				#endif
				NSRunLoop.Current.AddTimer (_progressTimer, NSRunLoopMode.Common);
			}
		}

		void StopProgressTimer ()
		{
			if (_progressTimer != null) {
				_progressTimer.Invalidate ();
				_progressTimer = null;
			}
		}


		void UpdateProgress ()
		{
			obj.InvokeOnMainThread (delegate {
				if (!_displayContinuousImage) {
					ImageView.Image = null;
					ImageView.Hidden = false;
				}
				
				SpinnerView.StopAnimating ();
		
				if (RingLayer.StrokeEnd > 1) {
					RingLayer.StrokeEnd = 0.0f;
				} else {
					RingLayer.StrokeEnd += 0.1f;
				}
			});
		}

		void CancelRingLayerAnimation ()
		{
			CATransaction.Begin ();
			CATransaction.DisableActions = true;
			HudView.Layer.RemoveAllAnimations ();
			
			RingLayer.StrokeEnd = 0;
			if (RingLayer.SuperLayer != null) {
				RingLayer.RemoveFromSuperLayer ();
			}
			RingLayer = null;
			
			if (BackgroundRingLayer.SuperLayer != null) {
				BackgroundRingLayer.RemoveFromSuperLayer ();
			}
			BackgroundRingLayer = null;
			
			CATransaction.Commit ();
		}

		CAShapeLayer RingLayer {
			get {
				if (_ringLayer == null) {
					var center = new CGPoint (HudView.Frame.Width / 2, HudView.Frame.Height / 2);
					_ringLayer = CreateRingLayer (center, _ringRadius, _ringThickness, Ring.Color);
					HudView.Layer.AddSublayer (_ringLayer);
				}
				return _ringLayer;
			}
			set { _ringLayer = value; }
		}

		CAShapeLayer BackgroundRingLayer {
			get {
				if (_backgroundRingLayer == null) {
					var center = new CGPoint (HudView.Frame.Width / 2, HudView.Frame.Height / 2);
					_backgroundRingLayer = CreateRingLayer (center, _ringRadius, _ringThickness, Ring.BackgroundColor);
					_backgroundRingLayer.StrokeEnd = 1;
					HudView.Layer.AddSublayer (_backgroundRingLayer);
				}
				return _backgroundRingLayer;
			}
			set { _backgroundRingLayer = value; }
		}

		CGPoint PointOnCircle (CGPoint center, float radius, float angleInDegrees)
		{
			float x = radius * (float)Math.Cos (angleInDegrees * Math.PI / 180) + radius;
			float y = radius * (float)Math.Sin (angleInDegrees * Math.PI / 180) + radius;
			return new CGPoint (x, y);
		}

		UIBezierPath CreateCirclePath (CGPoint center, float radius, int sampleCount)
		{
			var smoothedPath = new UIBezierPath ();
			CGPoint startPoint = PointOnCircle (center, radius, -90);

			smoothedPath.MoveTo (startPoint);

			float delta = 360 / sampleCount;
			float angleInDegrees = -90;
			for (int i = 1; i < sampleCount; i++) {
				angleInDegrees += delta;
				var point = PointOnCircle (center, radius, angleInDegrees);
				smoothedPath.AddLineTo (point);
			}
			smoothedPath.AddLineTo (startPoint);
			return smoothedPath;
		}

		CAShapeLayer CreateRingLayer (CGPoint center, float radius, float lineWidth, UIColor color)
		{
			var smoothedPath = CreateCirclePath (center, radius, 72);
			var slice = new CAShapeLayer ();
			slice.Frame = new CGRect (center.X - radius, center.Y - radius, radius * 2, radius * 2);
			slice.FillColor = UIColor.Clear.CGColor;
			slice.StrokeColor = color.CGColor;
			slice.LineWidth = lineWidth;
			slice.LineCap = CAShapeLayer.JoinBevel;
			slice.LineJoin = CAShapeLayer.JoinBevel;
			slice.Path = smoothedPath.CGPath;
			return slice;
		
		}

		bool isClear {
			get {
				return (_maskType == ProgressHUD.MaskType.Clear || _maskType == ProgressHUD.MaskType.None);
			}
		}

		UIView OverlayView {
			get {
				if (_overlayView == null) {
					_overlayView = new UIView (UIScreen.MainScreen.Bounds);
					_overlayView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
					_overlayView.BackgroundColor = UIColor.Clear;
					_overlayView.UserInteractionEnabled = false;
				}
				return _overlayView;
			}
			set { _overlayView = value; }
		}

		UIView HudView {
			get {
				if (_hudView == null) {
					if (IsiOS7ForLookAndFeel) {
						_hudView = new UIToolbar ();
						(_hudView as UIToolbar).Translucent = true;
						(_hudView as UIToolbar).BarTintColor = HudBackgroundColour;
					} else {
						_hudView = new UIView ();
					}
					_hudView.Layer.CornerRadius = 10;
					_hudView.Layer.MasksToBounds = true;
					_hudView.BackgroundColor = HudBackgroundColour;
					_hudView.AutoresizingMask = (UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleTopMargin |
					UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleLeftMargin);

					AddSubview (_hudView);
				}
				return _hudView;
			}
			set { _hudView = value; }
		}

		UILabel StringLabel {
			get {
				if (_stringLabel == null) {
					_stringLabel = new UILabel ();
					_stringLabel.BackgroundColor = HudToastBackgroundColor;
					_stringLabel.AdjustsFontSizeToFitWidth = true;
					_stringLabel.TextAlignment = HudTextAlignment;
					_stringLabel.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
					_stringLabel.TextColor = HudForegroundColor;
					_stringLabel.Font = HudFont;
					if (!IsiOS7ForLookAndFeel) {
						_stringLabel.ShadowColor = HudStatusShadowColor;
						_stringLabel.ShadowOffset = new CGSize (0, -1);
					} 
					_stringLabel.Lines = 0;
				}
				if (_stringLabel.Superview == null) {
					HudView.AddSubview (_stringLabel);
				}
				return _stringLabel;
			}
			set { _stringLabel = value; }
		}

		UIButton CancelHudButton {
			get {
				if (_cancelHud == null) {
					_cancelHud = new UIButton ();

					_cancelHud.BackgroundColor = UIColor.Clear;
					_cancelHud.SetTitleColor (HudForegroundColor, UIControlState.Normal);
					_cancelHud.UserInteractionEnabled = true;
					_cancelHud.Font = HudFont;
					this.UserInteractionEnabled = true; 
				}
				if (_cancelHud.Superview == null) {
					HudView.AddSubview (_cancelHud);
					// Position the Cancel button at the bottom
					/* var hudFrame = HudView.Frame;
					var cancelFrame = _cancelHud.Frame;
					var x = ((hudFrame.Width - cancelFrame.Width)/2) + 0;
					var y = (hudFrame.Height - cancelFrame.Height - 10);
					_cancelHud.Frame = new RectangleF(x, y, cancelFrame.Width, cancelFrame.Height);
					HudView.SizeToFit();
					*/
				}
				return _cancelHud;
			}
			set {
				_cancelHud = value;
			}
		}

		UIImageView ImageView {
			get {
				if (_imageView == null) {
					_imageView = new UIImageView (new CGRect (0, 0, 28, 28));
				}
				if (_imageView.Superview == null) {
					HudView.AddSubview (_imageView);
				}
				return _imageView;
			}
			set { _imageView = value; }
		}

		UIActivityIndicatorView SpinnerView {
			get {
				if (_spinnerView == null) {
					_spinnerView = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge);
					_spinnerView.HidesWhenStopped = true;
					_spinnerView.Bounds = new CGRect (0, 0, 37, 37);
					_spinnerView.Color = HudForegroundColor;
				}

				if (_spinnerView.Superview == null)
					HudView.AddSubview (_spinnerView);

				return _spinnerView;
			}
			set { _spinnerView = value; }
		}

		float VisibleKeyboardHeight {
			get {
				foreach (var testWindow in UIApplication.SharedApplication.Windows) {
					if (testWindow.Class.Handle != Class.GetHandle("UIWindow")) {
						foreach (var possibleKeyboard in testWindow.Subviews) {
							if ((clsUIPeripheralHostView != null && possibleKeyboard.IsKindOfClass(clsUIPeripheralHostView)) ||
								(clsUIKeyboard != null && possibleKeyboard.IsKindOfClass(clsUIKeyboard))) {
								return (float)possibleKeyboard.Bounds.Size.Height;
							}
							else if (clsUIInputSetContainerView != null && possibleKeyboard.IsKindOfClass(clsUIInputSetContainerView)) {
								foreach (var possibleKeyboardSubview in possibleKeyboard.Subviews)
								{
									if (clsUIInputSetHostView != null && possibleKeyboardSubview.IsKindOfClass(clsUIInputSetHostView))
										return (float)possibleKeyboardSubview.Bounds.Size.Height;
								}
							}
						}
					}
				}

				return 0;
			}
		}

		void DismissWorker ()
		{
			SetFadeoutTimer (null);
			SetProgressTimer (null);

			UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseIn | UIViewAnimationOptions.AllowUserInteraction,
				delegate {
					HudView.Transform.Scale (0.8f, 0.8f);
					if (isClear) {
						HudView.Alpha = 0f;
					} else {
						Alpha = 0f;
					}
				}, delegate {
				if (Alpha == 0f || HudView.Alpha == 0f) {
					InvokeOnMainThread (delegate {
						Alpha = 0f;
						HudView.Alpha = 0f;

						//Removing observers
						UnRegisterNotifications ();
						NSNotificationCenter.DefaultCenter.RemoveObserver (this);

						Ring.ResetStyle (IsiOS7ForLookAndFeel, (IsiOS7ForLookAndFeel ? TintColor : UIColor.White));

						CancelRingLayerAnimation ();
						StringLabel.RemoveFromSuperview ();
						SpinnerView.RemoveFromSuperview ();
						ImageView.RemoveFromSuperview ();
						if (_cancelHud != null)
							_cancelHud.RemoveFromSuperview ();

						StringLabel = null;
						SpinnerView = null;
						ImageView = null;
						_cancelHud = null;

						HudView.RemoveFromSuperview ();
						HudView = null;
						OverlayView.RemoveFromSuperview ();
						OverlayView = null;
						this.RemoveFromSuperview ();

						if (IsiOS7ForLookAndFeel) {
							var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;
							if (rootController != null)
								rootController.SetNeedsStatusBarAppearanceUpdate ();
						}
					});
				}
			});
		}

		void SetStatusWorker (string status)
		{
			StringLabel.Text = status;
			UpdatePosition ();

		}

		void RegisterNotifications ()
		{
			if (_eventListeners == null) {
				_eventListeners = new List<NSObject> ();
			}
			_eventListeners.Add (NSNotificationCenter.DefaultCenter.AddObserver (UIApplication.DidChangeStatusBarOrientationNotification,
				PositionHUD));
			_eventListeners.Add (NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillHideNotification,
				PositionHUD));
			_eventListeners.Add (NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.DidHideNotification,
				PositionHUD));
			_eventListeners.Add (NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillShowNotification,
				PositionHUD));
			_eventListeners.Add (NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.DidShowNotification,
				PositionHUD));
		}

		void UnRegisterNotifications ()
		{
			if (_eventListeners != null) {
				NSNotificationCenter.DefaultCenter.RemoveObservers (_eventListeners);
				_eventListeners.Clear ();
				_eventListeners = null;
			}
		}

		void MoveToPoint (CGPoint newCenter, float angle)
		{
			HudView.Transform = CGAffineTransform.MakeRotation (angle); 
			HudView.Center = newCenter;
		}

		ToastPosition toastPosition = ToastPosition.Center;

		void PositionHUD (NSNotification notification)
		{

			nfloat keyboardHeight = 0;
			double animationDuration = 0;

			Frame = UIScreen.MainScreen.Bounds;

			UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;
			bool ignoreOrientation = UIDevice.CurrentDevice.CheckSystemVersion (8, 0);

			if (notification != null) {
				var keyboardFrame = UIKeyboard.FrameEndFromNotification (notification);
				animationDuration = UIKeyboard.AnimationDurationFromNotification (notification);
				
				if (notification.Name == UIKeyboard.WillShowNotification || notification.Name == UIKeyboard.DidShowNotification) {
					if (ignoreOrientation || IsPortrait (orientation))
						keyboardHeight = keyboardFrame.Size.Height;
					else
						keyboardHeight = keyboardFrame.Size.Width;
				} else
					keyboardHeight = 0;

			} else {
				keyboardHeight = VisibleKeyboardHeight;
			}
			
			CGRect orientationFrame = this.Window.Bounds;
			CGRect statusBarFrame = UIApplication.SharedApplication.StatusBarFrame;
			
			if (!ignoreOrientation && IsLandscape (orientation)) {
				orientationFrame.Size = new CGSize (orientationFrame.Size.Height, orientationFrame.Size.Width);
				statusBarFrame.Size = new CGSize (statusBarFrame.Size.Height, statusBarFrame.Size.Width);

			}
			
			var activeHeight = orientationFrame.Size.Height;
			
			if (keyboardHeight > 0)
				activeHeight += statusBarFrame.Size.Height * 2;
			
			activeHeight -= keyboardHeight;
			nfloat posY = (float)Math.Floor (activeHeight * 0.45);
			nfloat posX = orientationFrame.Size.Width / 2;
			nfloat textHeight = _stringLabel.Frame.Height / 2 + 40;

			switch (toastPosition) {
			case ToastPosition.Bottom:
				posY = activeHeight - textHeight;
				break;
			case ToastPosition.Center:
					// Already set above
				break;
			case ToastPosition.Top:
				posY = textHeight;
				break;
			default:
				break;
			}

			CGPoint newCenter;
			float rotateAngle;

			if (ignoreOrientation) {
				rotateAngle = 0.0f;
				newCenter = new CGPoint (posX, posY);
			} else {
				switch (orientation) { 
				case UIInterfaceOrientation.PortraitUpsideDown:
					rotateAngle = (float)Math.PI; 
					newCenter = new CGPoint (posX, orientationFrame.Size.Height - posY);
					break;
				case UIInterfaceOrientation.LandscapeLeft:
					rotateAngle = (float)(-Math.PI / 2.0f);
					newCenter = new CGPoint (posY, posX);
					break;
				case UIInterfaceOrientation.LandscapeRight:
					rotateAngle = (float)(Math.PI / 2.0f);
					newCenter = new CGPoint (orientationFrame.Size.Height - posY, posX);
					break;
				default: // as UIInterfaceOrientationPortrait
					rotateAngle = 0.0f;
					newCenter = new CGPoint (posX, posY);
					break;
				} 
			}
			
			if (notification != null) {
				UIView.Animate (animationDuration,
					0, UIViewAnimationOptions.AllowUserInteraction, delegate {
					MoveToPoint (newCenter, rotateAngle);
				}, null);

			} else {
				MoveToPoint (newCenter, rotateAngle);
			}
		}

		void SetFadeoutTimer (NSTimer newtimer)
		{
			if (_fadeoutTimer != null) {
				_fadeoutTimer.Invalidate ();
				_fadeoutTimer = null;
			}

			if (newtimer != null)
				_fadeoutTimer = newtimer;
		}


		void SetProgressTimer (NSTimer newtimer)
		{

			StopProgressTimer ();

			if (newtimer != null)
				_progressTimer = newtimer;
		}

		void UpdatePosition (bool textOnly = false)
		{
			nfloat hudWidth = 100f;
			nfloat hudHeight = 100f;
			nfloat stringWidth = 0f;
			nfloat stringHeight = 0f;
			nfloat stringHeightBuffer = 20f;
			nfloat stringAndImageHeightBuffer = 80f;

			CGRect labelRect = new CGRect ();
			
			string @string = StringLabel.Text;

			// False if it's text-only
			bool imageUsed = (ImageView.Image != null) || (ImageView.Hidden);
			if (textOnly) {
				imageUsed = false;
			}

			if (imageUsed) {
				hudHeight = stringAndImageHeightBuffer + stringHeight;
			} else {
				hudHeight = (textOnly ? stringHeightBuffer : stringHeightBuffer + 40);
			}

			if (!string.IsNullOrEmpty (@string)) {
				int lineCount = Math.Min (10, @string.Split ('\n').Length + 1);

				if (IsIOS7OrNewer) {
					var stringSize = new NSString (@string).GetBoundingRect (new CGSize (200, 30 * lineCount), NSStringDrawingOptions.UsesLineFragmentOrigin,
						                 new UIStringAttributes{ Font = StringLabel.Font },
						                 null);
					stringWidth = stringSize.Width;
					stringHeight = stringSize.Height;
				} else {
					var stringSize = new NSString (@string).StringSize (StringLabel.Font, new CGSize (200, 30 * lineCount));
					stringWidth = stringSize.Width;
					stringHeight = stringSize.Height;
				}


                

				hudHeight += stringHeight;

				if (stringWidth > hudWidth)
					hudWidth = (float)Math.Ceiling (stringWidth / 2) * 2;
				
				float labelRectY = imageUsed ? 66 : 9;
				
				if (hudHeight > 100) {
					labelRect = new CGRect (12, labelRectY, hudWidth, stringHeight);
					hudWidth += 24;
				} else {	
					hudWidth += 24;
					labelRect = new CGRect (0, labelRectY, hudWidth, stringHeight);
				}
			} 

			// Adjust for Cancel Button
			var cancelRect = new CGRect ();
			string @cancelCaption = _cancelHud == null ? null : CancelHudButton.Title (UIControlState.Normal);
			if (!string.IsNullOrEmpty (@cancelCaption)) {
				const int gap = 20;

				if (IsIOS7OrNewer) {
					var stringSize = new NSString (@cancelCaption).GetBoundingRect (new CGSize (200, 300), NSStringDrawingOptions.UsesLineFragmentOrigin,
						                 new UIStringAttributes{ Font = StringLabel.Font },
						                 null);
					stringWidth = stringSize.Width;
					stringHeight = stringSize.Height;
				} else {
					var stringSize = new NSString (@cancelCaption).StringSize (StringLabel.Font, new CGSize (200, 300));
					stringWidth = stringSize.Width;
					stringHeight = stringSize.Height;
				}

				if (stringWidth > hudWidth)
					hudWidth = (float)Math.Ceiling (stringWidth / 2) * 2;

				// Adjust for label
				nfloat cancelRectY = 0f;
				if (labelRect.Height > 0) {
					cancelRectY = labelRect.Y + labelRect.Height + (nfloat)gap;
				} else {
					if (string.IsNullOrEmpty (@string)) {
						cancelRectY = 76;
					} else {
						cancelRectY = (imageUsed ? 66 : 9);
					}

				}

				if (hudHeight > 100) {
					cancelRect = new CGRect (12, cancelRectY, hudWidth, stringHeight);
					labelRect = new CGRect (12, labelRect.Y, hudWidth, labelRect.Height);
					hudWidth += 24;
				} else {
					hudWidth += 24;
					cancelRect = new CGRect (0, cancelRectY, hudWidth, stringHeight);
					labelRect = new CGRect (0, labelRect.Y, hudWidth, labelRect.Height);
				}
				CancelHudButton.Frame = cancelRect;
				hudHeight += (cancelRect.Height + (string.IsNullOrEmpty (@string) ? 10 : gap));
			}

			HudView.Bounds = new CGRect (0, 0, hudWidth, hudHeight);
			if (!string.IsNullOrEmpty (@string))
				ImageView.Center = new CGPoint (HudView.Bounds.Width / 2, 36);
			else
				ImageView.Center = new CGPoint (HudView.Bounds.Width / 2, HudView.Bounds.Height / 2);


			StringLabel.Hidden = false;
			StringLabel.Frame = labelRect;

			if (!textOnly) {
				if (!string.IsNullOrEmpty (@string) || !string.IsNullOrEmpty(@cancelCaption)) {
					SpinnerView.Center = new CGPoint ((float)Math.Ceiling (HudView.Bounds.Width / 2.0f) + 0.5f, 40.5f);
					if (_progress != -1) {
						BackgroundRingLayer.Position = RingLayer.Position = new CGPoint (HudView.Bounds.Width / 2, 36f);
					}
				} else {
					SpinnerView.Center = new CGPoint ((float)Math.Ceiling (HudView.Bounds.Width / 2.0f) + 0.5f, (float)Math.Ceiling (HudView.Bounds.Height / 2.0f) + 0.5f);
					if (_progress != -1) {
						BackgroundRingLayer.Position = RingLayer.Position = new CGPoint (HudView.Bounds.Width / 2, HudView.Bounds.Height / 2.0f + 0.5f);
					}
				}
			}
		}

		public bool IsLandscape (UIInterfaceOrientation orientation)
		{
			return (orientation == UIInterfaceOrientation.LandscapeLeft || orientation == UIInterfaceOrientation.LandscapeRight);
		}

		public bool IsPortrait (UIInterfaceOrientation orientation)
		{
			return (orientation == UIInterfaceOrientation.Portrait || orientation == UIInterfaceOrientation.PortraitUpsideDown);
		}

		public bool IsiOS7ForLookAndFeel {
			get {
				if (ForceiOS6LookAndFeel)
					return false;

				return UIDevice.CurrentDevice.CheckSystemVersion (7, 0);
			}
		}

		public bool IsIOS7OrNewer {
			get {
				return UIDevice.CurrentDevice.CheckSystemVersion (7, 0);
			}
		}

		bool forceiOS6LookAndFeel = false;

		public bool ForceiOS6LookAndFeel {
			get { return forceiOS6LookAndFeel; }
			set {
				forceiOS6LookAndFeel = value;
				SetOSSpecificLookAndFeel ();
			}
		}
	}
}

