using System.Text.Json.Serialization;

namespace TaskBoard.Application.DTOs.User
{
    public class FacebookAccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}
