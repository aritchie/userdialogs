//
// MessageBarManager.cs
//
// Author:
//       Prashant Cholachagudda <pvc@outlook.com>
//
// Copyright (c) 2013 Prashant Cholachagudda
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.



using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Collections.Generic;
using System.Threading;

namespace MessageBar
{
	interface IStyleSheetProvider
	{
		/// <summary>
		/// Stylesheet for message view.
		/// </summary>
		/// <returns>The style sheet for message view.</returns>
		/// <param name="messageView">Message view.</param>
		MessageBarStyleSheet StyleSheetForMessageView (MessageView messageView);
	}

	public class MessageBarManager : NSObject, IStyleSheetProvider
	{
		public static MessageBarManager SharedInstance {
			get{ return instance ?? (instance = new MessageBarManager ()); }
		}

		MessageBarManager ()
		{
			messageBarQueue = new Queue<MessageView> ();
			MessageVisible = false;
			MessageBarOffset = 20;
			styleSheet = new MessageBarStyleSheet ();
		}

		nfloat MessageBarOffset { get; set; }

		bool MessageVisible{ get; set; }

		Queue<MessageView> MessageBarQueue {
			get{ return messageBarQueue; }
		}

		/// <summary>
		/// Gets or sets the style sheet.
		/// </summary>
		/// <value>The style sheet.</value>
		public MessageBarStyleSheet StyleSheet {
			get {
				return styleSheet;
			}
			set {
				if (value != null) {
					styleSheet = value;
				}
			}
		}

		UIView MessageWindowView{
			get{
				return  GetMessageBarViewController ().View;
			}
		}

		nfloat initialPosition = 0;
		nfloat showPosition = 0;
		/// <summary>
		/// Show all messages at the bottom.
		/// </summary>
		/// <value><c>true</c> if show at the bottom; otherwise, <c>false</c>.</value>
		public bool ShowAtTheBottom {get; set;}
		/// <summary>
		/// Discard all repeated messages enqueued by a freak finger.
		/// </summary>
		/// <value><c>true</c> if discard repeated; otherwise, <c>false</c>.</value>
		public bool DiscardRepeated {get; set;}
		MessageView lastMessage;

		/// <summary>
		/// Shows the message
		/// </summary>
		/// <param name="title">Messagebar title</param>
		/// <param name="description">Messagebar description</param>
		/// <param name="type">Message type</param>
		public void ShowMessage (string title, string description, MessageType type)
		{
			ShowMessage (title, description, type, null);
		}

		/// <summary>
		/// Shows the message
		/// </summary>
		/// <param name="title">Messagebar title</param>
		/// <param name="description">Messagebar description</param>
		/// <param name="type">Message type</param>
		/// <param name = "onDismiss">OnDismiss callback</param>
		public void ShowMessage (string title, string description, MessageType type, Action onDismiss)
		{
			var messageView = new MessageView (title, description, type);
			messageView.StylesheetProvider = this;
			messageView.OnDismiss = onDismiss;
			messageView.Hidden = true;

			//UIApplication.SharedApplication.KeyWindow.InsertSubview (messageView, 1);

			MessageWindowView.AddSubview (messageView);
			MessageWindowView.BringSubviewToFront (messageView);

			MessageBarQueue.Enqueue (messageView);

			if (!MessageVisible) {
				ShowNextMessage ();
			}
		}

		void ShowNextMessage ()
		{
			MessageView messageView = GetNextMessage ();

			if (messageView != null) {
				MessageVisible = true;

				if (ShowAtTheBottom) {
					initialPosition = MessageWindowView.Bounds.Height + messageView.Height;
					showPosition = MessageWindowView.Bounds.Height - messageView.Height;
				} else {
					initialPosition = MessageWindowView.Bounds.Y - messageView.Height;
					showPosition = MessageWindowView.Bounds.Y + MessageBarOffset;
				}

				messageView.Frame = new CGRect (0, initialPosition, messageView.Width, messageView.Height);
				messageView.Hidden = false;
				messageView.SetNeedsDisplay ();

				var gest = new UITapGestureRecognizer (MessageTapped);
				messageView.AddGestureRecognizer (gest);
				if (messageView == null)
					return; 

				UIView.Animate (DismissAnimationDuration, 
					() => 
					messageView.Frame = new CGRect (messageView.Frame.X, 
						showPosition, 
						messageView.Width, messageView.Height)
				);

				//Need a better way of dissmissing the method
				var dismiss = new Timer (DismissMessage, messageView, TimeSpan.FromSeconds (DisplayDelay),
					TimeSpan.FromMilliseconds (-1));
			}
		}

		MessageView GetNextMessage ()
		{
			MessageView message = null;

			if (!DiscardRepeated)
				return MessageBarQueue.Dequeue ();

			while (MessageBarQueue.Count > 0) {
				message = MessageBarQueue.Dequeue ();

				if (IsEqualLastMessage (message))
					message = null;
				else
					break;
			}

			lastMessage = message;

			return message;
		}


		bool IsEqualLastMessage(MessageView message){
			return message.Equals(lastMessage);
		}

		/// <summary>
		/// Hides all messages
		/// </summary>
		public void HideAll ()
		{
			MessageView currentMessageView = null;
			var subviews = MessageWindowView.Subviews;

			foreach (UIView subview in subviews) {
				var view = subview as MessageView;
				if (view != null) {
					currentMessageView = view;
					currentMessageView.RemoveFromSuperview ();
				}
			}

			MessageVisible = false;
			MessageBarQueue.Clear ();
			CancelPreviousPerformRequest (this);
		}

		void MessageTapped (UIGestureRecognizer recognizer)
		{
			var view = recognizer.View as MessageView;
			if (view != null) {
				DismissMessage (view);
			}
		}

		void DismissMessage (object messageView)
		{
			var view = messageView as MessageView;
			if (view != null) {
				InvokeOnMainThread (() =>	DismissMessage (view));
			}
		}

		void DismissMessage (MessageView messageView)
		{
			if (messageView != null && !messageView.Hit) {

				messageView.Hit = true;
				UIView.Animate (DismissAnimationDuration, 
					delegate {
						messageView.Frame = new CGRect (
							messageView.Frame.X, 
							initialPosition, 
							messageView.Frame.Width, messageView.Frame.Height);
					}, 
					delegate {
						MessageVisible = false;
						messageView.RemoveFromSuperview ();

						var action = messageView.OnDismiss;
						if (action != null) {
							action ();
						}

						if (MessageBarQueue.Count > 0) {
							ShowNextMessage ();
						}else{
							lastMessage = null;
						}
					}
				);
			}
		}

		MessageBarViewController GetMessageBarViewController ()
		{
			if (messageWindow == null) {
				messageWindow = new MessageWindow () {
					Frame = UIApplication.SharedApplication.KeyWindow.Frame,
					Hidden = false,
					WindowLevel = UIWindowLevel.Normal,
					BackgroundColor = UIColor.Clear,
					RootViewController = new MessageBarViewController()
				};

			}

			return (MessageBarViewController) messageWindow.RootViewController;
		}


		MessageWindow messageWindow;
		const float DisplayDelay = 3.0f;
		const float DismissAnimationDuration = 0.25f;
		MessageBarStyleSheet styleSheet;
		readonly Queue<MessageView> messageBarQueue;
		static MessageBarManager instance;

		#region IStyleSheetProvider implementation

		public MessageBarStyleSheet StyleSheetForMessageView (MessageView messageView)
		{
			return StyleSheet;
		}

		#endregion

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			instance = null;
		}
	}
}
 