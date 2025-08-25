using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the Sale entity.
/// </summary>
public class SaleTests
{
    [Fact]
    public void CreateSale_ShouldGenerateSaleNumber()
    {
        // Arrange & Act
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };

        // Assert
        Assert.NotNull(sale.SaleNumber);
        Assert.StartsWith("SALE-", sale.SaleNumber);
        Assert.Equal(SaleStatus.Active, sale.Status);
        Assert.Equal(DateTime.UtcNow.Date, sale.SaleDate.Date);
    }

    [Fact]
    public void AddItem_WithValidQuantity_ShouldAddItem()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };

        // Act
        sale.AddItem("Beer", 5, 10.00m);

        // Assert
        Assert.Single(sale.Items);
        var item = sale.Items.First();
        Assert.Equal("Beer", item.Product);
        Assert.Equal(5, item.Quantity);
        Assert.Equal(10.00m, item.UnitPrice);
        Assert.Equal(10.0m, item.Discount); // 10% discount for 5 items
        Assert.Equal(45.00m, item.TotalAmount); // 50 - 10% = 45
    }

    [Fact]
    public void AddItem_WithQuantityBelow4_ShouldNotApplyDiscount()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };

        // Act
        sale.AddItem("Beer", 3, 10.00m);

        // Assert
        var item = sale.Items.First();
        Assert.Equal(0.0m, item.Discount);
        Assert.Equal(30.00m, item.TotalAmount); // No discount
    }

    [Fact]
    public void AddItem_WithQuantity10To20_ShouldApply20PercentDiscount()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };

        // Act
        sale.AddItem("Beer", 12, 10.00m);

        // Assert
        var item = sale.Items.First();
        Assert.Equal(20.0m, item.Discount);
        Assert.Equal(96.00m, item.TotalAmount); // 120 - 20% = 96
    }

    [Fact]
    public void AddItem_WithQuantityAbove20_ShouldThrowException()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => 
            sale.AddItem("Beer", 21, 10.00m));
        
        Assert.Equal("Cannot sell more than 20 identical items", exception.Message);
    }

    [Fact]
    public void AddItem_WithQuantityBelow4AndDiscount_ShouldThrowException()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => 
            sale.AddItem("Beer", 3, 10.00m, 10.0m));
        
        Assert.Equal("No discount allowed for quantities below 4 items", exception.Message);
    }

    [Fact]
    public void CancelSale_ShouldChangeStatusToCancelled()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };

        // Act
        sale.Cancel();

        // Assert
        Assert.Equal(SaleStatus.Cancelled, sale.Status);
        Assert.NotNull(sale.UpdatedAt);
    }

    [Fact]
    public void CancelSale_WhenAlreadyCancelled_ShouldThrowException()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };
        sale.Cancel();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => sale.Cancel());
        Assert.Equal("Sale is already cancelled", exception.Message);
    }

    [Fact]
    public void CancelItem_ShouldRemoveItemAndRecalculateTotal()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };
        sale.AddItem("Beer", 5, 10.00m);
        sale.AddItem("Wine", 3, 20.00m);
        
        var itemToCancel = sale.Items.First();

        // Act
        sale.CancelItem(itemToCancel.Id);

        // Assert
        Assert.Single(sale.Items);
        Assert.Equal(60.00m, sale.TotalAmount); // Only wine item remains
    }

    [Fact]
    public void CancelItem_WithInvalidItemId_ShouldThrowException()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };
        sale.AddItem("Beer", 5, 10.00m);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => 
            sale.CancelItem(Guid.NewGuid()));
        
        Assert.Equal("Item not found in sale", exception.Message);
    }

    [Fact]
    public void Validate_WithValidSale_ShouldReturnValidResult()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };
        sale.AddItem("Beer", 5, 10.00m);

        // Act
        var result = sale.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WithInvalidSale_ShouldReturnInvalidResult()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "", // Invalid: empty customer
            Branch = "Downtown Store"
        };
        sale.AddItem("Beer", 5, 10.00m);

        // Act
        var result = sale.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}
