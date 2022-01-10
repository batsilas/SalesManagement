using SalesManagement.Core.Services.Options;
using SalesManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellersManagement.Services
{
    public interface ISellerService
    {    
        Task<Seller> CreateSeller(CreateSellerOptions options);
        Task<bool> EditSeller(EditSellerOptions options);
        Task<bool> DeleteSeller(DeleteSellerOptions options);
        IQueryable<Seller> SearchSellers(SearchSellerOptions options);
        Seller GetSellerById(GetSellerByIdOptions options);
        bool SellerExists(int? id);
        Dictionary<int, decimal> CalculateCommissionAmounts(List<int> sellerIds);
    }
}
