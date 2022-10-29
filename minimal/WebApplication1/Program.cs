var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("get-example", () => "Hello get" );
app.MapPost("post-example", () => "Hello post");
app.MapGet("ok-object", () => Results.Ok(new
{
    Message = "Hello ok object"
}));
app.MapGet("asyn-response", async () =>
{
    await Task.Delay(1000);
    return Results.Ok(new
    {
        Message = "Hello ok async"
    });
});

app.MapGet("get-params/{age:int}", (int age) => $"Age provided was {age}");
app.MapGet("cars/{carId:regex(^[a-z0-9]+$)}", (string carId) => $"Card id provided was: {carId}");
app.MapGet("books/{isbn:length(13)}", (string isbn) => $"ISBN was: {isbn}");


app.Run();

