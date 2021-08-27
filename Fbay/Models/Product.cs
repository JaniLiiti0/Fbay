using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fbay.Models
{
    public class Product
    {
        //Same as Listing_id
        public int Product_id { get; set; }

        public string Product_name { get; set; }

        public int Amount { get; set; }

        //Useful or not?
        public string Image { get; set; }

        //Implement later
        //public int Price { get; set; }
    }
}
