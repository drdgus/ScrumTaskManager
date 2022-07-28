using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ScrumTaskManager.Client.Core.Services;

namespace ScrumTaskManager.WPF.Client.ViewModels;
[ObservableObject]
public partial class LoginViewModel
{
    private readonly IAuthorizationManager _authorizationManager;

    public string Username { get; set; }
    public string Password { get; set; }

    public LoginViewModel(IAuthorizationManager authorizationManager)
    {
        _authorizationManager = authorizationManager;
    }

    [ICommand]
    public void Login()
    {
        _authorizationManager.Login(Username, Password);
    }
}
