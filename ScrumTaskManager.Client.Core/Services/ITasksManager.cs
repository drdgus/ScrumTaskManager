using ScrumTaskManager.Client.Core.Models;
using System.Collections.ObjectModel;

namespace ScrumTaskManager.Client.Core.Services;
public interface ITasksManager
{
    ObservableCollection<ToDoTask> TasksStack { get; }
    ObservableCollection<ToDoTask> TasksInWork { get; }
    ObservableCollection<ToDoTask> TasksInTests { get; }
    ObservableCollection<ToDoTask> CompletedTasks { get; }
    Task GetTasks();
}
