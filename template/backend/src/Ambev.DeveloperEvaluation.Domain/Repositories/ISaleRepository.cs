using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale in the repository
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its sale number
    /// </summary>
    /// <param name="saleNumber">The sale number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all sales with optional filtering and pagination
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A collection of sales</returns>
    Task<IEnumerable<Sale>> GetAllAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves sales with filtering and pagination
    /// </summary>
    /// <param name="filters">The filters to apply</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A collection of sales matching the filters</returns>
    Task<IEnumerable<Sale>> GetFilteredAsync(SaleFilters filters, int page = 1, int size = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total count of sales matching the filters
    /// </summary>
    /// <param name="filters">The filters to apply</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The total count of sales matching the filters</returns>
    Task<int> GetFilteredCountAsync(SaleFilters filters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing sale in the repository
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a sale from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total count of sales
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The total count of sales</returns>
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Filters for sale queries
/// </summary>
public class SaleFilters
{
    /// <summary>
    /// Gets or sets the customer filter
    /// </summary>
    public string? Customer { get; set; }

    /// <summary>
    /// Gets or sets the branch filter
    /// </summary>
    public string? Branch { get; set; }

    /// <summary>
    /// Gets or sets the status filter
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the minimum date filter
    /// </summary>
    public DateTime? MinDate { get; set; }

    /// <summary>
    /// Gets or sets the maximum date filter
    /// </summary>
    public DateTime? MaxDate { get; set; }

    /// <summary>
    /// Gets or sets the minimum total amount filter
    /// </summary>
    public decimal? MinTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the maximum total amount filter
    /// </summary>
    public decimal? MaxTotalAmount { get; set; }
}
