// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Moq;
using Moq.Protected;
using PDNDClientAssertionGenerator.Configuration;
using PDNDClientAssertionGenerator.Services;
using System.Net;

namespace PDNDClientAssertionGenerator.Tests
{
    public class OAuth2ServiceTests
    {
        private readonly ClientAssertionConfig _config;
        private readonly OAuth2Service _oauth2Service;
        private readonly Mock<HttpMessageHandler> _handlerMock;

        public OAuth2ServiceTests()
        {
            // ClientAssertionConfig Mock configuration
            _config = new ClientAssertionConfig
            {
                Duration = 60,
                KeyPath = "path/to/key",
                Issuer = "issuer",
                Subject = "subject",
                Audience = "audience",
                PurposeId = "purposeId",
                ClientId = "clientId",
                ServerUrl = "https://mocked-server-url/token"
            };

            // HttpMessageHandler Mock
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient(_handlerMock.Object);

            // OAuth2Service instance
            _oauth2Service = new OAuth2Service(_config);
        }

        [Fact]
        public async Task RequestAccessTokenAsync_ReturnsValidTokenResponse()
        {
            // Arrange: HTTP Mock
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
                    Content = new StringContent("{\"access_token\":\"valid_token\",\"expires_in\":3600}"),
                });

            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _oauth2Service.RequestAccessTokenAsync("valid_client_assertion"));
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
