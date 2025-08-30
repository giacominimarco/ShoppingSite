using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an individual item in a sale with product details, quantity, pricing, and discounts.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets the sale ID that this item belongs to.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets the product name (external identity with denormalized description).
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// Gets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets the discount percentage applied to this item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets the total amount for this item (including discount).
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets the current status of the sale item.
    /// </summary>
    public SaleItemStatus Status { get; set; }

    /// <summary>
    /// Gets the navigation property to the parent sale.
    /// </summary>
    public Sale Sale { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    public SaleItem()
    {
        Status = SaleItemStatus.Active;
    }

    /// <summary>
    /// Initializes a new instance of the SaleItem class with specified values.
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="product">The product name</param>
    /// <param name="quantity">The quantity</param>
    /// <param name="unitPrice">The unit price</param>
    /// <param name="discount">The discount percentage</param>
    /// <param name="totalAmount">The total amount</param>
    public SaleItem(Guid saleId, string product, int quantity, decimal unitPrice, decimal discount, decimal totalAmount)
    {
        SaleId = saleId;
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        TotalAmount = totalAmount;
        Status = SaleItemStatus.Active;
    }
}
