using StocksApplication.Core.Models;

namespace StocksApplication.Core.Contracts.RepositoryContracts
{
    public interface IStocksRepository
    {
        public Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);
        public Task<SellOrder> CreateSellOrder(SellOrder sellOrder);
        public Task<List<BuyOrder>> GetBuyOrders();
        public Task<List<SellOrder>> GetSellOrders();
    }
}
