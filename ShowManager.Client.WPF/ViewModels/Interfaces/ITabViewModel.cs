using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowManager.Client.WPF.Views.Interfaces;

namespace ShowManager.Client.WPF.ViewModels.Interfaces
{
    interface ITabViewModel
    {
        bool IsActive { get; set; }
        string Title { get; }
        IView View { get; }
    }
}
