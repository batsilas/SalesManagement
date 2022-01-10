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
using SellersManagement.Services;

namespace SalesManagement.Controllers
{
    public class SalesController : Controller
    {
        private ISaleService _saleService;
        private ISellerService _sellerService;
        public SalesController(ISaleService saleService, ISellerService sellerService)
        {
            _saleService = saleService;
            _sellerService = sellerService;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var sales = await _saleService
                .SearchSales(new SearchSaleOptions())
                .ToListAsync();

            return View(sales);
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var sale = await _saleService
              .SearchSales(new SearchSaleOptions()
              {
                  Id = id
              }).FirstOrDefaultAsync();

            if (sale == null)
                return NotFound();

            return View(sale);
        }

        // GET: Sales/Create
        public async Task<IActionResult> Create()
        {
            var sellersList = await _sellerService
               .SearchSellers(new SearchSellerOptions())
               .ToListAsync();

            ViewData["Sellers"] = new SelectList(sellersList, "Id", "Name");
            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SellerId,Amount,SaleDate")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!_sellerService.SellerExists(sale.SellerId))
                        return NotFound();

                    await _saleService.CreateSale(new CreateSaleOptions()
                    {
                        SellerId = sale.SellerId,
                        Amount = sale.Amount,
                        SaleDate = sale.SaleDate
                    });
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            var sellersList = await _sellerService
               .SearchSellers(new SearchSellerOptions())
               .ToListAsync();

            ViewData["Sellers"] = new SelectList(sellersList, "Id", "Name");
            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var sale = await _saleService
                           .SearchSales(new SearchSaleOptions()
                           {
                               Id = id
                           }).FirstOrDefaultAsync();

            if (sale == null)
                return NotFound();

            var sellersList = await _sellerService
            .SearchSellers(new SearchSellerOptions())
            .ToListAsync();

            ViewData["Sellers"] = new SelectList(sellersList, "Id", "Name", sale.SellerId);
            return View(sale);
        }

        // POST: Sales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SellerId,Amount,SaleDate")] Sale sale)
        {
            if (id != sale.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (!_sellerService.SellerExists(sale.SellerId))
                        return NotFound();

                    var updated = await _saleService.EditSale(new EditSaleOptions()
                    {
                        Id = sale.Id,
                        SellerId = sale.SellerId,
                        Amount = sale.Amount,
                        SaleDate = sale.SaleDate
                    });

                    if (!updated)
                        throw new DbUpdateConcurrencyException();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var sellersList = await _sellerService
                        .SearchSellers(new SearchSellerOptions())
                        .ToListAsync();

            ViewData["Sellers"] = new SelectList(sellersList, "Id", "Name", sale.SellerId); 
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var sale = await _saleService
                 .SearchSales(new SearchSaleOptions()
                 {
                     Id = id
                 }).FirstOrDefaultAsync();

            if (sale == null)
                return NotFound();

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == 0)
                return NotFound();
            try
            {
                if (SaleExists(id))
                {
                    var deleted = await _saleService.DeleteSale(new DeleteSaleOptions()
                    {
                        Id = id
                    });

                    if (!deleted)
                        throw new DbUpdateConcurrencyException();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _saleService.SaleExists(id);
        }
    }
}
