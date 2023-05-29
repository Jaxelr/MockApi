var builder = WebApplication.CreateBuilder(args);

const string version = "v1";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc(version, new() { Title = builder.Environment.ApplicationName, Version = version }));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{version}/swagger.json", name: $"{builder.Environment.ApplicationName} {version}"));

/// 500s
// 500
app.MapGet("/item/problem", () =>
    Results.Problem(
        detail: "This is an internal error item",
        statusCode: 500,
        title: "Internal Server Error",
        type: "application/problem+json"))
.WithName("Problem")
.ProducesProblem(500, "application/problem+json");

/// 400s
// 400
app.MapGet("/item/badrequest", () =>
    {
        var dict = new Dictionary<string, string[]>
        {
            { "Invalid Id", new[] { "This path will always return invalid" } }
        };

        Results.ValidationProblem(
            dict,
            detail: "Validation error with the id",
            statusCode: 400,
            title: "Validation error",
            type: "application/problem+json");
    })
    .ProducesValidationProblem(400, "application/validationproblem+json")
    .WithName("ValidationProblem");

// 429
app.MapGet("/item/toomanyrequests", (HttpResponse response) =>
    {
        response.Headers.Add("Retry-After", "120");

        return Results.Problem(
            detail: "Validation error with the id",
            statusCode: 429,
            title: "Too many requests error",
            type: "application/problem+json");
    })
    .ProducesProblem(429, "application/validationproblem+json")
    .WithName("ValidationProblem");


/// 200s
//201
app.MapGet("/item/accepted", () => Results.Accepted())
    .WithName("Accepted");

//204
app.MapGet("/item/empty", () => Results.NoContent())
    .WithName("NoItem");

//200
app.MapGet("/item/Ok/{id}", (int? id) =>
{
    id ??= 0;

    return Results.Ok(new Item(id.Value));
})
.Produces<Item>(200)
.WithName("Ok");

app.Run();
