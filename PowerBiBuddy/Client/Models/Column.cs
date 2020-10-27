using Newtonsoft.Json;

namespace PowerBiBuddy.Client.Models
{
    public class Column
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dataType")]
        public string DataType { get; set; }
    }
}