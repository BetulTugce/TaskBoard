namespace TaskBoard.Application.DTOs
{
    public class Token
    {
        public string AccessToken { get; set; } // Kullanicinin kimligini dogrulamak ve yetkilendirmek icin kullanilacak.
        public DateTime Expiration { get; set; } // AccessTokenin gecerlilik suresi.
        public string RefreshToken { get; set; } // AccessToken gecerliligini kaybettiginde kontrol edilecek, yeni bir token almak icin kullanilacak.
    }
}
