using System.Collections.Generic;
using Newtonsoft.Json;

namespace PowerBiBuddy.Client.Models
{
    public class Table
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("columns")]
        public IEnumerable<Column> Columns { get; set; }
    }
}