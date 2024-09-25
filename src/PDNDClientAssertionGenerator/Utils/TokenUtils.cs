// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Text.Json;

namespace PDNDClientAssertionGenerator.Utils
{
    public static class TokenUtils
    {
        public static string ExtractAccessToken(string response)
        {
            try
            {
                using (JsonDocument document = JsonDocument.Parse(response))
                {
                    if (document.RootElement.TryGetProperty("access_token", out JsonElement accessTokenElement))
                    {
                        return accessTokenElement.GetString() ?? string.Empty;
                    }
                }

                return string.Empty;
            }
            catch (JsonException)
            {
                return string.Empty;
            }            
        }
    }
}
