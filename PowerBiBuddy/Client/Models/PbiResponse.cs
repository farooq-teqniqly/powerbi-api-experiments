using System.Collections.Generic;
using Newtonsoft.Json;

namespace PowerBiBuddy.Client.Models
{
    public class PbiResponse<T>
    {
        [JsonProperty("@odata.context")]
        public string Context { get; set; }

        [JsonProperty("@odata.count")]
        public int Count { get; set; }

        [JsonProperty("value")]
        public IEnumerable<T> Values { get; set; }
    }
}
