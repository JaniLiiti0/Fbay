using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fbay.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }

        public string BuyerName { get; set; }

        public string ItemNames { get; set; }

        public string ItemAmounts { get; set; }

        public string ItemImages { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
