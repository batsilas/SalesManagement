using SalesManagement.Core.Services.Options;
using SalesManagement.Data;
using SalesManagement.Models;
using SellersManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.Services
{
    public class SellerService : ISellerService
    {
        private SalesManagementDbContext context;

        public SellerService(SalesManagementDbContext contextByProgram)
        {
            context = contextByProgram;
        }

        public async Task<Seller> CreateSeller(CreateSellerOptions options)
        {
            if (options == null)
                return await Task.FromResult(new Seller());

            var Seller = new Seller()
            {
                Name = options.Name
            };

            context.Add(Seller);

            if (await context.SaveChangesAsync() > 0)
                return Seller;

            return await Task.FromResult(new Seller());
        }

        public async Task<bool> EditSeller(EditSellerOptions options)
        {

            if (options == null || options.Id == 0)
                return await Task.FromResult(false);

            var Seller = SearchSellers(new SearchSellerOptions()
            {
                Id = options.Id
            }).SingleOrDefault();

            if (Seller == null)
                return await Task.FromResult(false);

            if (!string.IsNullOrWhiteSpace(options.Name))
                Seller.Name = options.Name;

            if (await context.SaveChangesAsync() > 0)
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteSeller(DeleteSellerOptions options)
        {

            if (options == null || options.Id == 0)
                return await Task.FromResult(false);

            var Seller = SearchSellers(new SearchSellerOptions()
            {
                Id = options.Id
            }).SingleOrDefault();

            if (Seller == null)
                return await Task.FromResult(false);

            context.Seller.Remove(Seller);

            if (await context.SaveChangesAsync() > 0)
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        public IQueryable<Seller> SearchSellers(SearchSellerOptions options)
        {
            if (options == null || options.Id == 0)
                return context
                      .Set<Seller>()
                      .Where(s => s.Id == 0)
                      .AsQueryable();

            var query = context
                .Set<Seller>()
                .AsQueryable();

            if (options.Id != null && options.Id != 0)
                query = query.Where(m => m.Id == options.Id);

            if (!string.IsNullOrWhiteSpace(options.Name))
                query = query.Where(m => m.Name == options.Name);

            query = query.Take(500);

            return query;

        }

        public Seller GetSellerById(GetSellerByIdOptions options)
        {

            if (options == null || options.Id == 0)
                return new Seller();

            var Seller = context
                .Set<Seller>()
                .Where(m => m.Id == options.Id)
                .SingleOrDefault();

            if (Seller != null)
                return Seller;

            return new Seller();
        }

        public bool SellerExists(int? id)
        {
            if (id != null)
                return context.Seller.Any(e => e.Id == id);

            return false;
        }

        public Dictionary<int, decimal> CalculateCommissionAmounts(List<int> sellerIds) 
        {
            Dictionary<int, decimal> sellerCommissionAmount = new Dictionary<int, decimal>();
            decimal commission = 0.1m;
            var sales = context
                .Set<Sale>()
                .Where(s => sellerIds.Contains(s.SellerId))
                .ToList()
                .GroupBy(s => s.SellerId);

            foreach (var sellerSales in sales)
                sellerCommissionAmount.Add(sellerSales.Select(s => s.SellerId).FirstOrDefault(), 
                    Math.Round(sellerSales.Sum(s => s.Amount) * commission,2));

            return sellerCommissionAmount;
        }
    }
}
