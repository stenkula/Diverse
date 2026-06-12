var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


var todos = new List<Todo>();
todos.Add(new Todo { Title = "Buy groceries", Description = "Milk, Bread, Eggs"});


app.MapGet("/todos", () => Results.Ok(todos));

app.Run();

public class Todo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
}