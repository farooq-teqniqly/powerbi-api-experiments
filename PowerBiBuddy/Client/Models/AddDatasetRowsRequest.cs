using System.Collections.Generic;
using Newtonsoft.Json;

namespace PowerBiBuddy.Client.Models
{
    public class AddDatasetRowsRequest<TRow>
    {
        [JsonProperty("rows")]
        public IEnumerable<TRow> Rows { get; set; }
    }
}