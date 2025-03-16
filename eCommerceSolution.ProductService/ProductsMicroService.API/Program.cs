using eCommerce.ProductService.BusinessLogicLayer;
using eCommerce.ProductService.DataAccessLayer;
using eCommerce.ProductsMicroService.API.Middleware;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


//Add DAL and BLL services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddControllers();

//FluentValidations
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

//Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
