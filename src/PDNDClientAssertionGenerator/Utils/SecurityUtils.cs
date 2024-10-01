// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Security.Cryptography;

namespace PDNDClientAssertionGenerator.Utils
{
    public static class SecurityUtils
    {
        /// <summary>
        /// Retrieves the RSAParameters from a PEM file located at the specified key path.
        /// </summary>
        /// <param name="keyPath">The file path to the PEM file containing the RSA private key.</param>
        /// <returns>An RSAParameters object containing the RSA key parameters.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the specified key file is not found.</exception>
        /// <exception cref="FormatException">Thrown when the key format is invalid.</exception>
        public static RSAParameters GetSecurityParameters(string keyPath)
        {
            // Check if the key path is valid
            if (string.IsNullOrWhiteSpace(keyPath))
            {
                throw new ArgumentException("Key path cannot be null or empty.", nameof(keyPath));
            }

            // Read the PEM content from the specified file
            string pemContent;
            try
            {
                pemContent = File.ReadAllText(keyPath).Trim();
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("Unable to read the key file.", ex);
            }

            // Extract the base64 key content
            string base64Key = ExtractBase64Key(pemContent);
            byte[] privateKeyBytes;

            try
            {
                privateKeyBytes = Convert.FromBase64String(base64Key);
            }
            catch (FormatException ex)
            {
                throw new FormatException("The key format is invalid.", ex);
            }

            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
                return rsa.ExportParameters(true);
            }
        }

        /// <summary>
        /// Extracts the base64 encoded key from the PEM formatted string.
        /// </summary>
        /// <param name="pemContent">The PEM formatted string.</param>
        /// <returns>The base64 encoded key.</returns>
        public static string ExtractBase64Key(string pemContent)
        {
            // Remove the header, footer, and any newlines or whitespaces
            return pemContent
                .Replace("-----BEGIN RSA PRIVATE KEY-----", string.Empty)
                .Replace("-----END RSA PRIVATE KEY-----", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Replace(" ", string.Empty)
                .Trim();
        }

        /// <summary>
        /// Retrieves and imports RSA security parameters from the specified key path.
        /// </summary>
        /// <param name="keyPath">The file path to the RSA private key.</param>
        /// <returns>An instance of RSA with the imported key parameters.</returns>
        /// <exception cref="InvalidOperationException">Thrown if there is an error retrieving or importing the RSA parameters.</exception>
        public static RSA GetRsaFromKeyPath(string keyPath)
        {
            try
            {
                // Retrieve RSA security parameters from the specified key path.
                RSAParameters rsaParams = GetSecurityParameters(keyPath);

                // Create a new instance of RSA and import the retrieved key parameters.
                var rsa = RSA.Create();
                rsa.ImportParameters(rsaParams);

                // Return the configured RSA instance.
                return rsa;
            }
            catch (Exception ex)
            {
                // If there is an error during the retrieval or import of the key, throw an InvalidOperationException.
                throw new InvalidOperationException("Failed to retrieve or import RSA security parameters.", ex);
            }
        }
    }
}
