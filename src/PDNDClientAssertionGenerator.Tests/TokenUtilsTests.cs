// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using PDNDClientAssertionGenerator.Utils;

namespace PDNDClientAssertionGenerator.Tests
{
    public class TokenUtilsTests
    {
        [Fact]
        public void ExtractAccessToken_ShouldReturnAccessToken_WhenValidJsonResponseIsProvided()
        {
            // Arrange
            string jsonResponse = "{\"access_token\": \"abc123\", \"expires_in\": 3600}";
            string expectedAccessToken = "abc123";

            // Act
            string actualAccessToken = TokenUtils.ExtractAccessToken(jsonResponse);

            // Assert
            Assert.Equal(expectedAccessToken, actualAccessToken);
        }

        [Fact]
        public void ExtractAccessToken_ShouldReturnEmptyString_WhenAccessTokenIsMissing()
        {
            // Arrange
            string jsonResponse = "{\"expires_in\": 3600}";

            // Act
            string actualAccessToken = TokenUtils.ExtractAccessToken(jsonResponse);

            // Assert
            Assert.Equal(string.Empty, actualAccessToken);
        }

        [Fact]
        public void ExtractAccessToken_ShouldReturnEmptyString_WhenJsonIsInvalid()
        {
            // Arrange
            string invalidJsonResponse = "{\"access_token\": \"abc123\", \"expires_in\": }"; // Invalid JSON

            // Act
            string actualAccessToken = TokenUtils.ExtractAccessToken(invalidJsonResponse);

            // Assert
            Assert.Equal(string.Empty, actualAccessToken);
        }

        [Fact]
        public void ExtractAccessToken_ShouldReturnEmptyString_WhenAccessTokenIsNull()
        {
            // Arrange
            string jsonResponse = "{\"access_token\": null}";

            // Act
            string actualAccessToken = TokenUtils.ExtractAccessToken(jsonResponse);

            // Assert
            Assert.Equal(string.Empty, actualAccessToken);
        }
    }
}
