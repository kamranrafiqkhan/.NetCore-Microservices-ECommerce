using eCommerce.ProductService.BusinessLogicLayer;
using eCommerce.ProductService.DataAccessLayer;
using eCommerce.ProductsMicroService.API.APIEndpoints;
using eCommerce.ProductsMicroService.API.Middleware;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


//Add DAL and BLL services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddControllers();

//FluentValidations
builder.Services.AddFluentValidationAutoValidation();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//Add Swagger service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Products API", Version = "v1" });
});


//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

//Cors
app.UseCors();

//Swagger
app.UseSwagger();
app.UseSwaggerUI();

//Authentication
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapProductAPIEndpoints();

app.Run();
