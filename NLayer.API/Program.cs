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
using NLayer.API.Filters;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Middleware;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using NLayer.API.Modules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(i => { 
    i.Filters.Add(new ValidateFilterAttribute()); 
}).AddFluentValidation(i => i.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());


builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddScoped(typeof(NotFoundFilter<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddAutoMapper(typeof(MapProfile));


builder.Services.AddDbContext<VtContext>(i =>
{
    i.UseSqlServer(builder.Configuration.GetConnectionString("Local"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(VtContext)).GetName().Name);
    });
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();
