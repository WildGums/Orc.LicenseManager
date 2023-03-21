namespace Orc.LicenseManager;

using System;
using System.Windows;

internal static class ResourceHelper
{
    private static bool InitializedStyles = false;

    public static void EnsureStyles()
    {
        if (InitializedStyles)
        {
            return;
        }

        var app = System.Windows.Application.Current;
        if (app is null)
        {
            return;
        }

        var resourceDictionary = new ResourceDictionary()
        {
            Source = new Uri("/Orc.LicenseManager.Client.WPF;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
        };

        app.Resources.MergedDictionaries.Add(resourceDictionary);

        InitializedStyles = true;
    }
}
