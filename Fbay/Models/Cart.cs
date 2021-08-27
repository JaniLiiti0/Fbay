using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fbay.Models
{
    public class Cart
    {
        public List<Product> products { get; set; }
    }
}
