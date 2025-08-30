using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Result for cancelling a specific item in a sale.
/// </summary>
public class CancelItemResult
{
    /// <summary>
    /// Gets or sets the updated sale after cancelling the item.
    /// </summary>
    public Sale Sale { get; set; } = null!;

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
