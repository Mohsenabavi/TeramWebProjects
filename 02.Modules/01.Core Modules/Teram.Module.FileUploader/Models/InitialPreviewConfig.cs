namespace Teram.HR.Module.FileUploader.Models
{
    public class InitialPreviewConfig
    {
        public int key { get; set; }

        public int fileId { get; set; }

        public string type { get; set; }

        public string caption { get; set; }

        public string width { get; set; }

        public string url { get; set; }
        public int DocumentTypeId { get; set; }

        public string downloadUrl { get; set; }


        public string frameAttr { get; set; }

        public Extraobject extra { get; set; }
        public string Name { get; set; }
        public bool isApproved {  get; set; }

        public InitialPreviewConfig()
        {
            extra = new Extraobject();
        }
    }

    public class Extraobject
    {
        public int id { get; set; }
    }
}
