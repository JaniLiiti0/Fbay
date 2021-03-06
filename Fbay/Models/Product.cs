using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fbay.Models
{
    public class Product
    {
        //Same as the Listing_id
        public int Product_id { get; set; }

        //Name of the product
        public string Product_name { get; set; }

        //Amount to be bought
        public int Amount { get; set; }

        //Link to an image of the product
        public string Image { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }
    }
}
