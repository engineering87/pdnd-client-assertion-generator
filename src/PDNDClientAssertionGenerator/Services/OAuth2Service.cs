using Microsoft.IdentityModel.Tokens;
using PDNDClientAssertionGenerator.Configuration;
using PDNDClientAssertionGenerator.Interfaces;
using PDNDClientAssertionGenerator.Models;
using PDNDClientAssertionGenerator.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

namespace PDNDClientAssertionGenerator.Services
{
    public class OAuth2Service : IOAuth2Service
    {
        private readonly ClientAssertionConfig _config;

        public OAuth2Service(ClientAssertionConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public Task<string> GenerateClientAssertionAsync()
        {
            // Creazione e inizializzazione delle variabili temporali
            DateTime issuedAt = DateTime.UtcNow;
            DateTime expiresAt = issuedAt.AddMinutes(_config.Duration);
            Guid tokenId = Guid.NewGuid();

            // Creazione delle credenziali di firma usando RSA
            RSAParameters rsaParams = SecurityUtils.GetSecurityParameters(_config.KeyPath);
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParams);

            var rsaSecurityKey = new RsaSecurityKey(rsa);
            var signingCredentials = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }  // Per evitare il caching di provider di firma
            };

            // Creazione dei claims del token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss, _config.Issuer),
                new Claim(JwtRegisteredClaimNames.Sub, _config.Subject),
                new Claim(JwtRegisteredClaimNames.Aud, _config.Audience),
                new Claim("purposeId", _config.PurposeId),
                new Claim(JwtRegisteredClaimNames.Jti, tokenId.ToString("D").ToLower()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(issuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            // Creazione del token JWT
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string clientAssertion = tokenHandler.WriteToken(securityToken);

            return Task.FromResult(clientAssertion);
        }

        public async Task<PDNDTokenResponse> RequestAccessTokenAsync(string clientAssertion)
        {
            using var httpClient = new HttpClient();

            // Creazione del payload come contenuto URL-encoded
            var payload = new Dictionary<string, string>
            {
                { "client_id", _config.ClientId },
                { "client_assertion", clientAssertion },
                { "client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer" },
                { "grant_type", "client_credentials" }
            };

            // Impostazione dell'intestazione Accept per il tipo di contenuto JSON
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Creazione del contenuto del POST
            var content = new FormUrlEncodedContent(payload);

            // Invio della richiesta POST e attesa della risposta
            HttpResponseMessage response = await httpClient.PostAsync(_config.ServerUrl, content);

            // Verifica del successo della risposta
            response.EnsureSuccessStatusCode();

            // Lettura e deserializzazione del contenuto della risposta
            string jsonResponse = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<PDNDTokenResponse>(jsonResponse);
            }
            catch (JsonException ex)
            {
                // Gestione delle eccezioni di deserializzazione
                throw new InvalidOperationException("Failed to deserialize the token response.", ex);
            }
        }
    }
}
