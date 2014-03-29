using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShowManager.Client.WPF.Models
{
    interface IPromptModel
    {
        string Header { get; }
        bool IsOpen { get; set; }

        ICommand ConfirmCommand { get; }
        ICommand RejectCommand { get; }
    }
}
