namespace PowerBiBuddy.Client.Models
{
    public class Dataset : PbiResource
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