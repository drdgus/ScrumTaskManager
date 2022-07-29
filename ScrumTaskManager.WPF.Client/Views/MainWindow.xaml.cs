using Microsoft.Extensions.DependencyInjection;
using ScrumTaskManager.WPF.Client.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace ScrumTaskManager.WPF.Client.Views;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<TasksViewModel>();
    }
}