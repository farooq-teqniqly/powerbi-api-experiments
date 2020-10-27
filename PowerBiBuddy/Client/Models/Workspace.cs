using System;

namespace PowerBiBuddy.Client.Models
{
    public class Workspace : PbiResource
    {
        public bool IsReadOnly { get; set; }
        public bool IsOnDedicatedCapacity { get; set; }
        public Guid CapacityId { get; set; }
    }
}