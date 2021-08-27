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

        [Required]
        public int Amount { get; set; }

        //Using images from the internet is obviously a bad way to implement them, should be replaced with storage
        public string Image { get; set; }

        public int InNumberOfCarts { get; set; }
    }
}
