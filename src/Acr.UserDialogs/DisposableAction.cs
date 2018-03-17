using System;


namespace Acr.UserDialogs
{
    public class DisposableAction : IDisposable
    {
        readonly Action action;


        public DisposableAction(Action action)
        {
            this.action = action;
        }


        public void Dispose()
        {
            this.action();
            GC.SuppressFinalize(this);
        }
    }
}
