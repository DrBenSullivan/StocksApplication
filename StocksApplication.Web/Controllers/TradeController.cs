﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using StocksApplication.Core.Contracts.ServiceContracts;
using StocksApplication.Core.DTOs;
using StocksApplication.Core.Models;
using StocksApplication.Web.Filters;
using StocksApplication.Web.ViewModels;

namespace StocksApp.Controllers
{
	public class TradeController : Controller
    {
        #region private readonly fields
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly IConfiguration _configuration;
        #endregion

        #region constructors
        public TradeController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IStocksService stocksService, IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _stocksService = stocksService;
            _configuration = configuration;
        }
        #endregion

        [HttpGet]
        [Route("/")]
        [Route("Trade")]
        public async Task<IActionResult> Index(string? symbol)
        {
            string stockSymbol = symbol is not null
                ? symbol
                : _tradingOptions.DefaultStockSymbol
                ?? throw new Exception("Default Stock Symbol not found in configuration.");

            Dictionary<string, object> stockQuote = await _finnhubService.GetStockPriceQuote(stockSymbol)
                ?? throw new Exception("Failed to retrieve stockQuote from finnhubService.");

            Dictionary<string, object> companyProfile = await _finnhubService.GetCompanyProfile(stockSymbol)
                ?? throw new Exception("Failed to retrieve companyProfile from finnhubService.");

            var stockTradeViewModel = new StockTradeViewModel()
            {
                StockSymbol = companyProfile.GetValueOrDefault("ticker")?.ToString() ?? "Unknown",
                StockName = companyProfile.GetValueOrDefault("name")?.ToString() ?? "Unknown",
                Price = 0.00,
                Quantity = 0
            };

            ViewBag.FinnhubAPIKey = _configuration["FinnhubAPIKey"];
            return View(stockTradeViewModel);
        }

        [HttpPost]
        [Route("/BuyOrder")]
        [BuyOrderAndSellOrderActionFilter]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Orders", "Trade", request);
            }

            request.DateAndTimeOfOrder = DateTime.Now;
            BuyOrderResponse response = await _stocksService.CreateBuyOrder(request);
            return new RedirectToActionResult("Orders", "Trade", new { });
        }

        [HttpPost]
        [Route("/SellOrder")]
        [BuyOrderAndSellOrderActionFilter]
        public async Task<IActionResult> SellOrder(SellOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Orders", "Trade", request);
            }

            request.DateAndTimeOfOrder = DateTime.Now;
            SellOrderResponse response = await _stocksService.CreateSellOrder(request);
            return new RedirectToActionResult("Orders", "Trade", new { });
        }

        [HttpGet]
        [Route("/Orders")]
        public async Task<IActionResult> Orders()
        {
            try
            {
                List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();
                List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();
                var viewModel = new OrdersViewModel() { BuyOrders = buyOrders, SellOrders = sellOrders };
                return View(viewModel);
            }
            catch
            {
                throw new Exception("Error getting BuyOrders and SellOrders from database");
            }
            
        }

        [HttpGet]
        [Route("/OrdersPDF")]
        public async Task<IActionResult> OrdersPdf()
        {
            List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();
            var viewModel = new OrdersPdfViewModel(buyOrders, sellOrders);
            return new ViewAsPdf("OrdersPdf", viewModel, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}