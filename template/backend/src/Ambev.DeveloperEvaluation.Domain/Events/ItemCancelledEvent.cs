using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when an item is cancelled from a sale.
/// </summary>
public class ItemCancelledEvent
{
    /// <summary>
    /// Gets the sale that contains the cancelled item.
    /// </summary>
    public Sale Sale { get; }

    /// <summary>
    /// Gets the ID of the cancelled item.
    /// </summary>
    public Guid ItemId { get; }

    /// <summary>
    /// Gets the timestamp when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Initializes a new instance of the ItemCancelledEvent class.
    /// </summary>
    /// <param name="sale">The sale that contains the cancelled item</param>
    /// <param name="itemId">The ID of the cancelled item</param>
    public ItemCancelledEvent(Sale sale, Guid itemId)
    {
        Sale = sale;
        ItemId = itemId;
        OccurredAt = DateTime.UtcNow;
    }
}
