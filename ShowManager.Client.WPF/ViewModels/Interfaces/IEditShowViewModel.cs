using System;
using System.Threading.Tasks;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.WPF.ViewModels
{
    interface IEditShowViewModel
    {
        void Populate(string header, Show show);
        Task<bool> ClearAsyc();

        //Action<Show> Saved { get; set; }
        //Action<Show> Refreshed { get; set; }
        //Action<Show> Deleted { get; set; }
    }
}
