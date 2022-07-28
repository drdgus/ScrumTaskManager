namespace ScrumTaskManager.Api.DAL.Entities;

public class ToDoTask
{
    public int Id { get; set; }
    public string Header { get; set; } = null!;
    public ToDoTaskType Type { get; set; }
    public ToDoTaskState State { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<Tag> Tags { get; set; } = null!;
    public Priority Priority { get; set; }
    public TimeSpan TimeLimit { get; set; }
    public string PerformerId { get; set; } = null!;
}