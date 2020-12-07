using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Xamarin.Forms.Platform.WPF.Controls;

namespace Acr.UserDialogs
{
    public sealed class ProgressDialog : IProgressDialog, INotifyPropertyChanged
    {
        readonly ProgressDialogConfig config;
        readonly UserDialogsImpl parent;
        readonly FormsContentDialog dialog;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get => dialog.Title as string;
            set => dialog.Title = value;
        }

        int percentComplete = 0;
        public int PercentComplete
        {
            get => percentComplete;
            set
            {
                percentComplete = Math.Max(0, Math.Min(100, value));
                Change();
                Change(nameof(PercentCompleteString));
            }
        }
        public string PercentCompleteString => $"{percentComplete:0}%";

        public bool IsShowing { get; private set; }

        public bool IsIndeterministic => !this.config.IsDeterministic;
        public Visibility TextPercentVisibility => this.config.IsDeterministic ? Visibility.Visible : Visibility.Collapsed;

        public ProgressDialog(ProgressDialogConfig config, UserDialogsImpl parent)
        {
            this.config = config;
            this.parent = parent;

            this.dialog = new FormsContentDialog()
            {
                DataContext = this,
                Title = config.Title,
                Content = new ProgressDialogControl(),
                PrimaryButtonText = config.CancelText,
                IsPrimaryButtonEnabled = !String.IsNullOrEmpty(config.CancelText) && config.OnCancel != null
            };
            this.dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;
        }

        private void Dialog_PrimaryButtonClick(object sender, LightContentDialogButtonClickEventArgs e)
        {
            e.Cancel = true;
            config.OnCancel?.Invoke();
        }

        public void Dispose()
        {
            parent.HideContentDialog();
        }

        public void Hide()
        {
            parent.HideContentDialog();
            IsShowing = false;
        }

        public void Show()
        {
            parent.ShowContentDialog(this.dialog);
            IsShowing = true;
        }

        void Change([CallerMemberName] string property = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
