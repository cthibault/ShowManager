using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowManager.Client.WPF.Views.Interfaces
{
    interface IView
    {
        object DataContext { get; set; }
    }
}
