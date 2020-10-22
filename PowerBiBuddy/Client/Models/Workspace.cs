using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiBuddy.Client.Models
{
    public class Workspace
    {
        public Guid Id { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsOnDedicatedCapacity { get; set; }
        public Guid CapacityId { get; set; }
        public string Name { get; set; }

    }
}
