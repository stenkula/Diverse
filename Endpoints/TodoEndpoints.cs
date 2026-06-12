public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this WebApplication app)
    {
        app.MapGet("/todos", (List<Todo> todos) => Results.Ok(todos));

        app.MapPost("/todos", (Todo todo, List<Todo> todos) =>
        {
            todos.Add(todo);
            return Results.Created($"/todos/{todo.Id}", todo);
        });

        app.MapDelete("/todos/{id}", (Guid id, List<Todo> todos) =>
        {
            var todo = todos.FirstOrDefault(t => t.Id == id);
            if (todo is null) return Results.NotFound();
            todos.Remove(todo);
            return Results.NoContent();
        });
    }
}