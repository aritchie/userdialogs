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
            services.AddSingleton(UserDialogs.Instance);
            #if __ANDROID__
            // hope for the best casting for now
            UserDialogs.Init(() => (Android.App.Activity)ContextHelper.Current);
            #endif
        });
}

