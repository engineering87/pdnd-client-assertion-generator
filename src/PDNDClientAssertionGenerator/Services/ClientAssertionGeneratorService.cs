using PDNDClientAssertionGenerator.Interfaces;
using PDNDClientAssertionGenerator.Models;

namespace PDNDClientAssertionGenerator.Services
{
    public class ClientAssertionGeneratorService : IClientAssertionGenerator
    {
        private readonly IOAuth2Service _oauth2Service;

        public ClientAssertionGeneratorService(IOAuth2Service oauth2Service)
        {
            _oauth2Service = oauth2Service ?? throw new ArgumentNullException(nameof(oauth2Service));
        }

        public async Task<string> GetClientAssertionAsync()
        {
            return await _oauth2Service.GenerateClientAssertionAsync();
        }

        public async Task<PDNDTokenResponse> GetToken(string clientAssertion)
        {
            return await _oauth2Service.RequestAccessTokenAsync(clientAssertion);
        }
    }
}
