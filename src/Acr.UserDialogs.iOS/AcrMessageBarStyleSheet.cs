using System;
using MessageBar;
using Splat;
using UIKit;


namespace Acr.UserDialogs {

    public class AcrMessageBarStyleSheet : MessageBarStyleSheet {
        readonly ToastConfig config;


        public AcrMessageBarStyleSheet(ToastConfig config) {
            this.config = config;
        }


        public override UIColor StrokeColorForMessageType(MessageType type) {
            return this.config.TextColor.ToNative();
        }


        public override UIColor BackgroundColorForMessageType(MessageType type) {
            return this.config.BackgroundColor.ToNative();
        }


        public override UIImage IconImageForMessageType(MessageType type) {
            // ignore incoming type
            if (this.config.Icon == null)
                return base.IconImageForMessageType(type);

            return this.config.Icon.ToNative();
        }
    }
}