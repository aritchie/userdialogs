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
    public partial class ActionSheetControl : ContentControl
    {
        public ActionSheetControl()
        {
            InitializeComponent();
            this.List.SelectionChanged += (s, e) => { (this.List.SelectedItem as ActionSheetOption)?.Action?.Invoke(); };
        }
    }
}
