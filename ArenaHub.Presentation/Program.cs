using System.Text.Json.Serialization;
using ArenaHub.Application;
using ArenaHub.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => o.SwaggerDoc("v1", new() { Title = "ArenaHub API", Version = "v1" }));

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration.GetConnectionString("ArenaHub")!);

var app = builder.Build();

app.Services.AplicarMigraciones();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();

public partial class Program;
