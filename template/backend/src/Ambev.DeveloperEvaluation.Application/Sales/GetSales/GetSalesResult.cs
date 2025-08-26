namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Result for GetSalesCommand.
/// </summary>
public class GetSalesResult
{
    /// <summary>
    /// Gets or sets the list of sales.
    /// </summary>
    public List<GetSaleSummaryResult> Sales { get; set; } = new List<GetSaleSummaryResult>();

    /// <summary>
    /// Gets or sets the total count of sales.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// Summary result for sale listing.
/// </summary>
public class GetSaleSummaryResult
{
    /// <summary>
    /// Gets or sets the sale ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sale date.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string Branch { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sale status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
