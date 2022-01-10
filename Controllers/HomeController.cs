using Microsoft.AspNetCore.Mvc;
using SalesManagement.Core.Services.Options;
using SalesManagement.Models;
using SalesManagement.Services;
using SalesManagement.ViewModels;
using SellersManagement.Services;
using System.Diagnostics;

namespace SalesManagement.Controllers
{
    public class HomeController : Controller
    {
        private ISaleService _saleService;
        private ISellerService _sellerService;
        public HomeController(ISaleService saleService, ISellerService sellerService)
        {
            _saleService = saleService;
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            var numOfSellers = _sellerService
                .SearchSellers(new SearchSellerOptions())
                .Count();

            var totalSalesAmount = _saleService
                .SearchSales(new SearchSaleOptions())
                .Sum(s => s.Amount);

            HomeModel homeModel = new HomeModel() { 
                NumOfSellers = numOfSellers,
                TotalSalesAmount = totalSalesAmount
            };

            return View(homeModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}