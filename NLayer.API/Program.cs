using Microsoft.EntityFrameworkCore;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using System.Reflection;
using NLayer.Repository.UnitOfWorks;
using NLayer.Core.Repositories;
using NLayer.Repository.Repositories;
using NLayer.Core.Services;
using NLayer.Service.Services;
using NLayer.Service.Mapping;
using NLayer.Service.ValidationRules;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(i => i.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


builder.Services.AddDbContext<VtContext>(i =>
{
    i.UseSqlServer(builder.Configuration.GetConnectionString("Local"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(VtContext)).GetName().Name);
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
