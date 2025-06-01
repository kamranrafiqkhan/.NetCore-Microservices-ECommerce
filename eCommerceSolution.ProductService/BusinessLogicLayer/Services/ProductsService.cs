
using AutoMapper;
using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using eCommerce.DataAccessLayer.Entities;
using eCommerce.DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace eCommerce.BusinessLogicLayer.Services;

public class ProductsService : IProductService
{
    private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
    private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
    private readonly IMapper _mapper;
    private readonly IProductsRepository _productsRepository;

    public ProductsService(IValidator<ProductAddRequest> producAddRequestValidator, IValidator<ProductUpdateRequest> productUpdateRequestValidator, IMapper mapper, IProductsRepository productsRepository)
    {
        _productAddRequestValidator = producAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
        _mapper = mapper;
        _productsRepository = productsRepository;
    }
    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        if(productAddRequest == null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));
        }

        ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }

        Product product = _mapper.Map<Product>(productAddRequest);

        Product? addedProduct = await _productsRepository.AddProduct(product);

        if (addedProduct == null)
        {
            return null;
        }

        ProductResponse response = _mapper.Map<ProductResponse>(addedProduct);

        return response;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        Product product = await _productsRepository.GetProductByCondition(temp => temp.ProductId == productId);
        if (product == null)
        {
            return false;
        }

        bool response = await _productsRepository.DeleteProduct(productId);

        return response;
    }

    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        Product? product = await _productsRepository.GetProductByCondition(conditionExpression);

        if (product == null)
        {
            return null;
        }

        ProductResponse? response = _mapper?.Map<ProductResponse>(product);

        return response;
    }

    public async Task<List<ProductResponse?>> GetProducts()
    {
        IEnumerable<Product> products = await _productsRepository.GetProducts();

        IEnumerable<ProductResponse?> response = _mapper.Map<IEnumerable<ProductResponse>>(products);

        return response.ToList();
    }

    public async Task<List<ProductResponse>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        IEnumerable<Product> products = await _productsRepository.GetProductsByCondition(conditionExpression);

        IEnumerable<ProductResponse> response = _mapper.Map<IEnumerable<ProductResponse>>(products);

        return response.ToList();
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        Product product = await _productsRepository.GetProductByCondition(temp => temp.ProductId == productUpdateRequest.ProductId);

        if(product == null) { return null; }

        ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }

        Product mappedProduct = _mapper.Map<Product>(productUpdateRequest);

        Product? response = await _productsRepository.UpdateProduct(mappedProduct);

        ProductResponse? productResponse = _mapper.Map<ProductResponse>(response);

        return productResponse;
    }
}
