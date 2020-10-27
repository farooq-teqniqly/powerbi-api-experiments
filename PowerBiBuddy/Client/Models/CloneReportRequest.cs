using System;
using Newtonsoft.Json;

namespace PowerBiBuddy.Client.Models
{
    public class CloneReportRequest
    {
        [JsonProperty("name")]
        public string TargetReportName { get; set; }

        [JsonProperty("targetModelId")]
        public Guid TargetDataSetId { get; set; }

        [JsonProperty("targetWorkspaceId")]
        public Guid TargetWorkspaceId { get; set; }
    }
}
