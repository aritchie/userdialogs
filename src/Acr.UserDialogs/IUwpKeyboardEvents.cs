using System;


namespace Acr.UserDialogs
{
    public interface IUwpKeyboardEvents
    {
        bool UwpCancelOnEscKey { get; set; }
        bool UwpSubmitOnEnterKey { get; set; }
    }
}
