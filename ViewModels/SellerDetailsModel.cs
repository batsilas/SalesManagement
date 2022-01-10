using SalesManagement.Models;

namespace SalesManagement.ViewModels
{
    public class SellerDetailsModel
    {
        public SellerDetailsModel()
        {
            SalesPerMonth = new Dictionary<int, decimal>();
        }
        public Seller Seller { get; set; }
        public int TotalSales { get; set; }
        public Dictionary<int, decimal> SalesPerMonth { get; set; }
    }
}

