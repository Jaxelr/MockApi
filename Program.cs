var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 500s
app.MapGet("/item/problem", () =>
    Results.Problem(
        detail: "This is an internal error item",
        statusCode: 500,
        type: "application/problem+json",
        title: "Internal Server Error"))
.ProducesProblem(500)
.WithName("Problem")
.WithGroupName(nameof(Item));

// 200s
app.MapGet("/item/Ok/{id}", (int? id) =>
{
    if (id % 3 == 0)
    {
        var dict = new Dictionary<string, string[]>
        {
            { "Invalid Id", new[] { "The id can be divided by 3" } }
        };

        Results.ValidationProblem(dict, detail: "Validation error with the id", statusCode: 400, title: "Validation error");
    }

    var item = new Item { Id = 1, Name = "Item name", Description = "The item description" };

    return Results.Ok(item);
}
)
.ProducesValidationProblem(400, "application/validationproblem+json")
.Produces<Item>(200)
.WithName("Ok")
.WithGroupName(nameof(Item));

app.Run();

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
