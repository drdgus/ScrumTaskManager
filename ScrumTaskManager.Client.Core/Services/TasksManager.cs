using ScrumTaskManager.Client.Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RestClient = ScrumTaskManager.Client.Core.Api.RestClient;

namespace ScrumTaskManager.Client.Core.Services
{
    public class TasksManager : ITasksManager
    {
        private readonly RestClient _restClient;
        public ObservableCollection<ToDoTask> Tasks { get; } = new();

        public TasksManager(RestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task GetTasks()
        {
            var tasks = await _restClient.GetTasks();
            foreach (var task in tasks) Tasks.Add(task);
        }

        public async Task UpdateStatus(ToDoTask task)
        {
            await _restClient.UpdateTaskStatus(task.Id, task.Status);
        }

        public async Task CreateTask(ToDoTask task)
        {
            var newTask = await _restClient.CreateTask(task);
            Tasks.Add(newTask);
        }

        public async Task DeleteTask(ToDoTask task)
        {
            await _restClient.DeleteTask(task.Id);
            Tasks.Remove(task);
        }
    }
}
