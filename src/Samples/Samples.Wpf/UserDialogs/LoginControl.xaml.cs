using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Acr.UserDialogs
{
    /// <summary>
    /// Interaction logic for XAML
    /// </summary>
    public partial class LoginControl : ContentControl
    {

        public string LoginValue 
        {
            get => TextEdit.Text;
            set => TextEdit.Text = value;
        }

        public string Password
        {
            get => PasswordEdit.Password;
            set => PasswordEdit.Password = value;
        }

        public LoginControl()
        {
            InitializeComponent();
            this.PasswordEdit.PasswordChanged += (s, e) =>
            {
                this.Placeholder.Visibility = String.IsNullOrEmpty(this.PasswordEdit.Password) ? Visibility.Visible : Visibility.Collapsed;
            };
        }
    }
}
