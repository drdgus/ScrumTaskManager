using Microsoft.Extensions.DependencyInjection;
using ScrumTaskManager.WPF.Client.ViewModels;
using System.Windows;

namespace ScrumTaskManager.WPF.Client.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<LoginViewModel>();
        }
    }
}
