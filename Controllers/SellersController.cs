#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Core.Services.Options;
using SalesManagement.Data;
using SalesManagement.Models;
using SalesManagement.Services;
using SalesManagement.ViewModels;
using SellersManagement.Services;

namespace SalesManagement.Controllers
{
    public class SellersController : Controller
    {
        private ISaleService _saleService;
        private ISellerService _sellerService;
        public SellersController(ISellerService sellerService, ISaleService saleService)
        {
            _sellerService = sellerService;
            _saleService = saleService;
        }

        // GET: Sellers
        public async Task<IActionResult> Index()
        {
            List<SellersIndexModel> sellerModels = new List<SellersIndexModel>();
            var sellersList = await _sellerService
             .SearchSellers(new SearchSellerOptions())
             .ToListAsync();

            Dictionary<int,decimal> sellerCommissionAmounts = _sellerService
             .CalculateCommissionAmounts(sellersList.Select(s => s.Id).ToList());

            foreach (var seller in sellersList) {
                SellersIndexModel model = new SellersIndexModel()
                {
                    Seller = seller,
                    CommissionAmount = sellerCommissionAmounts.ContainsKey(seller.Id) ? sellerCommissionAmounts[seller.Id] : 0
                };
                sellerModels.Add(model);
            }
            return View(sellerModels);
        }

        // GET: Sellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var seller = await _sellerService
                .SearchSellers(new SearchSellerOptions()
                {
                    Id = id
                }).FirstOrDefaultAsync();

            if (seller == null)
                return NotFound();

            DateTime firstDay = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime lastDay = new DateTime(DateTime.Now.Year, 12, 31);

            var sales = await _saleService
             .SearchSales(new SearchSaleOptions()
             {
                 SellerId = seller.Id,
                 SaleDateFrom = firstDay,
                 SaleDateTo = lastDay
             }).ToListAsync();

            SellerDetailsModel sellerDetailsModel = new SellerDetailsModel()
            {
                Seller = seller,
                TotalSales = sales.Count(),
                SalesPerMonth = _saleService.SaleAmountsPerMonth(sales)
            };
            return View(sellerDetailsModel);
        }

        // GET: Sellers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sellers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Seller seller)
        {
            if (ModelState.IsValid)
            {
                await _sellerService.CreateSeller(new CreateSellerOptions()
                {
                    Name = seller.Name
                });
                return RedirectToAction(nameof(Index));
            }
            return View(seller);
        }

        // GET: Sellers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var seller = await _sellerService
                               .SearchSellers(new SearchSellerOptions()
                               {
                                   Id = id
                               }).FirstOrDefaultAsync();
            
            if (seller == null)
                return NotFound();

            return View(seller);
        }

        // POST: Sellers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Seller seller)
        {
            if (id != seller.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var updated = await _sellerService.EditSeller(new EditSellerOptions()
                    {
                        Id = seller.Id,
                        Name = seller.Name
                    });

                    if (!updated)
                        throw new DbUpdateConcurrencyException();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerExists(seller.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(seller);
        }

        // GET: Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var seller = await _sellerService
              .SearchSellers(new SearchSellerOptions()
              {
                  Id = id
              }).FirstOrDefaultAsync();

            if (seller == null)
                return NotFound();

            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == 0)
                return NotFound();
            try
            {
                if (SellerExists(id))
                {
                    var deleted = await _sellerService.DeleteSeller(new DeleteSellerOptions()
                    {
                        Id = id
                    });

                    if (!deleted)
                        throw new DbUpdateConcurrencyException();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SellerExists(id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SellerExists(int id)
        {
            return _sellerService.SellerExists(id);
        }
    }
}
