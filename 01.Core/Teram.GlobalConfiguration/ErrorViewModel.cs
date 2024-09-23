namespace Teram.GlobalConfiguration
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string PageId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}
