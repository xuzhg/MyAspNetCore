using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreConsole.Models
{
    public partial class Machine
    {
        public Machine()
        {
            SupportTicket = new HashSet<SupportTicket>();
        }

        public int MachineId { get; set; }
        public string Name { get; set; }
        public string GeneralRole { get; set; }
        public string InstalledRoles { get; set; }
        public int OperatingSysId { get; set; }
        public int MachineTypeId { get; set; }
        public MachineType MachineType { get; set; }
        public OperatingSys OperatingSys { get; set; }
        public ICollection<SupportTicket> SupportTicket { get; set; }
    }
}
