using Microsoft.AspNetCore.Mvc;
using PDNDClientAssertionGenerator.Interfaces;
using PDNDClientAssertionGenerator.Models;

namespace PDNDClientAssertionGenerator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientAssertionController : ControllerBase
    {
        private readonly IClientAssertionGenerator _clientAssertionGenerator;
        private readonly ILogger<ClientAssertionController> _logger;

        public ClientAssertionController(ILogger<ClientAssertionController> logger, IClientAssertionGenerator clientAssertionGenerator)
        {
            _logger = logger;
            _clientAssertionGenerator = clientAssertionGenerator;
        }

        [HttpGet("GetClientAssertion", Name = "GetClientAssertion")]
        public async Task<string> GetClientAssertionAsync()
        {
            return await _clientAssertionGenerator.GetClientAssertionAsync();
        }

        [HttpGet("GetToken", Name = "GetToken")]
        public async Task<PDNDTokenResponse> GetToken([FromQuery] string clientAssertion)
        {
            return await _clientAssertionGenerator.GetToken(clientAssertion);
        }
    }
}
