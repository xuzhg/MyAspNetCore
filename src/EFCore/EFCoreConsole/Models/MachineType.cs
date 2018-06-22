using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreConsole.Models
{
    public partial class MachineType
    {
        public MachineType()
        {
            Machine = new HashSet<Machine>();
        }

        public int MachineTypeId { get; set; }
        public string Description { get; set; }
        public ICollection<Machine> Machine { get; set; }
    }
}
