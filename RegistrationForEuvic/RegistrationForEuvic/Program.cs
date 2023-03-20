using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RegistrationForEuvic.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connect to DB
var connectionString = builder.Configuration.GetConnectionString("UserDbConnection");
builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    using (var context = serviceScope.ServiceProvider.GetRequiredService<UserDbContext>())
    {
        bool state = context.Database.CanConnect();
        if (!state)
        {
            context.Database.EnsureCreated();
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
