using System;


namespace Acr.UserDialogs
{
    public class DialogResult<T>
    {
        public DialogResult(DialogChoice choice, T value)
        {
            this.Choice = choice;
            this.Value = value;
        }


        public DialogChoice Choice { get; }
        public T Value { get; }
    }
}
