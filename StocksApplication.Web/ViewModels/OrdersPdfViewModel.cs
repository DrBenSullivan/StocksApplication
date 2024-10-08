﻿using StocksApplication.Core.DTOs;

namespace StocksApplication.Web.ViewModels
{
    public class OrdersPdfViewModel
    {
        public List<OrderResponse> Orders { get; set; } = [];

        public OrdersPdfViewModel(List<BuyOrderResponse>? buyOrders, List<SellOrderResponse>? sellOrders)
        {
            if (buyOrders != null)
                Orders.AddRange(buyOrders);

            if (sellOrders != null)
                Orders.AddRange(sellOrders);

            Orders = Orders
                .OrderBy(o => o.DateAndTimeOfOrder)
                .ToList();
        }
    }
}

