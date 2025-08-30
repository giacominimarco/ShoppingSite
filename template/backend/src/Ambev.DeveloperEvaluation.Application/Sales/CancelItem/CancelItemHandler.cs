using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Handler for processing CancelItemCommand requests.
/// </summary>
public class CancelItemHandler : IRequestHandler<CancelItemCommand, CancelItemResult>
{
    private readonly ISaleService _saleService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CancelItemHandler.
    /// </summary>
    /// <param name="saleService">The sale service</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CancelItemHandler(ISaleService saleService, IMapper mapper)
    {
        _saleService = saleService;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CancelItemCommand request.
    /// </summary>
    /// <param name="command">The CancelItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of cancelling the item</returns>
    public async Task<CancelItemResult> Handle(CancelItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelItemCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Cancel the item using the domain service
        var (sale, wasAutomaticallyCancelled) = await _saleService.CancelItemAsync(command.SaleId, command.ItemId, cancellationToken);

        // Create result
        var result = new CancelItemResult
        {
            Sale = sale,
            CancelledItemId = command.ItemId,
            Message = wasAutomaticallyCancelled 
                ? "Item cancelled successfully. Sale was automatically cancelled as it had no more items." 
                : "Item cancelled successfully",
            WasAutomaticallyCancelled = wasAutomaticallyCancelled
        };

        return result;
    }
}
