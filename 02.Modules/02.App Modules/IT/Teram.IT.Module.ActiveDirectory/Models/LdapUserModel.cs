namespace Teram.IT.Module.ActiveDirectory.Models
{
    public class LdapUserModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool IsAuthorized { get; set; }
    }
}
