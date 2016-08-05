using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;


namespace Acr.UserDialogs {

    public class ProgressPopUp : CustomMessageBox {

        private readonly TextBlock percentText = new TextBlock {
            Visibility = Visibility.Collapsed,
            Margin = new Thickness(0, 10, 0, 0)
        };
        private readonly ProgressBar progressBar = new ProgressBar {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Margin = new Thickness(0, 10, 0, 0)
        };


        public ProgressPopUp() {
            this.IsRightButtonEnabled = false;
            this.IsLeftButtonEnabled = false;

            var stack = new StackPanel {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            stack.Children.Add(this.progressBar);
            stack.Children.Add(this.percentText);
            this.Content = stack;
        }


        public bool IsIndeterminate {
            get { return this.progressBar.IsIndeterminate; }
            set {
                this.progressBar.IsIndeterminate = value;
                this.percentText.Visibility = value
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
        }


        public string LoadingText {
            get { return this.Caption; }
            set { this.Caption = value; }
        }


        public string CompletionText {
            get { return this.percentText.Text; }
            set { this.percentText.Text = value; }
        }


        public int PercentComplete {
            get { return Convert.ToInt32(this.progressBar.Value); }
            set { this.progressBar.Value = value; }
        }


        public void SetCancel(Action action, string cancelText) {
            this.Dismissed += (sender, args) => {
                if (args.Result == CustomMessageBoxResult.RightButton)
                    action();
            };
            this.RightButtonContent = cancelText;
            this.IsRightButtonEnabled = true;
        }
    }
}
