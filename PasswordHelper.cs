using System;
using System.Security.Cryptography;

namespace myfirst_sql_practice
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        // Ibinabalik ang "salt:hash" na ise-save sa database
        public static string Hash(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);
                return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            }
        }

        // Tinitingnan kung tugma ang password sa na-save na "salt:hash"
        public static bool Verify(string password, string stored)
        {
            string[] parts = stored.Split(':');
            if (parts.Length != 2) return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expected = Convert.FromBase64String(parts[1]);

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] actual = pbkdf2.GetBytes(expected.Length);

                int diff = 0;
                for (int i = 0; i < expected.Length; i++)
                    diff |= actual[i] ^ expected[i];

                return diff == 0;
            }
        }
    }
}