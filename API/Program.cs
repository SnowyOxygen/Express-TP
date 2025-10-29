using Microsoft.EntityFrameworkCore;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using API.DependancyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCategoryMappings();
builder.Services.AddProductMappings();
builder.Services.AddOrderMappings();

// Use in-memory database for the ExpressDataContext (suitable for dev/testing)
builder.Services.AddDbContext<ExpressDataContext>(options
    => options.UseInMemoryDatabase("ExpressDb"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExpressDataContext>();
    await db.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
