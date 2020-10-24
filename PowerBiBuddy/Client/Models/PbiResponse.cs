using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PowerBiBuddy.Client.Models
{
    public class PbiResponse<T>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("@odata.context")]
        public string Context { get; set; }

        [JsonProperty("@odata.count")]
        public int Count { get; set; }

        [JsonProperty("value")]
        public IEnumerable<T> Values { get; set; }
    }

    public class Workspace : PbiResponse<Workspace>
    {
        public bool IsReadOnly { get; set; }
        public bool IsOnDedicatedCapacity { get; set; }
        public Guid CapacityId { get; set; }
    }

    public class Dataset : PbiResponse<Dataset>
    {
        public bool AddRowsApiEnabled { get; set; }

        public string ConfiguredBy { get; set; }

        public bool IsRefreshable { get; set; }
        public bool IsEffectiveIdentityRequired { get; set; }
        public bool IsEffectiveIdentityRolesRequired { get; set; }
        public bool IsOnPremGatewayRequired { get; set; }

        public string TargetStorageMode { get; set; }

        public string CreateReportEmbedUrl { get; set; }

        public string QnaEmbedUrl { get; set; }
    }

}
