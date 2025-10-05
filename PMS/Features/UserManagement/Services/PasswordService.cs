using System.Security.Cryptography;
using System.Text;

namespace PMS.Features.UserManagement.Services
{
    public class PasswordService
    {
        private static readonly string key = "DAMSecretKey1234567890Invent1234"; // 32 chars
        private static readonly string iv = "DAMSecretKey1234";  // 16 chars
        private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        private const string Digits = "0123456789";
        private const string SpecialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";


        public string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }


        public string GenerateTempPassword(int length = 5)
        {
            if (length < 6)
                throw new ArgumentException("Password length should be at least 6 characters.");

            string allChars = Uppercase + Lowercase + Digits + SpecialChars;
            var password = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                // Ensure password has at least one uppercase, lowercase, digit, special char
                password[0] = Uppercase[GetRandomIndex(rng, Uppercase.Length)];
                password[1] = Lowercase[GetRandomIndex(rng, Lowercase.Length)];
                password[2] = Digits[GetRandomIndex(rng, Digits.Length)];
                password[3] = SpecialChars[GetRandomIndex(rng, SpecialChars.Length)];

                for (int i = 4; i < length; i++)
                {
                    password[i] = allChars[GetRandomIndex(rng, allChars.Length)];
                }

                // Shuffle the password for randomness
                return new string(password.OrderBy(x => GetRandomIndex(rng, int.MaxValue)).ToArray());
            }
        }

        private static int GetRandomIndex(RandomNumberGenerator rng, int max)
        {
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            int value = BitConverter.ToInt32(bytes, 0) & int.MaxValue;
            return value % max;
        }
    }
}
