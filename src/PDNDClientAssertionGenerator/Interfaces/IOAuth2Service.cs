using PDNDClientAssertionGenerator.Models;

namespace PDNDClientAssertionGenerator.Interfaces
{
    public interface IOAuth2Service
    {
        Task<string> GenerateClientAssertionAsync();
        Task<PDNDTokenResponse> RequestAccessTokenAsync(string clientAssertion);
    }
}
