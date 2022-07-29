using ScrumTaskManager.Api.DAL.Entities;

namespace ScrumTaskManager.Api.Models
{
    public class UpdateTaskStatusModel
    {
        public int Id { get; set; }
        public ToDoTaskStatus Status { get; set; }
    }
}
