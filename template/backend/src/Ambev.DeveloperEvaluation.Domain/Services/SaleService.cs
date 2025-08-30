using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Service implementation for Sale domain operations
/// </summary>
public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of the SaleService class
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    public SaleService(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Creates a new sale with business rules validation
    /// </summary>
    /// <param name="customer">The customer name</param>
    /// <param name="branch">The branch name</param>
    /// <param name="items">The sale items</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateSaleAsync(string customer, string branch, IEnumerable<SaleItemDto> items, CancellationToken cancellationToken = default)
    {
        var sale = new Sale
        {
            Customer = customer,
            Branch = branch
        };

        // Add items to the sale
        foreach (var itemDto in items)
        {
            sale.AddItem(itemDto.Product, itemDto.Quantity, itemDto.UnitPrice);
        }

        // Validate the sale
        var validationResult = sale.Validate();
        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(e => e.Error));
            throw new InvalidOperationException($"Sale validation failed: {errors}");
        }

        // Save the sale
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        // Publish event (in a real implementation, this would be handled by a domain event dispatcher)
        var saleCreatedEvent = new SaleCreatedEvent(createdSale);
        LogSaleEvent(saleCreatedEvent);

        return createdSale;
    }

    /// <summary>
    /// Cancels a sale
    /// </summary>
    /// <param name="saleId">The sale ID to cancel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cancelled sale</returns>
    public async Task<Sale> CancelSaleAsync(Guid saleId, CancellationToken cancellationToken = default)
    {
        var sale = await _saleRepository.GetByIdAsync(saleId, cancellationToken);
        if (sale == null)
            throw new InvalidOperationException($"Sale with ID {saleId} not found");

        sale.Cancel();
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Publish event
        var saleCancelledEvent = new SaleCancelledEvent(updatedSale);
        LogSaleEvent(saleCancelledEvent);

        return updatedSale;
    }

    /// <summary>
    /// Cancels a specific item in a sale
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="itemId">The item ID to cancel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale and information about whether it was automatically cancelled</returns>
    public async Task<(Sale Sale, bool WasAutomaticallyCancelled)> CancelItemAsync(Guid saleId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var sale = await _saleRepository.GetByIdAsync(saleId, cancellationToken);
        if (sale == null)
            throw new InvalidOperationException($"Sale with ID {saleId} not found");

        bool wasAutomaticallyCancelled = false;

        try
        {
            wasAutomaticallyCancelled = sale.CancelItem(itemId);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Item not found"))
        {
            // Verificar se o item realmente existe na venda
            var itemExists = sale.Items.Any(i => i.Id == itemId);
            if (!itemExists)
            {
                throw new InvalidOperationException($"Item with ID {itemId} not found in sale {saleId}. Available items: {string.Join(", ", sale.Items.Select(i => $"{i.Product} (ID: {i.Id})"))}");
            }
            throw; // Re-throw se for outro tipo de erro
        }

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Publish event
        var itemCancelledEvent = new ItemCancelledEvent(updatedSale, itemId);
        LogSaleEvent(itemCancelledEvent);

        // Se a venda foi cancelada automaticamente, publicar evento de cancelamento
        if (wasAutomaticallyCancelled)
        {
            var saleCancelledEvent = new SaleCancelledEvent(updatedSale);
            LogSaleEvent(saleCancelledEvent);
        }

        return (updatedSale, wasAutomaticallyCancelled);
    }

    /// <summary>
    /// Logs sale events (placeholder for event publishing)
    /// </summary>
    /// <param name="event">The event to log</param>
    private void LogSaleEvent(object @event)
    {
        // In a real implementation, this would publish to a message broker
        // For now, we'll just log to console
        Console.WriteLine($"Sale Event: {@event.GetType().Name} at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
    }
}
