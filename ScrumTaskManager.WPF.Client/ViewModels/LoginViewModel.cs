using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ScrumTaskManager.Client.Core.Services;
using System;

namespace ScrumTaskManager.WPF.Client.ViewModels;
[ObservableObject]
public partial class LoginViewModel
{
    private readonly IAuthorizationManager _authorizationManager;

    public string Username { get; set; }
    public string Password { get; set; }

    public SnackbarMessageQueue MessageQueue { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

    public LoginViewModel(IAuthorizationManager authorizationManager)
    {
        _authorizationManager = authorizationManager;
    }

    [ICommand]
    public async void Login()
    {
        try
        {
            await _authorizationManager.Login(Username, Password);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Unauthorized"))
                MessageQueue.Enqueue("Неверный логин или пароль");
            else
                MessageQueue.Enqueue(e.Message);
        }

    }
}
