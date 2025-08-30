using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CancelItemHandlerTests
{
    private readonly ISaleService _saleServiceMock;
    private readonly IMapper _mapperMock;
    private readonly CancelItemHandler _handler;

    public CancelItemHandlerTests()
    {
        _saleServiceMock = Substitute.For<ISaleService>();
        _mapperMock = Substitute.For<IMapper>();
        _handler = new CancelItemHandler(_saleServiceMock, _mapperMock);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCancelItemAndReturnSuccess()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var request = new CancelItemCommand { SaleId = saleId, ItemId = itemId };

        var sale = new Sale
        {
            Id = saleId,
            Customer = "John Doe",
            Branch = "Downtown Store",
            Status = SaleStatus.Active
        };
        sale.AddItem("Beer", 5, 10.00m);
        sale.AddItem("Wine", 3, 20.00m);

        var itemToCancel = sale.Items.First();
        itemToCancel.Id = itemId;

        _saleServiceMock.CancelItemAsync(saleId, itemId, Arg.Any<CancellationToken>())
            .Returns((sale, false)); // Não foi cancelada automaticamente

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(saleId, result.Sale.Id);
        Assert.Equal(itemId, result.CancelledItemId);
        Assert.False(result.WasAutomaticallyCancelled);

        _saleServiceMock.Received(1).CancelItemAsync(saleId, itemId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenLastItemCancelled_ShouldReturnAutomaticCancellation()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var request = new CancelItemCommand { SaleId = saleId, ItemId = itemId };

        var sale = new Sale
        {
            Id = saleId,
            Customer = "John Doe",
            Branch = "Downtown Store",
            Status = SaleStatus.Cancelled // Venda foi cancelada automaticamente
        };
        sale.AddItem("Beer", 5, 10.00m);
        var item = sale.Items.First();
        item.Id = itemId;

        _saleServiceMock.CancelItemAsync(saleId, itemId, Arg.Any<CancellationToken>())
            .Returns((sale, true)); // Foi cancelada automaticamente

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.WasAutomaticallyCancelled);
        Assert.Equal(SaleStatus.Cancelled, result.Sale.Status);
    }

    [Fact]
    public async Task Handle_WithInvalidSaleId_ShouldThrowException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var request = new CancelItemCommand { SaleId = saleId, ItemId = itemId };

        _saleServiceMock.CancelItemAsync(saleId, itemId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<(Sale, bool)>(new InvalidOperationException("Venda não encontrada")));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(request, CancellationToken.None)
        );

        Assert.Contains("Venda não encontrada", exception.Message);
        _saleServiceMock.Received(1).CancelItemAsync(saleId, itemId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenSaleServiceThrowsException_ShouldThrowException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var request = new CancelItemCommand { SaleId = saleId, ItemId = itemId };

        var sale = new Sale
        {
            Id = saleId,
            Customer = "John Doe",
            Branch = "Downtown Store",
            Status = SaleStatus.Active
        };

        _saleServiceMock.CancelItemAsync(saleId, itemId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<(Sale, bool)>(new InvalidOperationException("Item não encontrado na venda")));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(request, CancellationToken.None)
        );

        Assert.Contains("Item não encontrado na venda", exception.Message);
        _saleServiceMock.Received(1).CancelItemAsync(saleId, itemId, Arg.Any<CancellationToken>());
    }
}
