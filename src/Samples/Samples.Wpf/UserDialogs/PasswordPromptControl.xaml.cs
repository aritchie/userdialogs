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
    public partial class PasswordPromptControl : ContentControl
    {
        public PasswordPromptControl()
        {
            InitializeComponent();
            this.PasswordEdit.PasswordChanged += (s, e) =>
            {
                this.Placeholder.Visibility = String.IsNullOrEmpty(this.PasswordEdit.Password) ? Visibility.Visible : Visibility.Collapsed;
            };
        }
    }
}
