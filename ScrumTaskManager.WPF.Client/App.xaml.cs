using Microsoft.Extensions.DependencyInjection;
using ScrumTaskManager.Client.Core.Api;
using ScrumTaskManager.Client.Core.Services;
using ScrumTaskManager.WPF.Client.ViewModels;
using ScrumTaskManager.WPF.Client.Views;
using System;
using System.Windows;

namespace ScrumTaskManager.WPF.Client
{
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
        }

        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<RestClient>();
            services.AddSingleton<IAuthorizationManager, AuthorizationManager>();
            services.AddSingleton<ITasksManager, TasksManager>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<TasksViewModel>();

            return services.BuildServiceProvider();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var loginWindow = new LoginWindow();

            var authorizationManager = Services.GetRequiredService<IAuthorizationManager>();
            authorizationManager.OnLogin += successLogin =>
            {
                if (successLogin)
                    loginWindow.Hide();
            };

            loginWindow.ShowDialog();

            var mainWindow = new MainWindow();
            mainWindow.Show();
            loginWindow.Close();
        }
    }
}
