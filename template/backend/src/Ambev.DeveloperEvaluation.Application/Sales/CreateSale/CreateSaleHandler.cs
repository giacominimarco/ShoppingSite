using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests.
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleService _saleService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler.
    /// </summary>
    /// <param name="saleService">The sale service</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CreateSaleHandler(ISaleService saleService, IMapper mapper)
    {
        _saleService = saleService;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request.
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Convert command items to domain DTOs
        var domainItems = command.Items.Select(item => new Domain.Services.SaleItemDto
        {
            Product = item.Product,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        });

        // Create the sale using the domain service
        var sale = await _saleService.CreateSaleAsync(command.Customer, command.Branch, domainItems, cancellationToken);

        // Map to result
        var result = _mapper.Map<CreateSaleResult>(sale);
        return result;
    }
}
