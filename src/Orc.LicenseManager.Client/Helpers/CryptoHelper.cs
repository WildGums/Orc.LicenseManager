// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptoHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// This code originally comes from http://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-s
    /// </summary>
    internal static class CryptoHelper
    {
        private const int Keysize = 128;
        private const int DerivationIterations = 1024;

        private static readonly Encoding Encoding = Encoding.UTF8;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting
            var saltStringBytes = GenerateRandomBytes(Keysize);
            var ivStringBytes = GenerateRandomBytes(Keysize);
            var plainTextBytes = Encoding.GetBytes(plainText);

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);

                using (var symmetricKey = Aes.Create())
                {
                    symmetricKey.BlockSize = Keysize;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;

                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();

                                memoryStream.Position = 0L;

                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();

                                var base64 = Convert.ToBase64String(cipherTextBytes);
                                return base64;
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            var fixedDataLength = Keysize / 8;

            // Get the complete stream of bytes that represent:
            // [16 bytes of Salt] + [16 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);

            // Get the saltbytes by extracting the first 16 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(fixedDataLength).ToArray();

            // Get the IV bytes by extracting the next 16 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(fixedDataLength).Take(fixedDataLength).ToArray();

            // Get the actual cipher text bytes by removing the first 32 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip(fixedDataLength * 2).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(fixedDataLength);

                using (var symmetricKey = Aes.Create())
                {
                    symmetricKey.BlockSize = Keysize;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;

                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                                var decryptedText = Encoding.GetString(plainTextBytes, 0, decryptedByteCount);
                                return decryptedText;
                            }
                        }
                    }
                }
            }
        }

        private static byte[] GenerateRandomBytes(int keySizeInBits)
        {
            var length = keySizeInBits / 8;
            var randomBytes = new byte[length];

            using (var rngCsp = RandomNumberGenerator.Create())
            {
                rngCsp.GetBytes(randomBytes);
            }

            return randomBytes;
        }
    }
}
