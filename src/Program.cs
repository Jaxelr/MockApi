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
        statusCode: StatusCodes.Status500InternalServerError,
        title: "Internal Server Error",
        type: "application/problem+json"))
.WithName("Problem")
.ProducesProblem(StatusCodes.Status500InternalServerError, "application/problem+json");

/// 400s
// 400
app.MapGet("/item/badrequest", () =>
    {
        var dict = new Dictionary<string, string[]> { { "Invalid Id", new[] { "This path will always return invalid" } } };

        Results.ValidationProblem(
            dict,
            detail: "Validation error with the id",
            statusCode: StatusCodes.Status400BadRequest,
            title: "Validation error",
            type: "application/problem+json");
    })
    .ProducesValidationProblem(StatusCodes.Status400BadRequest, "application/validationproblem+json")
    .WithName("ValidationProblem");

// 429
app.MapGet("/item/toomanyrequests", (HttpResponse response) =>
    {
        response.Headers.Append("Retry-After", "120");

        return Results.Problem(
            detail: "Validation error with the id",
            statusCode: StatusCodes.Status429TooManyRequests,
            title: "Too many requests error",
            type: "application/problem+json");
    })
    .ProducesProblem(StatusCodes.Status429TooManyRequests, "application/validationproblem+json")
    .WithName("ValidationProblem");

/// 200s
//200
app.MapGet("/item/Ok/{id}", (int? id) =>
{
    id ??= 0;

    return Results.Ok(new Item(id.Value));
})
.Produces<Item>(StatusCodes.Status200OK)
.WithName("Ok");

//201
app.MapGet("/item/created", () => Results.Created())
    .Produces(StatusCodes.Status201Created)
    .WithName("Created");

//202
app.MapGet("/item/accepted", () => Results.Accepted())
    .Produces(StatusCodes.Status202Accepted)
    .WithName("Accepted");

//204
app.MapGet("/item/empty", () => Results.NoContent())
    .Produces(StatusCodes.Status204NoContent)
    .WithName("NoItem");

app.Run();
