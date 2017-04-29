using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;


namespace Acr.UserDialogs
{
    public class ProgressDialog : IProgressDialog, INotifyPropertyChanged
    {
        readonly ProgressDialogConfig config;
        ProgressContentDialog dialog;


        public ProgressDialog(ProgressDialogConfig config)
        {
            this.config = config;
            this.Cancel = new Command(() => config.OnCancel?.Invoke());
        }


        public bool IsShowing { get; private set; }


        int percent;
        public int PercentComplete
        {
            get { return this.percent; }
            set
            {
                if (value > 100)
                    this.percent = 100;
                else if (value < 0)
                    this.percent = 0;
                else
                    this.percent = value;
                this.Change();
            }
        }


        public string CancelText => this.config.CancelText;
        public bool IsIndeterministic => !this.config.IsDeterministic;
        public Visibility TextPercentVisibility => this.config.IsDeterministic ? Visibility.Visible : Visibility.Collapsed;


        string title;
        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.Change();
            }
        }


        public void Dispose()
        {
            this.Hide();
        }


        public void Hide()
        {
            if (!this.IsShowing)
                return;

            this.IsShowing = false;
            this.Dispatch(() => this.dialog.Hide());
        }


        public void Show()
        {
            if (this.IsShowing)
                return;

            this.IsShowing = true;
            this.Dispatch(() =>
            {
                if (this.dialog == null)
                    this.dialog = new ProgressContentDialog { DataContext = this };

                this.dialog.ShowAsync();
            });
        }


        void Change([CallerMemberName] string property = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        public ICommand Cancel { get; }
        public Visibility CancelVisibility => this.config.OnCancel == null
            ? Visibility.Collapsed
            : Visibility.Visible;


        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void Dispatch(Action action)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
