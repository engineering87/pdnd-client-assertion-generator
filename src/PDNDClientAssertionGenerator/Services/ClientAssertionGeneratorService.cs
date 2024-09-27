// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using PDNDClientAssertionGenerator.Interfaces;
using PDNDClientAssertionGenerator.Models;

namespace PDNDClientAssertionGenerator.Services
{
    // This service handles the generation of client assertions and the retrieval of access tokens.
    public class ClientAssertionGeneratorService : IClientAssertionGenerator
    {
        // Dependency on the OAuth2 service for generating client assertions and requesting tokens.
        private readonly IOAuth2Service _oauth2Service;

        // Constructor that injects the IOAuth2Service dependency.
        // Throws an ArgumentNullException if the provided OAuth2 service is null.
        public ClientAssertionGeneratorService(IOAuth2Service oauth2Service)
        {
            _oauth2Service = oauth2Service ?? throw new ArgumentNullException(nameof(oauth2Service));
        }

        // Asynchronously generates a client assertion (JWT) by delegating to the OAuth2 service.
        public async Task<string> GetClientAssertionAsync()
        {
            return await _oauth2Service.GenerateClientAssertionAsync();
        }

        // Asynchronously requests an OAuth2 access token using the provided client assertion.
        // Delegates the actual token request to the OAuth2 service.
        public async Task<PDNDTokenResponse> GetTokenAsync(string clientAssertion)
        {
            return await _oauth2Service.RequestAccessTokenAsync(clientAssertion);
        }
    }
}