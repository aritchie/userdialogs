//
// DefaultMessageBarStyleSheet.cs
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



using UIKit;

namespace MessageBar
{
	public class MessageBarStyleSheet {

		const float Alpha = 0.96f;
		const string ErrorIcon = "icon-error.png";
		const string SuccessIcon = "icon-success.png";
		const string InfoIcon = "icon-info.png";

		readonly UIColor errorBackgroundColor = null;
		readonly UIColor successBackgroundColor = null;
		readonly UIColor infoBackgroundColor = null;
		readonly UIColor errorStrokeColor = null;
		readonly UIColor successStrokeColor = null;
		readonly UIColor infoStrokeColor = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="MessageBar.MessageBarStyleSheet"/> class.
		/// </summary>
		public MessageBarStyleSheet ()
		{
			errorBackgroundColor = UIColor.FromRGBA (1.0f, 0.611f, 0.0f, Alpha);
			successBackgroundColor = UIColor.FromRGBA (0.0f, 0.831f, 0.176f, Alpha);
			infoBackgroundColor = UIColor.FromRGBA (0.0f, 0.482f, 1.0f, Alpha);
			errorStrokeColor = UIColor.FromRGBA (0.949f, 0.580f, 0.0f, 1.0f);
			successStrokeColor = UIColor.FromRGBA (0.0f, 0.772f, 0.164f, 1.0f);
			infoStrokeColor = UIColor.FromRGBA (0.0f, 0.415f, 0.803f, 1.0f);
		}

		/// <summary>
		/// Provides the background colour for message type
		/// </summary>
		/// <returns>The background colour for message type.</returns>
		/// <param name="type">Message type</param>
		public virtual UIColor BackgroundColorForMessageType (MessageType type)
		{
			UIColor backgroundColor = null;
			switch (type) {
			case MessageType.Error:
				backgroundColor = errorBackgroundColor;
				break;
			case MessageType.Success:
				backgroundColor = successBackgroundColor;
				break;
			case MessageType.Info:
				backgroundColor = infoBackgroundColor;
				break;
			}

			return backgroundColor;
		}

		/// <summary>
		/// Provides the stroke colour for message type
		/// </summary>
		/// <returns>The stroke colour for message type.</returns>
		/// <param name="type">Message type</param>
		public virtual UIColor StrokeColorForMessageType (MessageType type)
		{
			UIColor strokeColor = null;
			switch (type) {
			case MessageType.Error:
				strokeColor = errorStrokeColor;
				break;
			case MessageType.Success:
				strokeColor = successStrokeColor;
				break;
			case MessageType.Info:
				strokeColor = infoStrokeColor;
				break;
			}

			return strokeColor;
		}

		/// <summary>
		/// Provides the icon for message type
		/// </summary>
		/// <returns>The icon for message type.</returns>
		/// <param name="type">Message type</param>
		public virtual UIImage IconImageForMessageType (MessageType type)
		{
			UIImage iconImage = null;
			switch (type) {
			case MessageType.Error:
				iconImage = UIImage.FromBundle (ErrorIcon);
				break;
			case MessageType.Success:
				iconImage = UIImage.FromBundle (SuccessIcon);
				break;
			case MessageType.Info:
				iconImage = UIImage.FromBundle (InfoIcon);
				break;
			}

			return iconImage;
		}
	}
}
