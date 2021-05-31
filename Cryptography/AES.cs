using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DB_s2_1_1.Cryptography
{
    public static class AES
    {
        private const string defaultKeyString = @"Ys!Kq@EmB%j7Z2X";

        public static string EncryptText(string input)
        {
            return EncryptText(input, defaultKeyString);
        }

        public static string EncryptText(string input, string keyString)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(keyString);

            // Hash the password with SHA256
            using SHA256 sha = SHA256.Create();
            passwordBytes = sha.ComputeHash(passwordBytes);
            byte[] bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            if (bytesToBeEncrypted != null)
            {
                using Aes aes = Aes.Create();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.ECB;
                aes.Key = passwordBytes;

                using ICryptoTransform encrypto = aes.CreateEncryptor();
                encryptedBytes = encrypto.TransformFinalBlock(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
            }

            return encryptedBytes;
        }

        public static string DecryptText(string input)
        {
            return DecryptText(input, "");
        }

        public static string DecryptText(string input, string keyString)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            if (string.IsNullOrEmpty(keyString))
                keyString = defaultKeyString;

            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(keyString);
            using SHA256 sha = SHA256.Create();
            passwordBytes = sha.ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AESDecrypt(bytesToBeDecrypted, passwordBytes);
            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private static byte[] AESDecrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            if (bytesToBeDecrypted != null)
            {
                using Aes aes = Aes.Create();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.ECB;
                aes.Key = passwordBytes;

                using ICryptoTransform decrypto = aes.CreateDecryptor();
                decryptedBytes = decrypto.TransformFinalBlock(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
            }

            return decryptedBytes;
        }
    }
}
