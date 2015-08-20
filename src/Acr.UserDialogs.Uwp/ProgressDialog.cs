using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;


namespace Acr.UserDialogs {

    public class ProgressDialog : IProgressDialog, INotifyPropertyChanged {
        readonly ProgressContentDialog dialog;
        Action cancelAction;


        public ProgressDialog() {
            this.CancelVisibility = Visibility.Collapsed;
            this.IsIndeterministic = true;
            this.dialog = new ProgressContentDialog { DataContext = this };
            this.Cancel = new Command(() => this.cancelAction?.Invoke());
        }


        public bool IsIndeterministic { get; private set; }

        bool deter;
        public bool IsDeterministic {
            get { return this.deter; }
            set {
                this.deter = value;
                this.IsIndeterministic = !value;
                this.TextPercentVisibility = value
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                this.Change();
                this.Change("IsIndeterministic");
            }
        }


        public bool IsShowing { get; private set; }

        public MaskType MaskType { get; set; }


        int percent;
        public int PercentComplete {
            get { return this.percent; }
            set {
                if (value > 100)
                    this.percent = 100;
                else if (value < 0)
                    this.percent = 0;
                else
                    this.percent = value;
                this.Change();
            }
        }


        Visibility textVis;
        public Visibility TextPercentVisibility {
            get { return this.textVis; }
            private set {
                this.textVis = value;
                this.Change();
            }
        }


        string title;
        public string Title {
            get { return this.title; }
            set {
                this.title = value;
                this.Change();
            }
        }


        public void Dispose() {
            this.Hide();
        }


        public void Hide() {
            if (!this.IsShowing)
                return;

            this.IsShowing = false;
            this.Dispatch(() => this.dialog.Hide());
        }


        public void SetCancel(Action onCancel, string cancelText = "Cancel") {
            this.CancelVisibility = Visibility.Visible;
            this.cancelAction = onCancel;
            this.CancelText = cancelText;
        }


        public void Show() {
            if (this.IsShowing)
                return;

            this.IsShowing = true;
            this.Dispatch(() => this.dialog.ShowAsync());
        }


        void Change([CallerMemberName] string property = null) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        public ICommand Cancel { get; }


        string cancelText;
        public string CancelText {
            get { return this.cancelText; }
            set {
                this.cancelText = value;
                this.Change();
            }
        }


        Visibility cancelVisible;
        public Visibility CancelVisibility {
            get { return this.cancelVisible; }
            private set {
                this.cancelVisible = value;
                this.Change();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void Dispatch(Action action) {
            CoreWindow
                .GetForCurrentThread()
                .Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
