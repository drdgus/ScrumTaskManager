using ScrumTaskManager.Client.Core.Models;
using System.Collections.ObjectModel;

namespace ScrumTaskManager.Client.Core.Services;
public interface ITasksManager
{
    ObservableCollection<ToDoTask> Tasks { get; }
    Task GetTasks();
    Task UpdateStatus(ToDoTask task);
    Task CreateTask(ToDoTask task);
    Task DeleteTask(ToDoTask task);
}
