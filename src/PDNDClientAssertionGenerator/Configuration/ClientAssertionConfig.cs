// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
namespace PDNDClientAssertionGenerator.Configuration
{
    public class ClientAssertionConfig
    {
        /// <summary>
        /// Gets or sets the authentication server URL
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// Gets or sets the public key ID (kid)
        /// </summary>
        public string KeyId { get; set; }

        /// <summary>
        /// Gets or sets the signing algorithm (alg)
        /// </summary>
        /// <remarks>Actually only RS256 is available</remarks>
        /// <example>"RS256"</example>
        public string Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the type of object
        /// </summary>
        /// <example>"JWT"</example>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Client identifier
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the issuer (iss)
        /// </summary>
        /// <remarks>Should be set as the client ID</remarks>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the subject (sub)
        /// </summary>
        /// <remarks>Should be set as the client ID</remarks>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the audience (aud)
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the purpose for which access to resources will be requested (purposeId)
        /// </summary>
        public string PurposeId { get; set; }

        /// <summary>
        /// Gets or sets the path to the private key to sign the client assertion
        /// </summary>
        public string KeyPath { get; set; }

        /// <summary>
        /// Gets or sets the duration in minutes of the token (this will be used to calculate the token expiration)
        /// </summary>
        public int Duration { get; set; }
    }
}
