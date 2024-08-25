using System.Security.Cryptography;

namespace AuthClass.Utils
{
    public static class TokenGenerator
    {
        public static string GenerateToken(int length = 32)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var tokenData = new byte[length];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData)
                    .Replace("+", "")  // Remove characters that might cause issues in URLs
                    .Replace("/", "")
                    .Replace("=", "");
            }
        }
    }
}
