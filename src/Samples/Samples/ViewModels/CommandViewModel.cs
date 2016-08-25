using System;
using System.Windows.Input;


namespace Samples.ViewModels
{
    public class CommandViewModel
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
    }
}
