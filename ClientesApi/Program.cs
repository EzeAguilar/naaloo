using ClientesApi.Middleware;
using ClientesApi.Models;
using ClientesApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ClienteStore>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestTimingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
