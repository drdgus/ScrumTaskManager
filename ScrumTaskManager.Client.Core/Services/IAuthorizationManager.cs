namespace ScrumTaskManager.Client.Core.Services
{
    public interface IAuthorizationManager
    {
        Task Login(string login, string password);
        event Action<bool> OnLogin;
    }
}
