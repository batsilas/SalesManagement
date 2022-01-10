using SalesManagement.Core.Services.Options;
using SalesManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.Services
{
    public interface ISaleService
    {
        Task<Sale> CreateSale(CreateSaleOptions options);
        Task<bool> EditSale(EditSaleOptions options);
        Task<bool> DeleteSale(DeleteSaleOptions options);
        IQueryable<Sale> SearchSales(SearchSaleOptions options);
        Sale GetSaleById(GetSaleByIdOptions options);
        bool SaleExists(int? id);
        Dictionary<int, decimal> SaleAmountsPerMonth(List<Sale> sales);
    }
}
