using App.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.ViewModels
{
    public class MenuViewModel
    {
        public RelayCommand LogCommand { get; }
        public RelayCommand StateCommand { get; }
        private readonly StateManagerService stateManagerService = new();
        private readonly LoggerService loggerService = new();

        public MenuViewModel()
        {
            StateCommand = new RelayCommand(OpenState);
            LogCommand = new RelayCommand(OpenLog);
        }

        public void OpenState()
        {
            stateManagerService.OpenStateFile();
        }

        public void OpenLog()
        {
            loggerService.OpenLogFile();
        }
    }
}
