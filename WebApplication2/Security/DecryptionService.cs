using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication2.Security
{
    public class DecryptionService
    {
        private readonly string _password;
        private readonly byte[] _salt;

        public DecryptionService(string password, byte[] salt)
        {
            _password = password;
            _salt = salt;
        }

        public string Decrypt(string cipherText)
        {
            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(_password, _salt, 10000);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = new byte[aes.BlockSize / 8];
                aes.Padding = PaddingMode.None; // Explicitly set padding mode

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
