namespace Orc.LicenseManager.Views;

using Catel;
using Catel.Windows;
using ViewModels;

public partial class LicenseWindow
{
    partial void OnInitializedComponent()
    {
        if (CatelEnvironment.IsInDesignMode)
        {
            return;
        }

        LicenseManager.ResourceHelper.EnsureStyles();

        this.ApplyIconFromApplication();

        this.RemoveCloseButton();
    }

    partial void OnInitializingComponent()
    {
        Mode = DataWindowMode.OkCancel;
    }
}
