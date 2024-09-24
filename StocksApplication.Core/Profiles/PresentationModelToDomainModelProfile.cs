using AutoMapper;
using StocksApplication.Core.DTOs;
using StocksApplication.Core.Models;

namespace StocksApplication.Core.Profiles
{
    public class PresentationModelToDomainModelProfile : Profile
    {
        public PresentationModelToDomainModelProfile()
        {
            CreateMap<BuyOrderRequest, BuyOrder>()
                .ForMember(dest => dest.TradeAmount, opt => opt.MapFrom(
                    src => src.Quantity * src.Price));
            CreateMap<SellOrderRequest, SellOrder>()
                .ForMember(dest => dest.TradeAmount, opt => opt.MapFrom(
                    src => src.Quantity * src.Price));
        }
    }
}
