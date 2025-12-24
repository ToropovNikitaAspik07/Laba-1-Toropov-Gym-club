using BackendApi.Domain.Interfaces.Repositories;
using BackendApi.Domain.Interfaces.Services;
using BackendApi.Domain.Services;
using BackendApi.Infrastructure.DTO;
using BackendApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IClientRepository, ClientRepoitory>();
builder.Services.AddDbContext<ClientContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Toropov")));
builder.Services.AddScoped<ICardNumberService, CardNumberService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200") // порт Angular
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();
app.UseCors("AllowAngular");

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
