using Microsoft.EntityFrameworkCore;
using SalesManagement.Core.Services.Options;
using SalesManagement.Data;
using SalesManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.Services
{
    public class SaleService : ISaleService
    {
        private SalesManagementDbContext context;

        public SaleService(SalesManagementDbContext contextByProgram)
        {
            context = contextByProgram;
        }
        public async Task<Sale> CreateSale(CreateSaleOptions options)
        {
            if (options == null)
                return null;

            var Sale = new Sale()
            {
                SellerId = options.SellerId,
                Amount = options.Amount,
                SaleDate = options.SaleDate
            };

            context.Add(Sale);

            if (await context.SaveChangesAsync() > 0)
                return Sale;

            return null;
        }

        public async Task<bool> EditSale(EditSaleOptions options)
        {

            if (options == null || options.Id == 0)
                return false;

            var Sale = SearchSales(new SearchSaleOptions()
            {
                Id = options.Id
            }).SingleOrDefault();

            if (Sale == null)
                return false;

            if (options.SellerId != 0)
                Sale.SellerId = options.SellerId;

            Sale.Amount = options.Amount;
            Sale.SaleDate = options.SaleDate;

            if (await context.SaveChangesAsync() > 0)
                return true;

            return false;
        }

        public async Task<bool> DeleteSale(DeleteSaleOptions options)
        {

            if (options == null || options.Id == 0)
                return false;

            var Sale = SearchSales(new SearchSaleOptions()
            {
                Id = options.Id
            }).SingleOrDefault();

            if (Sale == null)
                return false;

            context.Sale.Remove(Sale);

            if (await context.SaveChangesAsync() > 0)
                return true;

            return false;
        }

        public IQueryable<Sale> SearchSales(SearchSaleOptions options)
        {
            if (options == null)
                return null;

            var query = context
                .Set<Sale>()
                .Include(s => s.Seller)
                .AsQueryable();

            if (options.Id != 0 && options.Id != null)
                query = query.Where(m => m.Id == options.Id);

            if (options.SellerId != 0)
                query = query.Where(m => m.SellerId == options.SellerId);

            if (options.Amount != null)
                query = query.Where(m => m.Amount == options.Amount);

            if (options.SaleDateFrom != null)
                query = query.Where(m => m.SaleDate >= options.SaleDateFrom);

            if (options.SaleDateTo != null)
                query = query.Where(m => m.SaleDate <= options.SaleDateTo);

            query = query.Take(500);

            return query;

        }

        public Sale GetSaleById(GetSaleByIdOptions options)
        {

            if (options == null)
                return null;

            var Sale = context
                .Set<Sale>()
                .Include(s => s.Seller)
                .Where(m => m.Id == options.Id)
                .SingleOrDefault();

            if (Sale != null)
                return Sale;

            return null;
        }

        public bool SaleExists(int? id)
        {
            if (id != null)
                return context.Sale.Any(e => e.Id == id);

            return false;
        }

        public Dictionary<int, decimal> SaleAmountsPerMonth(List<Sale> sales)
        {
            Dictionary<int, decimal> salesPerMonth = new Dictionary<int, decimal>();

            foreach (var sale in sales.GroupBy(s => s.SaleDate.Month))
            {
                DateTime date = sale.Select(s => s.SaleDate).FirstOrDefault();
                List<Sale> thisMonthSales = sales.FindAll(m => m.SaleDate.Month == sale.Select(s => s.SaleDate).FirstOrDefault().Month).ToList();
                salesPerMonth.Add(date.Month, thisMonthSales != null ? thisMonthSales.Select(s => s.Amount).Sum() : 0);
            }
            return salesPerMonth;
        }
    }
}
