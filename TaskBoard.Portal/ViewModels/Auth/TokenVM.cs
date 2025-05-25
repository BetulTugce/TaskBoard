namespace TaskBoard.Portal.ViewModels.Auth
{
    public class TokenVM
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
    }
}
