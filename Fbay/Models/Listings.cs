using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fbay.Models
{
    public class Listings
    {
        //Structure of the listings table in the Fbay database
        [Display(Name = "Listing ID")]
        [Key]
        public int Listing_id { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public int User_id { get; set; }
        
        [Required]
        public string Item { get; set; }

        //Amount of stock to be sold
        [Required]
        public int Amount { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        //Link to an image of the product
        //Using images from the internet is obviously a bad way to implement them, should be replaced with storage
        public string Image { get; set; }

        //The amount of shopping carts that have this product
        //Used for making sure all orders have been checked out before deleting listing when stock is 0
        public int InNumberOfCarts { get; set; }
    }
}
