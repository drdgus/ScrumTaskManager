using RestSharp;
using RestSharp.Authenticators;
using ScrumTaskManager.Client.Core.Models;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ScrumTaskManager.Client.Core.Api
{
    public class RestClient
    {
        private RestSharp.RestClient restClient;

        public RestClient()
        {
            restClient = new RestSharp.RestClient("https://localhost:44424");
        }

        public async Task<IEnumerable<ToDoTask>> GetTasks()
        {
            try
            {
                var request = new RestRequest("api/v1/tasks");
                var response = await restClient.GetAsync<ToDoTask[]>(request);

                return response!;

            }
            catch (Exception e)
            {
                return Array.Empty<ToDoTask>();
            }
        }

        public async Task<bool> Login(string login, string password)
        {
            var request = new RestRequest("api/v1/login");
            request.AddJsonBody(new LoginModel
            {
                Username = login,
                Password = GetHash(password)
            });


            var response = await restClient.GetAsync(request);

            if (response.IsSuccessful)
            {
                restClient.Authenticator = new JwtAuthenticator(response.Content.Replace("\"", ""));
                return true;
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return false;

            throw new Exception($"Ошибка входа. {response.StatusCode.ToString()}");
        }

        private static string GetHash(string text)
        {
            var md5 = new MD5CryptoServiceProvider();
            return new ASCIIEncoding().GetString(md5.ComputeHash(Encoding.ASCII.GetBytes(text)));
        }
    }
}
