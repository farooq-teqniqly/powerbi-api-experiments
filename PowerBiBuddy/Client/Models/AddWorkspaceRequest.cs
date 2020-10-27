using Newtonsoft.Json;

namespace PowerBiBuddy.Client.Models
{
    public class AddWorkspaceRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}