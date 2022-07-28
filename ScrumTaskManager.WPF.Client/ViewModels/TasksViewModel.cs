using ScrumTaskManager.Client.Core.Models;
using ScrumTaskManager.Client.Core.Services;
using System.Collections.ObjectModel;

namespace ScrumTaskManager.WPF.Client.ViewModels;
public class TasksViewModel
{
    private readonly ITasksManager _tasksManager;

    public ObservableCollection<ToDoTask> TasksStack => _tasksManager.TasksStack;
    public ObservableCollection<ToDoTask> TasksInWork => _tasksManager.TasksInWork;
    public ObservableCollection<ToDoTask> TasksInTests => _tasksManager.TasksInTests;
    public ObservableCollection<ToDoTask> CompletedTasks => _tasksManager.CompletedTasks;

    public TasksViewModel(ITasksManager tasksManager)
    {
        _tasksManager = tasksManager;
        _tasksManager.GetTasks();
    }
}
