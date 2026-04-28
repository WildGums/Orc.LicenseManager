namespace Orc.LicenseManager.Client.Example;

using System;
using System.Globalization;
using System.Windows;
using Catel;
using Catel.IoC;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orc.Automation;
using Orc.Controls;
using Orc.FileSystem;
using Orc.LicenseManager.Client.Example.Views;
using Orc.LicenseManager.Services;
using Orc.Serialization.Json;
using Orc.SystemInfo;
using Orc.Theming;
using Orchestra;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
#pragma warning disable IDISP006 // Implement IDisposable
    private readonly IHost _host;
#pragma warning restore IDISP006 // Implement IDisposable

    public App()
    {
        var hostBuilder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddCatelCore();
                services.AddCatelMvvm();
                services.AddOrcAutomation();
                services.AddOrcControls();
                services.AddOrcFileSystem();
                services.AddOrcLicenseManager();
                services.AddOrcLicenseManagerXaml();
                services.AddOrcSerializationJson();
                services.AddOrcSystemInfo();
                services.AddOrcTheming();
                services.AddOrchestraCore();

                // TODO: Pick the right expiration behavior
                //serviceLocator.RegisterType<IExpirationBehavior, PreventUsageOfAnyVersionExpirationBehavior>();
                services.AddSingleton<IExpirationBehavior, PreventUsageOfLaterReleasedVersionsExpirationBehavior>();
                services.AddSingleton<ILicenseInfoService, LicenseInfoService>();

                services.AddLogging(x =>
                {
                    x.AddConsole();
                    x.AddDebug();
                });
            });

        _host = hostBuilder.Build();

        IoCContainer.ServiceProvider = _host.Services;
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceProvider = IoCContainer.ServiceProvider;

        serviceProvider.CreateTypesThatMustBeConstructedAtStartup();

        var languageService = serviceProvider.GetRequiredService<ILanguageService>();

        // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
        // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
        // we use .CurrentCulture for the sake of the demo
        languageService.PreferredCulture = CultureInfo.CurrentCulture;
        languageService.FallbackCulture = new CultureInfo("en-US");

        this.ApplyTheme();

        // A Valid license for this application ID aka Public Key can be found in data/FakeLicenseInfo.txt
        var applicationIdService = serviceProvider.GetRequiredService<IApplicationIdService>();
        applicationIdService.ApplicationId = "MIIBKjCB4wYHKoZIzj0CATCB1wIBATAsBgcqhkjOPQEBAiEA/////wAAAAEAAAAAAAAAAAAAAAD///////////////8wWwQg/////wAAAAEAAAAAAAAAAAAAAAD///////////////wEIFrGNdiqOpPns+u9VXaYhrxlHQawzFOw9jvOPD4n0mBLAxUAxJ02CIbnBJNqZnjhE50mt4GffpAEIQNrF9Hy4SxCR/i85uVjpEDydwN9gS3rM6D0oTlF2JjClgIhAP////8AAAAA//////////+85vqtpxeehPO5ysL8YyVRAgEBA0IABHGri0/ra3c4Fi+x0pLZG0ZGPeVxOps9Lz6CY1IKMf9SKsmXNzWrUh0dPEU8qeOe4DNvlgOUsxDYznFPWS1PpLg=";

        var mainWindow = ActivatorUtilities.CreateInstance<MainWindow>(_host.Services);
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }

        base.OnExit(e);
    }
}
