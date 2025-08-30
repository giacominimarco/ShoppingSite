using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// AutoMapper profile for CancelSale operations in WebApi.
/// </summary>
public class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the CancelSaleProfile class.
    /// </summary>
    public CancelSaleProfile()
    {
        CreateMap<Guid, CancelSaleCommand>()
            .ConstructUsing(id => new CancelSaleCommand(id));
        CreateMap<CancelSaleRequest, CancelSaleCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        CreateMap<CancelSaleResult, CancelSaleResponse>();
    }
}
