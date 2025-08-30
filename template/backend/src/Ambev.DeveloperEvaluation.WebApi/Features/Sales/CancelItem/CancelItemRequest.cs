namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItem;

/// <summary>
/// Request model for cancelling a specific item in a sale.
/// </summary>
public class CancelItemRequest
{
    /// <summary>
    /// Gets or sets the sale ID.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the item ID to cancel.
    /// </summary>
    public Guid ItemId { get; set; }
}
