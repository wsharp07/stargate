using System;

namespace Stargate.API.Data.Entities
{
    public class File
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string ShortUri { get; set; }
        public string ExternalUri { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeBytes { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}