using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for processing GetSalesCommand requests.
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesCommand, GetSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSalesHandler.
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSalesCommand request.
    /// </summary>
    /// <param name="command">The GetSales command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of sales with pagination</returns>
    public async Task<GetSalesResult> Handle(GetSalesCommand command, CancellationToken cancellationToken)
    {
        var validator = new GetSalesCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // For now, we'll use a simple implementation
        // In a real implementation, the repository would support filtering and ordering
        var sales = await _saleRepository.GetAllAsync(command.Page, command.Size, cancellationToken);
        var totalCount = await _saleRepository.GetCountAsync(cancellationToken);

        var result = new GetSalesResult
        {
            Sales = _mapper.Map<List<GetSaleSummaryResult>>(sales),
            TotalCount = totalCount,
            Page = command.Page,
            Size = command.Size,
            TotalPages = (int)Math.Ceiling((double)totalCount / command.Size)
        };

        return result;
    }
}
