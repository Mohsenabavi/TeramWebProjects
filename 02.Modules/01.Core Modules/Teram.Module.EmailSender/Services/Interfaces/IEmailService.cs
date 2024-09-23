namespace Teram.Module.EmailSender.Services.Interfaces
{
    public interface IEmailService
    {
        public bool SendEmail(string title, string message, DateTime date, string address);
    }
}
