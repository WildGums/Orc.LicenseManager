namespace Orc.LicenseManager.Tests.Client
{
    using NUnit.Framework;

    [TestFixture]
    internal class CryptoHelperFacts
    {
        [TestCase("computerId|+|licenseSignature|+|20211116162841|+|computerId|+|licenseSignature|+|20211116162841",
            "1E80821A-96FB-4C93-85A7-289B5CA3228F")]
        public static void EncryptTest(string inputText, string inputPassword)
        {
            var text = CryptoHelper.Encrypt(inputText, inputPassword);
            var actualDecryptedText = CryptoHelper.Decrypt(text, inputPassword); 

            Assert.AreEqual(inputText, actualDecryptedText);
        }
    }
}
