using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        public override Task AlertAsync(string message, string title = null, string okText = null,
            CancellationToken? cancelToken = null)
        {
            MessageBox.Show(message, title);
            return Task.FromResult(true);
        }

        public override IDisposable Toast(string title, TimeSpan? dismissTimer = null)
        {
            MessageBox.Show(title);
            return null;
        }
    }
}