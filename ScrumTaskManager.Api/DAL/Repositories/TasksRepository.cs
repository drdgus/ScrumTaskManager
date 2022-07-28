using ScrumTaskManager.Api.DAL.Entities;

namespace ScrumTaskManager.Api.DAL.Repositories
{
    public class TasksRepository
    {
        private readonly DbContext _dbContext;

        public TasksRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ToDoTask> GetTasks(string userId)
        {
            return _dbContext.Tasks.Where(t => t.PerformerId == userId).ToList();
        }
    }
}
