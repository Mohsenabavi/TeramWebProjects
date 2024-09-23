namespace Teram.Module.Authentication.Models
{
    public class TokenSettings
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Authority { get; set; }
    }
}
