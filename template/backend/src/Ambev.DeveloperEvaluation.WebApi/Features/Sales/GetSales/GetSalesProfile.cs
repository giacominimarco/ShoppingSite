using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// AutoMapper profile for GetSales operations in WebApi.
/// </summary>
public class GetSalesProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the GetSalesProfile class.
    /// </summary>
    public GetSalesProfile()
    {
        CreateMap<GetSalesRequest, GetSalesCommand>();
        CreateMap<GetSalesResult, GetSalesResponse>();
        CreateMap<GetSaleSummaryResult, GetSaleSummaryResponse>();
    }
}
