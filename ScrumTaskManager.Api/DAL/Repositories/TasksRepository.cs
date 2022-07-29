using Microsoft.EntityFrameworkCore;
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
            return _dbContext.Tasks
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .ToList();
        }

        public async Task UpdateStatus(int id, ToDoTaskStatus toDoTaskStatus)
        {
            var task = await _dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == id);
            if (task != null)
            {
                task.Status = toDoTaskStatus;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<ToDoTask> Add(ToDoTask task, string userId)
        {
            _dbContext.Users.Include(u => u.Tasks).Single(u => u.Id == userId).Tasks.Add(task);
            await _dbContext.SaveChangesAsync();
            task.Name = $"tsk-{task.Id}";
            await _dbContext.SaveChangesAsync();
            return task;
        }

        public async Task Remove(int id)
        {
            var task = await _dbContext.Tasks.SingleAsync(t => t.Id == id);
            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();
        }
    }
}
