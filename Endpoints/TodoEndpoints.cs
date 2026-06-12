using Microsoft.EntityFrameworkCore;
public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this WebApplication app)
    {
        app.MapGet("/todos", async (TodoDbContext db) =>
            Results.Ok(await db.Todos.ToListAsync()));

        app.MapPost("/todos", async (Todo todo, TodoDbContext db) =>
        {
            db.Todos.Add(todo);
            await db.SaveChangesAsync();
            return Results.Created($"/todos/{todo.Id}", todo);
        });

        app.MapPut("/todos/{id}", async (Guid id, Todo updatedTodo, TodoDbContext db) =>
        {
            var todo = await db.Todos.FindAsync(id);
            if (todo is null) return Results.NotFound();
            todo.Title = updatedTodo.Title;
            todo.Description = updatedTodo.Description;
            todo.IsCompleted = updatedTodo.IsCompleted;
            await db.SaveChangesAsync();
            return Results.Ok(todo);
        });

         app.MapDelete("/todos/{id}", async (Guid id, TodoDbContext db) =>
        {
            var todo = await db.Todos.FindAsync(id);
            if (todo is null) return Results.NotFound();
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}