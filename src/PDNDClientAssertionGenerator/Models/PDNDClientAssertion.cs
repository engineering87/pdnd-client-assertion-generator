// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
namespace PDNDClientAssertionGenerator.Models
{
    public class PDNDClientAssertion
    {
        /// <summary>
        /// Gets or sets the public key ID (kid)
        /// </summary>
        public required string KeyId { get; init; }

        /// <summary>
        /// Gets or sets the signing algorithm (alg)
        /// </summary>
        /// <example>"RS256"</example>
        public required string Algorithm { get; init; }

        /// <summary>
        /// Gets or sets the type of object
        /// </summary>
        /// <example>"JWT"</example>
        public required string Type { get; init; }

        /// <summary>
        /// Gets or sets the issuer (iss)
        /// </summary>
        public required string Issuer { get; init; }

        /// <summary>
        /// Gets or sets the subject (sub)
        /// </summary>
        public required string Subject { get; init; }

        /// <summary>
        /// Gets or sets the audience (aud)
        /// </summary>
        public required string Audience { get; init; }

        /// <summary>
        /// Gets or sets the purpose (purposeId)
        /// </summary>
        public required string PurposeId { get; init; }

        /// <summary>
        /// Gets or sets the JWT identifier
        /// </summary>
        public required Guid TokenId { get; init; }

        /// <summary>
        /// Gets or sets the token creation date
        /// </summary>
        public required DateTime IssuedAt { get; init; }

        /// <summary>
        /// Gets or sets the token expiration date
        /// </summary>
        public required DateTime Expiration { get; init; }

        /// <summary>
        /// Gets or sets the client assertion
        /// </summary>
        public required string ClientAssertion { get; init; }
    }
}