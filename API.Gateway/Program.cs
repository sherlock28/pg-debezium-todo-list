using API.Gateway.Data;
using API.Gateway.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<TodoDBContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/todo", async (Todo todo, TodoDBContext dbContext) => 
{
    dbContext.Todo.Add(todo);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/api/todo/{todo.Id}", todo);
});

app.MapGet("/api/todo/{id:int}", async (int id, TodoDBContext dbContext) =>
{
    return await dbContext.Todo.FindAsync(id)
          is Todo todo
          ? Results.Ok(todo)
          : Results.NotFound();
});

app.MapGet("/api/todo", async (TodoDBContext dbContext) => await dbContext.Todo.ToListAsync());

app.MapPut("/api/todo/{id:int}", async (int id, Todo c, TodoDBContext dbContext) =>
{
    if (c.Id != id)
        return Results.BadRequest();

    var todo = await dbContext.Todo.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Description = c.Description;

    await dbContext.SaveChangesAsync();

    return Results.Ok(todo);
});

app.MapDelete("/api/todo/{id:int}", async (int id, TodoDBContext dbContext) =>
{
    var todo = await dbContext.Todo.FindAsync(id);

    if (todo is null) return Results.NotFound();

    dbContext.Todo.Remove(todo);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});


app.Run();
