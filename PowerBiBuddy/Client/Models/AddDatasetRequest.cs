using System.Collections.Generic;
using Newtonsoft.Json;

namespace PowerBiBuddy.Client.Models
{
    public class AddDatasetRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("defaultMode")]
        public string DefaultMode => "Push";

        [JsonProperty("tables")]
        public IEnumerable<Table> Tables { get; set; }
    }
}