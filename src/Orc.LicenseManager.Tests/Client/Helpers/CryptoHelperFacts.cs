namespace Orc.LicenseManager.Tests.Client
{
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    internal class CryptoHelperFacts
    {
        [TestCase("computerId|+|licenseSignature|+|20211116162841|+|computerId|+|licenseSignature|+|20211116162841",
            "1E80821A-96FB-4C93-85A7-289B5CA3228F")]
        public async Task EncryptTestAsync(string inputText, string inputPassword)
        {
            var text = await CryptoHelper.EncryptAsync(inputText, inputPassword);
            var actualDecryptedText = await CryptoHelper.DecryptAsync(text, inputPassword); 

            Assert.AreEqual(inputText, actualDecryptedText);
        }
    }
}
