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
    public void CancelItem_ShouldMarkItemAsRemovedAndRecalculateTotal()
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
        var initialTotal = sale.TotalAmount;

        // Act
        sale.CancelItem(itemToCancel.Id);

        // Assert
        Assert.Equal(2, sale.Items.Count); // Item ainda está na lista
        Assert.Equal(SaleItemStatus.Removed, itemToCancel.Status); // Mas marcado como removido
        Assert.Equal(60.00m, sale.TotalAmount); // Total recalculado sem o item removido
        Assert.True(sale.TotalAmount < initialTotal); // Total diminuiu
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

    [Fact]
    public void CancelItem_ShouldSetItemStatusToRemoved()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };
        sale.AddItem("Beer", 5, 10.00m);
        sale.AddItem("Wine", 3, 20.00m); // Adicionar segundo item para testar
        var itemToCancel = sale.Items.First();

        // Act
        sale.CancelItem(itemToCancel.Id);

        // Assert
        Assert.Equal(SaleItemStatus.Removed, itemToCancel.Status);
        Assert.Equal(SaleItemStatus.Active, sale.Items.Last().Status); // Outros itens permanecem ativos
    }

    [Fact]
    public void CancelItem_WhenLastActiveItem_ShouldAutomaticallyCancelSale()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };
        sale.AddItem("Beer", 5, 10.00m); // Apenas um item

        // Act
        sale.CancelItem(sale.Items.First().Id);

        // Assert
        Assert.Equal(SaleStatus.Cancelled, sale.Status);
        Assert.NotNull(sale.UpdatedAt);
    }

    [Fact]
    public void CancelItem_WhenMultipleItemsExist_ShouldNotCancelSale()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };
        sale.AddItem("Beer", 5, 10.00m);
        sale.AddItem("Wine", 3, 20.00m);

        // Act
        sale.CancelItem(sale.Items.First().Id);

        // Assert
        Assert.Equal(SaleStatus.Active, sale.Status);
        Assert.Equal(1, sale.GetActiveItemsCount());
        Assert.Equal(1, sale.GetRemovedItemsCount());
    }

    [Fact]
    public void GetActiveItemsCount_ShouldReturnOnlyActiveItems()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };
        sale.AddItem("Beer", 5, 10.00m);
        sale.AddItem("Wine", 3, 20.00m);
        sale.CancelItem(sale.Items.First().Id);

        // Act
        var activeCount = sale.GetActiveItemsCount();
        var removedCount = sale.GetRemovedItemsCount();

        // Assert
        Assert.Equal(1, activeCount);
        Assert.Equal(1, removedCount);
        Assert.Equal(2, sale.Items.Count); // Total de itens permanece
    }

    [Fact]
    public void RecalculateTotal_ShouldOnlySumActiveItems()
    {
        // Arrange
        var sale = new Sale
        {
            Customer = "John Doe",
            Branch = "Downtown Store"
        };
        sale.AddItem("Beer", 5, 10.00m); // 45.00 (com desconto)
        sale.AddItem("Wine", 3, 20.00m); // 60.00 (sem desconto)
        var initialTotal = sale.TotalAmount; // 105.00

        // Act
        sale.CancelItem(sale.Items.First().Id); // Remove Beer (45.00)
        sale.RecalculateTotal();

        // Assert
        Assert.Equal(60.00m, sale.TotalAmount); // Apenas Wine
        Assert.True(sale.TotalAmount < initialTotal);
    }

    [Fact]
    public void AddItem_ShouldSetStatusToActive()
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
        var item = sale.Items.First();
        Assert.Equal(SaleItemStatus.Active, item.Status);
    }
}
