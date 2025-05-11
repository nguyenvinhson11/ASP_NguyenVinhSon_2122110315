namespace NguyenVinhSon_2122110315.Config
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;           // 🟡 bị thiếu dòng này nè!
        public int ExpiryMinutes { get; set; } = 60;
    }
}
