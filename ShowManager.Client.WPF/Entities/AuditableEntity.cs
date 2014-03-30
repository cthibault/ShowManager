using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowManager.Client.WPF.ShowManagement
{
    public interface IAuditableEntity
    {
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        DateTime CreatedDtm { get; set; }
        DateTime ModifiedDtm { get; set; }
    }
}
