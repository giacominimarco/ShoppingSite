namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Request model for retrieving a list of sales.
/// </summary>
public class GetSalesRequest
{
    /// <summary>
    /// Gets or sets the page number (default: 1).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size (default: 10).
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the customer filter.
    /// </summary>
    public string? Customer { get; set; }

    /// <summary>
    /// Gets or sets the branch filter.
    /// </summary>
    public string? Branch { get; set; }

    /// <summary>
    /// Gets or sets the status filter.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the minimum date filter.
    /// </summary>
    public DateTime? MinDate { get; set; }

    /// <summary>
    /// Gets or sets the maximum date filter.
    /// </summary>
    public DateTime? MaxDate { get; set; }

    /// <summary>
    /// Gets or sets the minimum total amount filter.
    /// </summary>
    public decimal? MinTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the maximum total amount filter.
    /// </summary>
    public decimal? MaxTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the ordering (e.g., "SaleDate desc, TotalAmount asc").
    /// </summary>
    public string? OrderBy { get; set; }
}
