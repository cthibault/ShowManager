using System;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.WPF.ViewModels
{
    interface IEditShowViewModel
    {
        bool TryOpen(string header, Show show);
        bool TryClose();

        Action<Show> Saved { get; set; }
        Action<Show> Refreshed { get; set; }
        Action<Show> Deleted { get; set; }
    }
}
