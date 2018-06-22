using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreConsole.Models
{
    public partial class WarrantyProvider
    {
        public WarrantyProvider()
        {
            MachineWarranty = new HashSet<MachineWarranty>();
        }

        public int WarrantyProviderId { get; set; }
        public string ProviderName { get; set; }
        public int? SupportExtension { get; set; }
        public string SupportNumber { get; set; }
        public ICollection<MachineWarranty> MachineWarranty { get; set; }
    }
}
