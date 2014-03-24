using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using ShowManager.Client.WPF.Infrastructure;
using ShowManager.Client.WPF.ViewModels;
using ShowManager.Client.WPF.Views;

namespace ShowManager.Client.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var unityContainer = new UnityContainer();

            //unityContainer.RegisterType<IEventPublisher, EventPublisher>(new ContainerControlledLifetimeManager());

            //Views
            unityContainer.RegisterType<IView, ShowsView>(ShowsViewModel.TitleText);

            //ViewModels
            unityContainer.RegisterType<IEditShowViewModel, EditShowViewModel>();

            App.UnityContainer = unityContainer;

            base.OnStartup(e);
        }

        public static IUnityContainer UnityContainer { get; private set; }
    }
}
