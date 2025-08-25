using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Validator for GetSalesRequest.
/// </summary>
public class GetSalesRequestValidator : AbstractValidator<GetSalesRequest>
{
    /// <summary>
    /// Initializes a new instance of the GetSalesRequestValidator class.
    /// </summary>
    public GetSalesRequestValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than zero");

        RuleFor(request => request.Size)
            .GreaterThan(0)
            .WithMessage("Size must be greater than zero")
            .LessThanOrEqualTo(100)
            .WithMessage("Size cannot exceed 100");

        RuleFor(request => request.MinDate)
            .LessThanOrEqualTo(request => request.MaxDate)
            .When(request => request.MinDate.HasValue && request.MaxDate.HasValue)
            .WithMessage("Minimum date must be less than or equal to maximum date");

        RuleFor(request => request.MinTotalAmount)
            .LessThanOrEqualTo(request => request.MaxTotalAmount)
            .When(request => request.MinTotalAmount.HasValue && request.MaxTotalAmount.HasValue)
            .WithMessage("Minimum total amount must be less than or equal to maximum total amount");
    }
}
