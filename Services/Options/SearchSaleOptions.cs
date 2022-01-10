using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.Core.Services.Options
{
    public class SearchSaleOptions
    {
        public int? Id { get; set; }
        public int SellerId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? SaleDateFrom { get; set; }
        public DateTime? SaleDateTo { get; set; }

    }
}
