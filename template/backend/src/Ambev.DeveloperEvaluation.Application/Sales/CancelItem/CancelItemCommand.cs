using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Command for cancelling a specific item in a sale.
/// </summary>
public class CancelItemCommand : IRequest<CancelItemResult>
{
    /// <summary>
    /// Gets or sets the sale ID.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the item ID to cancel.
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Initializes a new instance of the CancelItemCommand class.
    /// </summary>
    public CancelItemCommand() { }

    /// <summary>
    /// Initializes a new instance of the CancelItemCommand class with sale and item IDs.
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="itemId">The item ID to cancel</param>
    public CancelItemCommand(Guid saleId, Guid itemId)
    {
        SaleId = saleId;
        ItemId = itemId;
    }

    /// <summary>
    /// Performs validation of the command.
    /// </summary>
    /// <returns>A validation result</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new CancelItemCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
