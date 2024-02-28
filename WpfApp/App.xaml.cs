using App.Core.Services;
using System;
using System.Threading;
using System.Windows;

namespace WpfApp
{
    public partial class App : Application
    {
        private static Mutex _mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Créez et démarrez le serveur au lancement de l'application
            
            Thread serverThread = new Thread(ServerService.Start);
            serverThread.Start();


            const string appName = "EasySave";
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                // L'application est déjà en cours d'exécution
                MessageBox.Show("L'application est déjà en cours d'exécution.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

       
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Arrêtez le serveur lors de la fermeture de l'application
            ServerService.Stop();
        }
    }
}
