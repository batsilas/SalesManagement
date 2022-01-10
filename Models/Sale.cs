using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public Seller? Seller { get; set; }
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Amount { get; set; }
        [Display(Name = "Sale Date")]
        public DateTime SaleDate { get; set; }
    }
}
