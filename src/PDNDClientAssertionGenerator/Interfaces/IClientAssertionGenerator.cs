using PDNDClientAssertionGenerator.Models;

namespace PDNDClientAssertionGenerator.Interfaces
{
    public interface IClientAssertionGenerator
    {
        Task<string> GetClientAssertionAsync();
        Task<PDNDTokenResponse> GetToken(string clientAssertion);
    }
}
