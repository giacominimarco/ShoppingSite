using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale is cancelled.
/// </summary>
public class SaleCancelledEvent
{
    /// <summary>
    /// Gets the sale that was cancelled.
    /// </summary>
    public Sale Sale { get; }

    /// <summary>
    /// Gets the timestamp when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Initializes a new instance of the SaleCancelledEvent class.
    /// </summary>
    /// <param name="sale">The sale that was cancelled</param>
    public SaleCancelledEvent(Sale sale)
    {
        Sale = sale;
        OccurredAt = DateTime.UtcNow;
    }
}
