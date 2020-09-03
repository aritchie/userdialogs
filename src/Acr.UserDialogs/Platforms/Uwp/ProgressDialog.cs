using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Acr.UserDialogs.Infrastructure;
using System.Threading.Tasks;

namespace Acr.UserDialogs
{
    public class ProgressDialog : IProgressDialog, INotifyPropertyChanged
    {
        readonly ProgressDialogConfig config;
        ProgressContentDialog dialog;

        readonly Func<Action, Task> dispatcher;

        public ProgressDialog(ProgressDialogConfig config, Func<Action, Task> dispatcher = null)
        {
            this.config = config;
            this.Cancel = new Command(() => config.OnCancel?.Invoke());

            this.dispatcher = dispatcher ?? new Func<Action, Task>(x => CoreApplication
                .MainView
                .CoreWindow
                .Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, () => x())
                .AsTask()
            );
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
                this.Change("PercentCompleteString");
            }
        }

        public string PercentCompleteString
        {
            get { return this.percent + "%"; }
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
            this.dispatcher.Invoke(action);
        }
    }
}
