using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScrumTaskManager.Api.DAL;
using ScrumTaskManager.Api.DAL.Repositories;
using ScrumTaskManager.Api.Models;
using ScrumTaskManager.Api.Services;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using DbContext = ScrumTaskManager.Api.DAL.DbContext;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JwtAuth:Issuer"],
            ValidAudience = configuration["JwtAuth:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtAuth:Key"]))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddDbContext<DbContext>(options =>
    options.UseSqlite(configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddTransient<JWTManager>();
builder.Services.AddTransient<TasksRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DbContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

#if DEBUG
app.Use(async (context, next) =>
{
    // Do work that doesn't write to the Response.
    string logStr = $"[{DateTime.Now}]\n\t" +
                    $"---Request method---:\n\t" +
                    $"[{context.Request.Method}] {context.Request.Path}\n\t" +
                    $"---Request Headers---:\n\t" +
                    String.Join("\n\t", context.Request.Headers.ToList()) + "\n\t" +
                    $"---Request parametrs---:\n\t[" + (context.Request.QueryString.Value == "" ? "Empty" : context.Request.QueryString.Value) + "]";

    var req = context.Request;
    var bodyStr = string.Empty;

    // Allows using several time the stream in ASP.Net Core
    req.EnableBuffering();

    // Arguments: Stream, Encoding, detect encoding, buffer size 
    // AND, the most important: keep stream opened
    using (StreamReader reader
           = new StreamReader(req.Body, Encoding.UTF8, true, 20000, true))
    {
        bodyStr = reader.ReadToEndAsync().Result;
    }

    // Rewind, so the core is not lost when it looks the body for the request
    req.Body.Position = 0;

    using (var requestStream = new MemoryStream())
    {
        logStr += $"\n\t---Request Body---:\n\t[{(bodyStr.Length == 0 ? "Empty" : bodyStr)}]";
    }
    app.Logger.LogInformation(logStr);

    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
});
#endif

app.MapGet("api/v1/login", ([FromBody] LoginModel login, JWTManager jwtGenerator, DbContext dbContext) =>
{
    var userId = AuthenticateUser(login, dbContext);
    if (userId != string.Empty)
    {
        var tokenString = jwtGenerator.GenerateJWT(userId);
        return Results.Ok(tokenString);
    }

    return Results.Unauthorized();
});

string AuthenticateUser(LoginModel login, DbContext dbContext)
{
    var user = dbContext.Users.AsNoTracking().SingleOrDefault(u => u.Username == login.Username && u.Password == login.Password);
    return user != null ? user.Id : string.Empty;
}



app.MapGet("/api/v1/tasks", [Authorize] (HttpContext context, TasksRepository tasksRepository, JWTManager jwtManager) =>
{
    var regex = new Regex("(?<=Bearer ).*");
    var token = regex.Match(context.Request.Headers.Authorization.First()).Value;
    var userId = jwtManager.GetUserIdFromJWT(token);
    var tasks = tasksRepository.GetTasks(userId);
    return Results.Ok(tasks);
})
.WithName("GetTasks");

app.Run();