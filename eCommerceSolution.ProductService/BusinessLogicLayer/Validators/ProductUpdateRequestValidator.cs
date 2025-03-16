﻿
using eCommerce.BusinessLogicLayer.DTO;
using FluentValidation;

namespace eCommerce.BusinessLogicLayer.Validators;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(temp => temp.ProductId)
            .NotEmpty().WithMessage("Product ID can't be blank");

        RuleFor(temp => temp.ProductName)
            .NotEmpty().WithMessage("Product Name can't be blank");

        RuleFor(temp => temp.Category)
            .IsInEnum().WithMessage("Category can't be blank");

        RuleFor(temp => temp.UnitPrice)
            .InclusiveBetween(0, double.MaxValue).WithMessage($"Unit Price should be between 0 to {double.MaxValue}");

        RuleFor(temp => temp.QuantityInStock)
            .InclusiveBetween(0, int.MaxValue).WithMessage($"Quantity in Stock should be between 0 to {int.MaxValue}");
    }
}
