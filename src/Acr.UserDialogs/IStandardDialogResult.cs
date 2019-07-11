using System;


namespace Acr.UserDialogs
{
    public interface IStandardDialogResult<T>
    {
        bool Ok { get; }
        T Value { get; }
    }
}
