namespace Acr.UserDialogs
{
    public static partial class UserDialogs
    {
        private static IUserDialogs currentInstance;

        public static IUserDialogs Instance
        {
            get
            {
                currentInstance = currentInstance ?? new UserDialogsImpl();
                return currentInstance;
            }
            set => currentInstance = value;
        }
    }
}