using Microsoft.AspNetCore.Mvc;

namespace PDNDClientAssertionGenerator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientAssertionController : ControllerBase
    {
        private readonly ILogger<ClientAssertionController> _logger;

        public ClientAssertionController(ILogger<ClientAssertionController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetClientAssertion", Name = "GetClientAssertion")]
        public string GetClientAssertion()
        {
            return string.Empty;
        }

        [HttpGet("GetToken", Name = "GetToken")]
        public string GetToken([FromQuery] string clientAssertion)
        {
            return string.Empty;
        }
    }
}
