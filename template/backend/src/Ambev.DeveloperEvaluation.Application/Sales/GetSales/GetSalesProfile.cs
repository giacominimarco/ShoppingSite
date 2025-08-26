using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// AutoMapper profile for GetSales operations.
/// </summary>
public class GetSalesProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the GetSalesProfile class.
    /// </summary>
    public GetSalesProfile()
    {
        CreateMap<Sale, GetSaleSummaryResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
