using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Acr.UserDialogs;


public static class AppBuilderExtensions
{
    public static IApplicationBuilder AddUserDialogs(this IApplicationBuilder builder)
        => builder.Configure((host, _) => host.AddUserDialogs());

    public static IHostBuilder AddUserDialogs(this IHostBuilder builder)
        => builder.ConfigureServices(services =>
        {
#if __ANDROID__
            UserDialogs.Init(() => (Android.App.Activity)ContextHelper.Current);
            services.AddSingleton(UserDialogs.Instance);
        
#elif __IOS__
            services.AddSingleton(UserDialogs.Instance);
#else
            throw new ApplicationException("This plugin only works with .NET 8.0 for Android, iOS, and Mac Catalyst.  You are calling this, but it isn't from one of those targets!");
#endif
        });
}
