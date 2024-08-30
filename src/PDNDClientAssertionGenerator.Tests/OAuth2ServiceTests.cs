using Microsoft.IdentityModel.Tokens;
using Moq;
using PDNDClientAssertionGenerator.Configuration;
using PDNDClientAssertionGenerator.Models;
using PDNDClientAssertionGenerator.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PDNDClientAssertionGenerator.Tests
{
    public class OAuth2ServiceTests
    {
        private readonly ClientAssertionConfig _config;
        private readonly OAuth2Service _oauth2Service;

        public OAuth2ServiceTests()
        {
            _config = new ClientAssertionConfig
            {
                Duration = 60,
                KeyPath = "path/to/key",
                Issuer = "issuer",
                Subject = "subject",
                Audience = "audience",
                PurposeId = "purposeId",
                ClientId = "clientId",
                ServerUrl = "https://example.com/token"
            };

            _oauth2Service = new OAuth2Service(_config);
        }

        //[Fact]
        //public async Task GenerateClientAssertionAsync_ReturnsValidClientAssertion()
        //{
        //    // Arrange
        //    var rsaParams = new RSAParameters();
        //    var rsa = new Mock<RSACryptoServiceProvider>();
        //    rsa.Setup(r => r.ImportParameters(It.IsAny<RSAParameters>()));
        //    rsa.Setup(r => r.ExportParameters(false)).Returns(rsaParams);

        //    var rsaSecurityKey = new RsaSecurityKey(rsa.Object);
        //    var signingCredentials = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256);

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var expectedClientAssertion = "valid_client_assertion";

        //    var mockTokenDescriptor = new Mock<SecurityTokenDescriptor>();
        //    mockTokenDescriptor.SetupGet(td => td.SigningCredentials).Returns(signingCredentials);

        //    var mockSecurityToken = new Mock<SecurityToken>();
        //    tokenHandler.Setup(th => th.CreateToken(mockTokenDescriptor.Object)).Returns(mockSecurityToken.Object);
        //    tokenHandler.Setup(th => th.WriteToken(mockSecurityToken.Object)).Returns(expectedClientAssertion);

        //    // Act
        //    var clientAssertion = await _oauth2Service.GenerateClientAssertionAsync();

        //    // Assert
        //    Assert.Equal(expectedClientAssertion, clientAssertion);
        //}

        [Fact]
        public async Task RequestAccessTokenAsync_ReturnsValidTokenResponse()
        {
            // Arrange
            var httpClient = new Mock<HttpClient>();
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponse.Content = new StringContent("{\"access_token\":\"valid_token\",\"expires_in\":3600}");

            httpClient.Setup(hc => hc.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>(), CancellationToken.None))
                .ReturnsAsync(httpResponse);

            // Act
            var tokenResponse = await _oauth2Service.RequestAccessTokenAsync("valid_client_assertion");

            // Assert
            Assert.Equal("valid_token", tokenResponse.AccessToken);
            Assert.Equal(3600, tokenResponse.ExpiresIn);
        }

        [Fact]
        public async Task RequestAccessTokenAsync_ThrowsExceptionOnInvalidJsonResponse()
        {
            // Arrange
            var httpClient = new Mock<HttpClient>();
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponse.Content = new StringContent("invalid_json_response");

            httpClient.Setup(hc => hc.PostAsync(It.IsAny<string>(), It.IsAny<FormUrlEncodedContent>(), CancellationToken.None))
                .ReturnsAsync(httpResponse);

            // Act and Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _oauth2Service.RequestAccessTokenAsync("valid_client_assertion"));
        }
    }
}

