namespace Stargate.API.ViewModels
{
    public class UploadResponseViewModel
    {
        public int Id { get; set; }
        public bool Success { get; set; }
        public string FileName { get; set; }
        public string Uri { get; set; }
    }
}