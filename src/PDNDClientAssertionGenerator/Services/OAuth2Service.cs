// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PDNDClientAssertionGenerator.Configuration;
using PDNDClientAssertionGenerator.Interfaces;
using PDNDClientAssertionGenerator.Models;
using PDNDClientAssertionGenerator.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace PDNDClientAssertionGenerator.Services
{
    public class OAuth2Service : IOAuth2Service
    {
        private readonly ClientAssertionConfig _config;

        // Constructor for OAuth2Service, takes a configuration object.
        public OAuth2Service(IOptions<ClientAssertionConfig> config)
        {
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));
        }

        // Asynchronously generates a client assertion JWT token.
        public async Task<string> GenerateClientAssertionAsync()
        {
            // Generate a unique token ID (JWT ID)
            Guid tokenId = Guid.NewGuid();

            // Define the current UTC time and the token expiration time.
            DateTime issuedAt = DateTime.UtcNow;
            DateTime expiresAt = issuedAt.AddMinutes(_config.Duration);

            // Define JWT header as a dictionary of key-value pairs.
            Dictionary<string, string> headers = new()
            {
                { "kid", _config.KeyId },    // Key ID used to identify the signing key
                { "alg", _config.Algorithm }, // Algorithm used for signing (e.g., RS256)
                { "typ", _config.Type }       // Type of the token, usually "JWT"
            };

            // Define the payload as a list of claims, which represent the content of the JWT.
            var payloadClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss, _config.Issuer),   // Issuer of the token
                new Claim(JwtRegisteredClaimNames.Sub, _config.Subject),  // Subject of the token
                new Claim(JwtRegisteredClaimNames.Aud, _config.Audience), // Audience for which the token is intended
                new Claim("purposeId", _config.PurposeId),                // Custom claim for the purpose of the token
                new Claim(JwtRegisteredClaimNames.Jti, tokenId.ToString("D").ToLower()), // JWT ID
                new Claim(JwtRegisteredClaimNames.Iat, issuedAt.ToUnixTimestamp().ToString(), ClaimValueTypes.Integer64), // Issued At time (as Unix timestamp)
                new Claim(JwtRegisteredClaimNames.Exp, expiresAt.ToUnixTimestamp().ToString(), ClaimValueTypes.Integer64)  // Expiration time (as Unix timestamp)
            };

            // Create signing credentials using RSA for signing the token.
            using var rsa = SecurityUtils.GetRsaFromKeyPath(_config.KeyPath);
            var rsaSecurityKey = new RsaSecurityKey(rsa);
            var signingCredentials = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            // Create the JWT token with the specified header and payload claims.
            var token = new JwtSecurityToken(
                new JwtHeader(signingCredentials, headers),
                new JwtPayload(payloadClaims)
            );

            // Use JwtSecurityTokenHandler to convert the token into a string.
            var tokenHandler = new JwtSecurityTokenHandler();
            string clientAssertion = string.Empty;

            try
            {
                clientAssertion = tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to generate JWT token.", ex);
            }

            return await Task.FromResult(clientAssertion); // Return the generated token as a string.
        }

        // Asynchronously requests an access token by sending the client assertion to the OAuth2 server.
        public async Task<PDNDTokenResponse> RequestAccessTokenAsync(string clientAssertion)
        {
            using var httpClient = new HttpClient();

            // Create the payload for the POST request in URL-encoded format.
            var payload = new Dictionary<string, string>
            {
                { "client_id", _config.ClientId }, // Client ID as per OAuth2 spec
                { "client_assertion", clientAssertion }, // Client assertion (JWT) generated in the previous step
                { "client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer" }, // Assertion type
                { "grant_type", "client_credentials" } // Grant type for client credentials
            };

            // Set the Accept header to request JSON responses from the server.
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Create the content for the POST request (FormUrlEncodedContent).
            var content = new FormUrlEncodedContent(payload);

            // Send the POST request to the OAuth2 server and await the response.
            HttpResponseMessage response = await httpClient.PostAsync(_config.ServerUrl, content);

            // Ensure the response indicates success (throws an exception if not).
            response.EnsureSuccessStatusCode();

            // Read and parse the response body as a JSON string.
            string jsonResponse = await response.Content.ReadAsStringAsync();

            try
            {
                // Deserialize the JSON response into the PDNDTokenResponse object.
                return JsonSerializer.Deserialize<PDNDTokenResponse>(jsonResponse);
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization errors.
                throw new InvalidOperationException("Failed to deserialize the token response.", ex);
            }
        }
    }
}
