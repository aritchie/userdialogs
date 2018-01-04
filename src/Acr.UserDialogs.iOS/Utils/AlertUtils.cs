using System;
using UIKit;
namespace Acr.UserDialogs.Utils
{
    //TODO check if the values are correct
    public static class AlertUtils
    {
        readonly static nfloat DefaultSpacing = 105.0f;

        public static nfloat GetAlertWidth()
        {
            nfloat deviceHeight = UIScreen.MainScreen.Bounds.Height;
            nfloat deviceWidth = UIScreen.MainScreen.Bounds.Width;
            nfloat spacing = DefaultSpacing;

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                if (deviceHeight == 1366)
                {
                    spacing = DefaultSpacing + 600.0f;
                }
                else
                {
                    spacing = DefaultSpacing + 350.0f;
                }

            }
            else if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                if (deviceHeight == 480)
                {
                    // iPhone Classic
                    spacing = 55.0f;
                } 
                else if (deviceHeight == 568)
                {
                    // iPhone 5
                    spacing = 65.0f;
                }
                else if (deviceHeight == 736)
                {
                    // iPhone 6/7 Plus
                    spacing = 130.0f;
                }
            }
            return deviceWidth - spacing;
        }
    }
}
