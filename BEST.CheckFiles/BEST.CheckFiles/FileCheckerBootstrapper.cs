using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BEST.CheckFiles.ViewModels;
using Caliburn.Micro;

namespace BEST.CheckFiles
{
    class FileCheckerBootstrapper : BootstrapperBase
    {
        public FileCheckerBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<FileCheckerViewModel>();
        }
    }
}
