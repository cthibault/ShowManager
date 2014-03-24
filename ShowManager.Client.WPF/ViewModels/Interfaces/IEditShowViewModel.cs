using System;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.WPF.ViewModels
{
    interface IEditShowViewModel
    {
        void Populate(string header, Show show);
        void Reset();

        //Action<Show> Saved { get; set; }
        //Action<Show> Refreshed { get; set; }
        //Action<Show> Deleted { get; set; }
    }
}
