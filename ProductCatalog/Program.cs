using ProductCatalog;
using ProductCatalog.Infra;
using ProductCatalog.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<MongoContext>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.Configure<CatalogSettings>(
    builder.Configuration.GetSection("CatalogSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
