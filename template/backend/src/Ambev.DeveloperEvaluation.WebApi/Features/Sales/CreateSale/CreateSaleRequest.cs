namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Request model for creating a sale.
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string Branch { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sale items.
    /// </summary>
    public List<CreateSaleItemRequest> Items { get; set; } = new List<CreateSaleItemRequest>();
}

/// <summary>
/// Request model for sale item creation.
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price.
    /// </summary>
    public decimal UnitPrice { get; set; }
}
