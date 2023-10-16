namespace review.Common.Models
{
    public class JwtTokensOptionsModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireAfterMinutes { get; set; }
        public string SigningKey { get; set; }
    }
}
