using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale in the system with all its details including items, customer, branch, and totals.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets the sale number (unique identifier for the sale).
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets the customer information (external identity with denormalized description).
    /// </summary>
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Gets the total sale amount.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets the branch where the sale was made (external identity with denormalized description).
    /// </summary>
    public string Branch { get; set; } = string.Empty;

    /// <summary>
    /// Gets the collection of sale items.
    /// </summary>
    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();

    /// <summary>
    /// Gets the current status of the sale.
    /// </summary>
    public SaleStatus Status { get; set; }

    /// <summary>
    /// Gets the date and time when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the sale.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        SaleDate = DateTime.UtcNow;
        Status = SaleStatus.Active;
        SaleNumber = GenerateSaleNumber();
    }

    /// <summary>
    /// Adds an item to the sale with business rules validation.
    /// </summary>
    /// <param name="product">The product name</param>
    /// <param name="quantity">The quantity to add</param>
    /// <param name="unitPrice">The unit price</param>
    /// <param name="discount">The discount percentage (0-100)</param>
    public void AddItem(string product, int quantity, decimal unitPrice, decimal discount = 0)
    {
        // Business rule: Cannot sell more than 20 identical items
        if (quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 identical items");

        // Business rule: No discount for quantities below 4 items
        if (quantity < 4 && discount > 0)
            throw new InvalidOperationException("No discount allowed for quantities below 4 items");

        // Business rule: 10% discount for 4+ items, 20% discount for 10-20 items
        decimal calculatedDiscount = CalculateDiscount(quantity);
        
        var item = new SaleItem
        {
            Product = product,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = calculatedDiscount,
            TotalAmount = CalculateItemTotal(quantity, unitPrice, calculatedDiscount),
            Status = SaleItemStatus.Active
        };

        Items.Add(item);
        RecalculateTotal();
    }

    /// <summary>
    /// Cancels the sale.
    /// </summary>
    public void Cancel()
    {
        if (Status == SaleStatus.Cancelled)
            throw new InvalidOperationException("Sale is already cancelled");

        Status = SaleStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels a specific item in the sale.
    /// </summary>
    /// <param name="itemId">The ID of the item to cancel</param>
    /// <returns>True if the sale was automatically cancelled (no more items), false otherwise</returns>
    public bool CancelItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new InvalidOperationException("Item not found in sale");

        // Marcar item como removido em vez de remover fisicamente
        item.Status = SaleItemStatus.Removed;
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;

        // Se não há mais itens ativos, cancelar a venda automaticamente
        if (GetActiveItemsCount() == 0)
        {
            Cancel();
            return true; // Venda foi cancelada automaticamente
        }

        return false; // Venda permanece ativa
    }

    /// <summary>
    /// Recalculates the total amount of the sale based on all items.
    /// </summary>
    public void RecalculateTotal()
    {
        TotalAmount = Items.Where(item => item.Status == SaleItemStatus.Active).Sum(item => item.TotalAmount);
    }

    /// <summary>
    /// Calculates the discount percentage based on quantity.
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <returns>The discount percentage</returns>
    private decimal CalculateDiscount(int quantity)
    {
        if (quantity >= 10 && quantity <= 20)
            return 20.0m; // 20% discount
        else if (quantity >= 4)
            return 10.0m; // 10% discount
        else
            return 0.0m; // No discount
    }

    /// <summary>
    /// Calculates the total amount for an item including discount.
    /// </summary>
    /// <param name="quantity">The quantity</param>
    /// <param name="unitPrice">The unit price</param>
    /// <param name="discount">The discount percentage</param>
    /// <returns>The total amount for the item</returns>
    private decimal CalculateItemTotal(int quantity, decimal unitPrice, decimal discount)
    {
        decimal subtotal = quantity * unitPrice;
        decimal discountAmount = subtotal * (discount / 100.0m);
        return subtotal - discountAmount;
    }

    /// <summary>
    /// Generates a unique sale number.
    /// </summary>
    /// <returns>A unique sale number</returns>
    private string GenerateSaleNumber()
    {
        var guid = Guid.NewGuid();
        var guidString = guid.ToString("N");
        if (guidString.Length >= 8)
        {
            return $"SALE-{DateTime.UtcNow:yyyyMMdd}-{guidString.Substring(0, 8).ToUpper()}";
        }
        return $"SALE-{DateTime.UtcNow:yyyyMMdd}-{guidString.ToUpper()}";
    }

    /// <summary>
    /// Gets the count of active items in the sale.
    /// </summary>
    /// <returns>The count of active items</returns>
    public int GetActiveItemsCount()
    {
        return Items.Count(item => item.Status == SaleItemStatus.Active);
    }

    /// <summary>
    /// Gets the count of removed items in the sale.
    /// </summary>
    /// <returns>The count of removed items</returns>
    public int GetRemovedItemsCount()
    {
        return Items.Count(item => item.Status == SaleItemStatus.Removed);
    }

    /// <summary>
    /// Performs validation of the sale entity.
    /// </summary>
    /// <returns>A validation result</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
