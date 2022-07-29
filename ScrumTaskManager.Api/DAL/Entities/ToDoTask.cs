namespace ScrumTaskManager.Api.DAL.Entities;

public class ToDoTask
{
    public int Id { get; set; }
    public string Header { get; set; } = null!;
    public ToDoTaskType Type { get; set; }
    public ToDoTaskStatus Status { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Priority Priority { get; set; }
    public TimeSpan TimeLimit { get; set; }
    public string UserId { get; set; } = null!;
}