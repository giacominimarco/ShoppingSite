using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItem;

/// <summary>
/// AutoMapper profile for CancelItem operations in WebApi.
/// </summary>
public class CancelItemProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the CancelItemProfile class.
    /// </summary>
    public CancelItemProfile()
    {
        CreateMap<CancelItemRequest, CancelItemCommand>()
            .ForMember(dest => dest.SaleId, opt => opt.MapFrom(src => src.SaleId))
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId));
        
        CreateMap<CancelItemResult, CancelItemResponse>()
            .ForMember(dest => dest.WasAutomaticallyCancelled, opt => opt.MapFrom(src => src.WasAutomaticallyCancelled));
        
        // Mapeamento de Sale para GetSaleResponse
        CreateMap<Sale, GetSaleResponse>();
        CreateMap<SaleItem, GetSaleItemResponse>();
    }
}
