using System;
using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using ScrumTaskManager.Client.Core.Models;
using ScrumTaskManager.Client.Core.Services;
using ScrumTaskManager.WPF.Client.Views;
using System.ComponentModel;
using System.Windows.Data;
using ToDoTask = ScrumTaskManager.Client.Core.Models.ToDoTask;

namespace ScrumTaskManager.WPF.Client.ViewModels;
public partial class TasksViewModel
{
    private readonly ITasksManager _tasksManager;

    public ICollectionView TasksStack { get; }
    public ICollectionView TasksInWork { get; }
    public ICollectionView TasksInTests { get; }
    public ICollectionView CompletedTasks { get; }
    public SnackbarMessageQueue MessageQueue { get; } = new (TimeSpan.FromSeconds(3));

    private ToDoTask? _newTask;

    public TasksViewModel(ITasksManager tasksManager)
    {
        _tasksManager = tasksManager;
        _tasksManager.GetTasks();

        TasksStack = new CollectionViewSource { Source = _tasksManager.Tasks }.View;
        TasksInWork = new CollectionViewSource { Source = _tasksManager.Tasks }.View;
        TasksInTests = new CollectionViewSource { Source = _tasksManager.Tasks }.View;
        CompletedTasks = new CollectionViewSource { Source = _tasksManager.Tasks }.View;

        TasksStack.Filter += o => ((ToDoTask)o).Status == ToDoTaskStatus.Stack;
        TasksInWork.Filter += o => ((ToDoTask)o).Status == ToDoTaskStatus.InWork;
        TasksInTests.Filter += o => ((ToDoTask)o).Status == ToDoTaskStatus.InTests;
        CompletedTasks.Filter += o => ((ToDoTask)o).Status == ToDoTaskStatus.Done;
    }

    [ICommand]
    public async void OpenTask(object param)
    {
        var task = (ToDoTask)param;
        var view = new TaskDialog()
        {
            DataContext = task
        };

        var oldStatus = task.Status;

        await DialogHost.Show(view, "RootDialog");

        var newStatus = task.Status;

        if (oldStatus == newStatus) return;

        RefreshTaskLists();

        await _tasksManager.UpdateStatus(task);
    }

    [ICommand]
    public async void CreateTask()
    {
        if (_newTask == null)
        {
            _newTask = new ToDoTask
            {
                Name = string.Empty,
            };
        }

        var view = new NewTaskDialog()
        {
            DataContext = _newTask
        };


        var res = await DialogHost.Show(view, "RootDialog");

        if (res != null && (bool)res)
        {
            
            try
            {
                await _tasksManager.CreateTask(_newTask);
                MessageQueue.Enqueue("Задача добавлена");
            }
            catch(Exception e)
            {
                MessageQueue.Enqueue("Ошибка добавления. " + e.Message);
            }

            _newTask = new ToDoTask()
            {
                Name = string.Empty,
            };
        }

        RefreshTaskLists();
    }

    [ICommand]
    public async void DeleteTask(object param)
    {
        try
        {
            await _tasksManager.DeleteTask((ToDoTask)param);
            DialogHost.CloseDialogCommand.Execute(null, null);
            MessageQueue.Enqueue("Задача удалена");
        }
        catch (Exception e)
        {
            MessageQueue.Enqueue("Ошибка удаления. " + e.Message);
        }
       
        RefreshTaskLists();
    }

    private void RefreshTaskLists()
    {
        TasksStack.Refresh();
        TasksInWork.Refresh();
        TasksInTests.Refresh();
        CompletedTasks.Refresh();
    }
}
