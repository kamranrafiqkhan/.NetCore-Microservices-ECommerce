using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using FluentValidation;
using FluentValidation.Results;
using System.Runtime.CompilerServices;

namespace eCommerce.ProductsMicroService.API.APIEndpoints;

public static class ProductAPIEndpoints
{
    public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        //GET /api/products
        app.MapGet("/api/products", async (IProductService productService) =>
        {
            List<ProductResponse?> products = await productService.GetProducts();
            return Results.Ok(products);
        });

        //GET /api/products/search/product-id/wsde-fddfs-asas-asas
        app.MapGet("/api/products/search/product-id/{ProductID:guid}", async (IProductService productService, Guid ProductID) =>
        {
            ProductResponse? product = await productService.GetProductByCondition(temp => temp.ProductId == ProductID);
            return Results.Ok(product);
        });

        //GET /api/products/search/xxxxxxxxxxxxxxxxxx
        app.MapGet("/api/products/search/{SearchString}", async (IProductService productService, string SearchString) =>
        {
            IEnumerable<ProductResponse?> productsByProductName = await productService.GetProductsByCondition(temp => temp.ProductName != null && temp.ProductName.Contains(SearchString, StringComparison.OrdinalIgnoreCase));
            IEnumerable<ProductResponse?> productsByCategory = await productService.GetProductsByCondition(temp => temp.Category != null && temp.Category.Contains(SearchString, StringComparison.OrdinalIgnoreCase));

            var products = productsByProductName.Union(productsByCategory);

            return Results.Ok(products);
        });

        //POST /api/products
        app.MapPost("/api/products", async (IProductService productService, IValidator<ProductAddRequest> validator, ProductAddRequest productAddRequest) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(productAddRequest);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                                                    .GroupBy(temp => temp.PropertyName)
                                                    .ToDictionary(grp => grp.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }

            ProductResponse productResponse = await productService.AddProduct(productAddRequest);

            if (productResponse != null)
                return Results.Created($"/api/products/search/product-id/{productResponse.ProductId}", productResponse);
            else
                return Results.Problem("Error in adding product");
        });


        //PUT /api/products
        app.MapPut("/api/products", async (IProductService productService, IValidator<ProductUpdateRequest> validator, ProductUpdateRequest productUpdateRequest) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(productUpdateRequest);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                                                    .GroupBy(temp => temp.PropertyName)
                                                    .ToDictionary(grp => grp.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }

            ProductResponse? productResponse = await productService.UpdateProduct(productUpdateRequest);

            if (productResponse != null)
                return Results.Ok(productResponse);
            else
                return Results.Problem("Error in updating product");
        });


        //DELETE /api/products/xxxxxxxxxxxxxxxxxxxxx
        app.MapDelete("/api/products/{ProductId:guid}", async (IProductService productService, Guid ProductId) =>
        {

            bool isDeleted = await productService.DeleteProduct(ProductId);

            if (isDeleted)
                return Results.Ok(true);
            else
                return Results.Problem("Error in deleting product");
        });

        return app;
    }
}
