using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Unity;
using ShowManager.Client.WPF.Infrastructure;
using ShowManager.Client.WPF.Messages;
using ShowManager.Client.WPF.ViewModels;

namespace ShowManager.Client.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : MetroWindow
    {
        public Shell()
        {
            InitializeComponent();

            this.ViewModel = App.UnityContainer.Resolve<ShellViewModel>();
            this.DataContext = this.ViewModel;

            this.Initialize();
        }

        private void Initialize()
        {
            this.InitializeEvents();
        }
        private async void InitializeEvents()
        {
            
        }

        #region DisplayStatusMessage
        private async void DisplayMessage(DisplayMessage displayMessage)
        {
            //if (args != null)
            //{
            //    var settings = new MetroDialogSettings
            //    {
            //        AffirmativeButtonText = args.AffirmativeButtonText,
            //        NegativeButtonText = args.NegativeButtonText
            //    };

            //    MessageDialogStyle style = MessageDialogStyle.Affirmative;
            //    if (!string.IsNullOrWhiteSpace(settings.NegativeButtonText))
            //    {
            //        style = MessageDialogStyle.AffirmativeAndNegative;
            //    }

            //    MessageDialogResult result = await this.ShowMessageAsync(args.Title, args.Message, style, settings);
            //}
            
        }
        #endregion

        #region DisplayInputMessage
        //private async Task<string> OnDisplayPromptMessage(PromptMessage promptMessage)
        //{
            

        //    if (promptMessage != null)
        //    {
        //        var settings = new MetroDialogSettings
        //        {
        //            AffirmativeButtonText = "Ok",
        //            NegativeButtonText = "Cancel"
        //        };

        //        MessageDialogStyle style = MessageDialogStyle.AffirmativeAndNegative;
        //}
        #endregion


        #region ViewModel
        ShellViewModel ViewModel { get; set; } 
        #endregion
    }

    static class MetroDialogSettingsFactory
    {
        public static MetroDialogSettings Create()
        {
            return null;
        }
    }
}
