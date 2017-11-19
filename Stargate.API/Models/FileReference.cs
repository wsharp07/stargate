namespace Stargate.API.Models
{
    public class FileReference
    {
        public string FileName { get; set; }
        public string Uri { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}