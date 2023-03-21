namespace Orc.LicenseManager.Views;

using System.Windows;

public sealed partial class LicenseView
{
    public LicenseView()
    {
        InitializeComponent();
    }

    public bool ShowAbout
    {
        get { return (bool)GetValue(ShowAboutProperty); }
        set { SetValue(ShowAboutProperty, value); }
    }

    public static readonly DependencyProperty ShowAboutProperty = DependencyProperty.Register(nameof(ShowAbout), typeof(bool), 
        typeof(LicenseView), new PropertyMetadata(true));
}
