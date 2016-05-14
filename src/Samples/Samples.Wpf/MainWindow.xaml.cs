using System;
using System.Windows;
using Acr.UserDialogs;
using Samples.ViewModels;


namespace Samples.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new StandardViewModel(UserDialogs.Instance);
            this.InitializeComponent();
        }
    }
}
