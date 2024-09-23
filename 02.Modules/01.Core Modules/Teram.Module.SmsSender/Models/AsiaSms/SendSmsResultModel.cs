namespace Teram.Module.SmsSender.Models.AsiaSms
{
    public class SendSmsResultModel
    {
        public Guid BatchId {  get; set; }
        public bool IsSuccessful {  get; set; }
        public int StatusCode {  get; set; }
        public string Message { get; set; }
    }
}
