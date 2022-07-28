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

            var rnd = new Random();

            var tags = new List<Tag>()
            {
                new Tag
                {
                    Id = 1,
                    Name = "Api"
                },
                new Tag
                {
                    Id = 2,
                    Name = "Service"
                },
                new Tag
                {
                    Id = 3,
                    Name = "WPF"
                },
            };

            context.Tags.AddRange(tags);

            context.Tasks.AddRange(Enumerable.Range(1, 20).Select(i => new ToDoTask
            {
                Header = $"Задача {i}",
                Type = (ToDoTaskType)rnd.Next(0, 12),
                State = (ToDoTaskState)rnd.Next(0, 4),
                Name = $"tsk-{i}",
                Description = "",
                Tags = tags.Take(rnd.Next(0, 3)).ToList(),
                Priority = (Priority)rnd.Next(0, 3),
                TimeLimit = new TimeSpan(rnd.Next(1, 21), rnd.Next(0, 60), 0),
                PerformerId = user.Entity.Id
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
