using TodoApi.Data;
using TodoApi.Data.Repositories;
using TodoApi.Middlewares;
using TodoApi.Models;
using TodoApi.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TodoDatabaseSettings>(
    builder.Configuration.GetSection("TodoDatabase"));

builder.Services.AddSingleton<ITodoRepository, TodoRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ErrorHandlingMiddleware>();


app.MapGet("/v1/todos", async (ITodoRepository db) =>
    await db.GetAsync());


app.MapGet("/v1/todos/{id}", async (string id, ITodoRepository db) =>
    await db.GetAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());


app.MapPost("/v1/todos", async (CreateTodoViewModel model, ITodoRepository db) =>
{
    var todo = model.MapTo();
    if (!model.IsValid) return Results.BadRequest(model.Notifications);
    await db.CreateAsync(todo);
    return Results.Created($"/todoitems/{todo.Id}", todo);
});


app.MapPut("/todos/{id}", async (string id, UpdateTodoViewModel model, ITodoRepository db) =>
{
    var todo = await db.GetAsync(id);
    if (todo is null) return Results.NotFound();
    model.MapTo(todo);
    if (!model.IsValid) return Results.BadRequest(model.Notifications);
    await db.UpdateAsync(todo.Id, todo);
    return Results.Ok();
});


app.MapDelete("/todos/{id}", async (string id, ITodoRepository db) =>
{
    var todo = await db.GetAsync(id);
    if (todo is null) return Results.NotFound();
    await db.RemoveAsync(todo.Id);
    return Results.Ok();
});


app.Run();