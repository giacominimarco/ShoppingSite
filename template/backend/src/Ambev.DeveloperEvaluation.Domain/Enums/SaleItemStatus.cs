namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Represents the status of a sale item.
/// </summary>
public enum SaleItemStatus
{
    /// <summary>
    /// The item is active and part of the sale.
    /// </summary>
    Active = 1,

    /// <summary>
    /// The item has been removed/cancelled from the sale.
    /// </summary>
    Removed = 2
}
