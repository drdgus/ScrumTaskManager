namespace ScrumTaskManager.Api.DAL.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<ToDoTask> Tasks { get; set; }
    }
}
