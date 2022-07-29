using ScrumTaskManager.Api.DAL.Entities;
using System.Security.Cryptography;
using System.Text;

namespace ScrumTaskManager.Api.DAL
{
    public class DbInitializer
    {
        public static void Initialize(DbContext context)
        {
            if (context.Users.Any()) return;

            var user = context.Users.Add(new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = "Test",
                Password = GetHash("Test")
            });

            context.Users.Add(new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = "Test2",
                Password = GetHash("Test2")
            });

            var rnd = new Random();

            context.Tasks.Add(new ToDoTask
            {
                Header = "Не подгружаются теги",
                Type = ToDoTaskType.Bug,
                Status = ToDoTaskStatus.Stack,
                Name = "tsk-1",
                Description = string.Empty,
                Priority = Priority.High,
                TimeLimit = TimeSpan.FromHours(4),
                UserId = user.Entity.Id
            });

            context.Tasks.Add(new ToDoTask
            {
                Header = "Роли для пользователей",
                Type = ToDoTaskType.Task,
                Status = ToDoTaskStatus.Stack,
                Name = "tsk-2",
                Description = string.Empty,
                Priority = Priority.Low,
                TimeLimit = TimeSpan.FromHours(4),
                UserId = user.Entity.Id
            });

            context.Tasks.Add(new ToDoTask
            {
                Header = "Уведомления о назначенных или о подписанных задачах",
                Type = ToDoTaskType.Task,
                Status = ToDoTaskStatus.Stack,
                Name = "tsk-3",
                Description = string.Empty,
                Priority = Priority.Low,
                TimeLimit = TimeSpan.FromHours(4),
                UserId = user.Entity.Id
            });

            context.Tasks.Add(new ToDoTask
            {
                Header = "Бэклог",
                Type = ToDoTaskType.Task,
                Status = ToDoTaskStatus.Stack,
                Name = "tsk-4",
                Description = string.Empty,
                Priority = Priority.Low,
                TimeLimit = TimeSpan.FromHours(2),
                UserId = user.Entity.Id
            });

            context.Tasks.Add(new ToDoTask
            {
                Header = "Спринты",
                Type = ToDoTaskType.Task,
                Status = ToDoTaskStatus.Stack,
                Name = "tsk-5",
                Description = string.Empty,
                Priority = Priority.Low,
                TimeLimit = TimeSpan.FromHours(4),
                UserId = user.Entity.Id
            });

            context.Tasks.Add(new ToDoTask
            {
                Header = "Метки для типов задач",
                Type = ToDoTaskType.Task,
                Status = ToDoTaskStatus.Stack,
                Name = "tsk-6",
                Description = string.Empty,
                Priority = Priority.Mid,
                TimeLimit = TimeSpan.FromHours(1),
                UserId = user.Entity.Id
            });

            context.Tasks.Add(new ToDoTask
            {
                Header = "Метки для приоритета задачи",
                Type = ToDoTaskType.Task,
                Status = ToDoTaskStatus.Stack,
                Name = "tsk-7",
                Description = string.Empty,
                Priority = Priority.Mid,
                TimeLimit = TimeSpan.FromHours(1),
                UserId = user.Entity.Id
            });

            context.Tasks.Add(new ToDoTask
            {
                Header = "Интеграция с Git",
                Type = ToDoTaskType.Task,
                Status = ToDoTaskStatus.Stack,
                Name = "tsk-8",
                Description = string.Empty,
                Priority = Priority.Low,
                TimeLimit = TimeSpan.FromHours(4),
                UserId = user.Entity.Id
            });

            context.Tasks.Add(new ToDoTask
            {
                Header = "Передача задачи другому исполнителю",
                Type = ToDoTaskType.Task,
                Status = ToDoTaskStatus.Stack,
                Name = "tsk-9",
                Description = string.Empty,
                Priority = Priority.Mid,
                TimeLimit = TimeSpan.FromHours(2),
                UserId = user.Entity.Id
            });

            context.Tasks.AddRange(Enumerable.Range(10, 20).Select(i => new ToDoTask
            {
                Header = $"Задача {i}",
                Type = (ToDoTaskType)rnd.Next(0, 2),
                Status = (ToDoTaskStatus)rnd.Next(0, 4),
                Name = $"tsk-{i}",
                Description = $"Description {i} " + Guid.NewGuid(),
                Priority = (Priority)rnd.Next(0, 3),
                TimeLimit = new TimeSpan(rnd.Next(1, 21), rnd.Next(0, 60), 0),
                UserId = user.Entity.Id
            }));

            context.SaveChanges();
        }

        private static string GetHash(string text)
        {
            var md5 = new MD5CryptoServiceProvider();
            return new ASCIIEncoding().GetString(md5.ComputeHash(Encoding.ASCII.GetBytes(text)));
        }
    }
}
