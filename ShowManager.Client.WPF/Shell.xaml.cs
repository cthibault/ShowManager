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
using Microsoft.Practices.Unity;
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
        }

        ShellViewModel ViewModel { get; set; }
    }
}
