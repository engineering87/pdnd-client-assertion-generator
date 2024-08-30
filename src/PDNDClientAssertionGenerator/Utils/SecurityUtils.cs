using System.Security.Cryptography;

namespace PDNDClientAssertionGenerator.Utils
{
    public static class SecurityUtils
    {
        public static RSAParameters GetSecurityParameters(string keyPath)
        {
            string pemContent = File.ReadAllText(keyPath).Trim();

            string base64Key = pemContent
                .Replace("-----BEGIN RSA PRIVATE KEY-----", string.Empty)
                .Replace("-----END RSA PRIVATE KEY-----", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Trim();

            byte[] privateKeyBytes = Convert.FromBase64String(base64Key);

            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
                return rsa.ExportParameters(true);
            }
        }
    }
}
