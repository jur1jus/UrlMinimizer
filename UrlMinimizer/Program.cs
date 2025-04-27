using UrlMinimizer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<AppInstanceSettings>(builder.Configuration.GetSection("AppInstanceSettings"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICodeGenerator, CodeGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/generate", async (ICodeGenerator codeGenerator) =>
{
    var code = await codeGenerator.Generate();
    return Results.Ok(code);
});

app.UseHttpsRedirection();

app.Run();

public class AppInstanceSettings
{
    public string AppInstanceName { get; init; }
}
