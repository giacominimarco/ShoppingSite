using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItem;

/// <summary>
/// Response model for cancelling a specific item in a sale.
/// </summary>
public class CancelItemResponse
{
    /// <summary>
    /// Gets or sets the updated sale after cancelling the item.
    /// </summary>
    public GetSaleResponse Sale { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the cancelled item.
    /// </summary>
    public Guid CancelledItemId { get; set; }

    /// <summary>
    /// Gets or sets the message indicating the result of the operation.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the sale was automatically cancelled when the last item was removed.
    /// </summary>
    public bool WasAutomaticallyCancelled { get; set; }
}
