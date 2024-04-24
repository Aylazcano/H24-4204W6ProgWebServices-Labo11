using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServeurImages.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ServeurImagesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ServeurImagesContext") ?? throw new InvalidOperationException("Connection string 'ServeurImagesContext' not found.")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermettreTout", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseCors("PermettreTout");

app.UseAuthorization();

app.MapControllers();

app.Run();
