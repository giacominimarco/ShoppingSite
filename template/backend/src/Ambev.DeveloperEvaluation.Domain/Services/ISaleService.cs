using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Service interface for Sale domain operations
/// </summary>
public interface ISaleService
{
    /// <summary>
    /// Creates a new sale with business rules validation
    /// </summary>
    /// <param name="customer">The customer name</param>
    /// <param name="branch">The branch name</param>
    /// <param name="items">The sale items</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    Task<Sale> CreateSaleAsync(string customer, string branch, IEnumerable<SaleItemDto> items, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a sale
    /// </summary>
    /// <param name="saleId">The sale ID to cancel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cancelled sale</returns>
    Task<Sale> CancelSaleAsync(Guid saleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a specific item in a sale
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="itemId">The item ID to cancel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    Task<Sale> CancelItemAsync(Guid saleId, Guid itemId, CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO for sale item creation
/// </summary>
public class SaleItemDto
{
    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price
    /// </summary>
    public decimal UnitPrice { get; set; }
}
