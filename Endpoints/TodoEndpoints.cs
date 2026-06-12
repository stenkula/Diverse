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

        app.MapPut("/todos/{id}", (Guid id, Todo updatedTodo, List<Todo> todos) =>
        {
            var todo = todos.FirstOrDefault(t => t.Id == id);
            if (todo is null) return Results.NotFound();
            todo.Title = updatedTodo.Title;
            todo.Description = updatedTodo.Description;
            todo.IsCompleted = updatedTodo.IsCompleted;
            return Results.Ok(todo);
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