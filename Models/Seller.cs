using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.Models
{
    public class Seller
    {
        public int Id { get; set; }
        [Required, StringLength(150, MinimumLength = 1)]
        public string Name { get; set; }
        public List<Sale> Sales { get; set; }
        public Seller()
        {
            Sales = new List<Sale>();
        }
    }
}
