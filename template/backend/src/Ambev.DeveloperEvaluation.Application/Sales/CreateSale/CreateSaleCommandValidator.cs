using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator class.
    /// </summary>
    public CreateSaleCommandValidator()
    {
        RuleFor(command => command.Customer)
            .NotEmpty()
            .WithMessage("Customer is required")
            .MaximumLength(200)
            .WithMessage("Customer name cannot exceed 200 characters");

        RuleFor(command => command.Branch)
            .NotEmpty()
            .WithMessage("Branch is required")
            .MaximumLength(200)
            .WithMessage("Branch name cannot exceed 200 characters");

        RuleFor(command => command.Items)
            .NotEmpty()
            .WithMessage("At least one item is required");

        RuleForEach(command => command.Items)
            .SetValidator(new CreateSaleItemDtoValidator());
    }
}

/// <summary>
/// Validator for CreateSaleItemDto.
/// </summary>
public class CreateSaleItemDtoValidator : AbstractValidator<CreateSaleItemDto>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleItemDtoValidator class.
    /// </summary>
    public CreateSaleItemDtoValidator()
    {
        RuleFor(item => item.Product)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(200)
            .WithMessage("Product name cannot exceed 200 characters");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(20)
            .WithMessage("Quantity cannot exceed 20 items");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero");
    }
}
