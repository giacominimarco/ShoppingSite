using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Command for cancelling a sale.
/// </summary>
public class CancelSaleCommand : IRequest<CancelSaleResult>
{
    /// <summary>
    /// Gets or sets the sale ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the CancelSaleCommand class.
    /// </summary>
    public CancelSaleCommand() { }

    /// <summary>
    /// Initializes a new instance of the CancelSaleCommand class with an ID.
    /// </summary>
    /// <param name="id">The sale ID</param>
    public CancelSaleCommand(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Performs validation of the command.
    /// </summary>
    /// <returns>A validation result</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new CancelSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
