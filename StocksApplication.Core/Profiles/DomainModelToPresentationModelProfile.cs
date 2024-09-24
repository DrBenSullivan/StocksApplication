using AutoMapper;
using StocksApplication.Core.DTOs;
using StocksApplication.Core.Models;

namespace StocksApplication.Core.Profiles
{
    public class DomainModelToPresentationModelProfile : Profile
    {
        public DomainModelToPresentationModelProfile()
        {
            CreateMap<BuyOrder, BuyOrderResponse>();
            CreateMap<SellOrder, SellOrderResponse>();
        }
    }
}
