using ChatApp.Api.Extensions;
using ChatApp.Api.Middlewares;
using ChatApp.Domain.Configurations;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JWT"));

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions( opt => { opt.SuppressModelStateInvalidFilter = true;});

builder.Services.AddOpenApi();
builder.Services.ConfigSqlServer(builder.Configuration);
builder.Services.ConfigSwagger();
builder.Services.ConfigJwt();
builder.Services.ConfigAppInfrastructure();
builder.Services.ConfigAppServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.HandleException();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
