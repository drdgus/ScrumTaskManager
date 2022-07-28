using ScrumTaskManager.Client.Core.Models;
using System.Collections.ObjectModel;
using RestClient = ScrumTaskManager.Client.Core.Api.RestClient;

namespace ScrumTaskManager.Client.Core.Services
{
    public class TasksManager : ITasksManager
    {
        private readonly RestClient _restClient;
        public ObservableCollection<ToDoTask> TasksStack { get; } = new();
        public ObservableCollection<ToDoTask> TasksInWork { get; } = new();
        public ObservableCollection<ToDoTask> TasksInTests { get; } = new();
        public ObservableCollection<ToDoTask> CompletedTasks { get; } = new();

        public TasksManager(RestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task GetTasks()
        {
            foreach (var task in await _restClient.GetTasks())
            {
                switch (task.State)
                {
                    case ToDoTaskState.Stack:
                        TasksStack.Add(task);
                        break;
                    case ToDoTaskState.InWork:
                        TasksInWork.Add(task);
                        break;
                    case ToDoTaskState.InTests:
                        TasksInTests.Add(task);
                        break;
                    case ToDoTaskState.Done:
                        CompletedTasks.Add(task);
                        break;
                }
            }
        }
    }
}
