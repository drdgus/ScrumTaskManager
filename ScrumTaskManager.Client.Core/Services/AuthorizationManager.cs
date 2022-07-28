﻿using ScrumTaskManager.Client.Core.Api;

namespace ScrumTaskManager.Client.Core.Services
{
    public class AuthorizationManager : IAuthorizationManager
    {
        private readonly RestClient _restClient;

        public AuthorizationManager(RestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<bool> Login(string login, string password)
        {
            var res = await _restClient.Login(login, password);
            OnLogin?.Invoke(res);
            return res;
        }

        public event Action<bool>? OnLogin;
    }
}
