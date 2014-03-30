using System;
using System.Threading.Tasks;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.WPF.ViewModels
{
    interface IEditShowViewModel
    {
        void Populate(string header, Show show);
        Task<bool> TryClearAsyc();
        Task<bool> TryClearAndCloseAsync();
        Task CloseAndDiscardChangesAsync();


        Action<Show> Save { get; set; }
        Action<Show> Refresh { get; set; }
        Action<Show> Cancel { get; set; }
        Action<Show> Delete { get; set; }
    }
}
