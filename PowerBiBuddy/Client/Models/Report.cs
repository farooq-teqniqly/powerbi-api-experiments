using System;

namespace PowerBiBuddy.Client.Models
{
    public class Report : PbiResource
    {
        public Guid DatasetId { get; set; }
        public string EmbedUrl { get; set; }
        public string WebUrl { get; set; }
    }
}
