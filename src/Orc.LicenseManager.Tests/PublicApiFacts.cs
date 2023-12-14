namespace Orc.LicenseManager.Tests;

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NUnit.Framework;
using PublicApiGenerator;
using VerifyNUnit;

[TestFixture]
public class PublicApiFacts
{
    [Test, MethodImpl(MethodImplOptions.NoInlining)]
    public async Task Orc_LicenseManager_Client_HasNoBreakingChanges_Async()
    {
        var assembly = typeof(ApplicationIdService).Assembly;

        await PublicApiApprover.ApprovePublicApiAsync(assembly);
    }

    [Test, MethodImpl(MethodImplOptions.NoInlining)]
    public async Task Orc_LicenseManager_Client_Wpf_HasNoBreakingChanges_Async()
    {
        var assembly = typeof(DialogLicenseVisualizerService).Assembly;

        await PublicApiApprover.ApprovePublicApiAsync(assembly);
    }

    internal static class PublicApiApprover
    {
        public static async Task ApprovePublicApiAsync(Assembly assembly)
        {
            var publicApi = ApiGenerator.GeneratePublicApi(assembly, new ApiGeneratorOptions());
            await Verifier.Verify(publicApi);
        }
    }
}
