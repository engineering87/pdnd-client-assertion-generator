// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Text.Json.Serialization;

namespace PDNDClientAssertionGenerator.Models
{
    public class PDNDTokenResponse
    {
        [JsonPropertyName("token_type")]
        public required string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }
    }
}
