using System.Security.Cryptography;

namespace OpenAPI.Identity
{
    public static class KeyGenerator
    {
        public static string GenerateApiKey(int length = 32)
        {
            return GenerateRandomKey(length);
        }

        public static string GenerateApiSecret(int length = 64)
        {
            return GenerateRandomKey(length);
        }

        private static string GenerateRandomKey(int length)
        {
            var randomNumber = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
