using GoogleSchoolProjectManager.UI.ViewModel;
using MahApps.Metro.Controls.Dialogs;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GoogleSchoolProjectManager.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            //TODO: Paste here all things that needs to be initialized in ViewModels
            this.container = new StandardKernel();
            container.Bind<IViewModel>().To<FakeMainViewModel>().InThreadScope();
            container.Bind<IDialogCoordinator>().To<DialogCoordinator>().InThreadScope();
        }

        private void ComposeObjects()
        {
            Current.MainWindow = this.container.Get<MainWindow>();
        }
    }
}
