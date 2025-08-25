using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Validator for GetSalesCommand.
/// </summary>
public class GetSalesCommandValidator : AbstractValidator<GetSalesCommand>
{
    /// <summary>
    /// Initializes a new instance of the GetSalesCommandValidator class.
    /// </summary>
    public GetSalesCommandValidator()
    {
        RuleFor(command => command.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than zero");

        RuleFor(command => command.Size)
            .GreaterThan(0)
            .WithMessage("Size must be greater than zero")
            .LessThanOrEqualTo(100)
            .WithMessage("Size cannot exceed 100");

        RuleFor(command => command.MinDate)
            .LessThanOrEqualTo(command => command.MaxDate)
            .When(command => command.MinDate.HasValue && command.MaxDate.HasValue)
            .WithMessage("Minimum date must be less than or equal to maximum date");

        RuleFor(command => command.MinTotalAmount)
            .LessThanOrEqualTo(command => command.MaxTotalAmount)
            .When(command => command.MinTotalAmount.HasValue && command.MaxTotalAmount.HasValue)
            .WithMessage("Minimum total amount must be less than or equal to maximum total amount");
    }
}
