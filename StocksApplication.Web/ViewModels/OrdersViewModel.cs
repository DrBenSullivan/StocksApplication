using StocksApplication.Core.DTOs;

namespace StocksApplication.Web.ViewModels
{
    public class OrdersViewModel
    {
        public List<BuyOrderResponse> BuyOrders { get; set; }
        public List<SellOrderResponse> SellOrders { get; set; }
    }
}
