// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using PDNDClientAssertionGenerator.Configuration;
using PDNDClientAssertionGenerator.Services;
using System.Net;

namespace PDNDClientAssertionGenerator.Tests
{
    public class OAuth2ServiceTests
    {
        private readonly OAuth2Service _oauth2Service;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly Mock<IOptions<ClientAssertionConfig>> _mockOptions;

        public OAuth2ServiceTests()
        {
            // Set up the HttpMessageHandler mock
            _handlerMock = new Mock<HttpMessageHandler>();

            // Set up the ClientAssertionConfig object
            var clientAssertionConfig = new ClientAssertionConfig
            {
                ServerUrl = "https://test-server-url.com",
                ClientId = "test-client-id",
                KeyId = "test-key-id",
                Algorithm = "RS256",
                Type = "JWT",
                Issuer = "test-issuer",
                Subject = "test-subject",
                Audience = "test-audience",
                PurposeId = "test-purpose-id",
                KeyPath = "path/to/key",
                Duration = 60 // token duration in minutes
            };

            // Create the mock IOptions<ClientAssertionConfig> instance
            _mockOptions = new Mock<IOptions<ClientAssertionConfig>>();
            _mockOptions.Setup(o => o.Value).Returns(clientAssertionConfig);

            // Initialize OAuth2Service with the mocked IOptions<ClientAssertionConfig>
            _oauth2Service = new OAuth2Service(_mockOptions.Object);
        }

        [Fact]
        public async Task RequestAccessTokenAsync_ThrowsExceptionOnInvalidJsonResponse()
        {
            // Arrange: Invalid JSON Mock
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("invalid_json_response"),
                });

            // Act & Assert: Verify that an exception is thrown on an invalid response
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _oauth2Service.RequestAccessTokenAsync("valid_client_assertion"));
        }
    }
}
