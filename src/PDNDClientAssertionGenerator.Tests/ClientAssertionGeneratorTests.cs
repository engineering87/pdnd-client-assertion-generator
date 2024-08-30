using Moq;
using Moq.Protected;
using PDNDClientAssertionGenerator.Interfaces;
using PDNDClientAssertionGenerator.Models;
using PDNDClientAssertionGenerator.Services;

namespace PDNDClientAssertionGenerator.Tests
{
    public class ClientAssertionGeneratorServiceTests
    {
        [Fact]
        public async Task GetClientAssertionAsync_ShouldCall_GenerateClientAssertionAsync()
        {
            // Arrange
            var oauth2ServiceMock = new Mock<IOAuth2Service>();
            var clientAssertionGeneratorService = new ClientAssertionGeneratorService(oauth2ServiceMock.Object);

            // Act
            await clientAssertionGeneratorService.GetClientAssertionAsync();

            // Assert
            oauth2ServiceMock.Verify(o => o.GenerateClientAssertionAsync(), Times.Once);
        }

        [Fact]
        public async Task GetToken_ShouldCall_RequestAccessTokenAsync_WithClientAssertion()
        {
            // Arrange
            var oauth2ServiceMock = new Mock<IOAuth2Service>();
            var clientAssertionGeneratorService = new ClientAssertionGeneratorService(oauth2ServiceMock.Object);
            var clientAssertion = "testClientAssertion";

            // Act
            await clientAssertionGeneratorService.GetToken(clientAssertion);

            // Assert
            oauth2ServiceMock.Verify(o => o.RequestAccessTokenAsync(clientAssertion), Times.Once);
        }
    }
}