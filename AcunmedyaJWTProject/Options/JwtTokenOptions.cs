namespace AcunmedyaJWTProject.Options
{
    public class JwtTokenOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; } 
        public int ExpireMinutes { get; set; }
    }
}
