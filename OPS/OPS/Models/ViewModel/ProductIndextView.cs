using OPS.Models.OPSContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Models.ViewModel
{
    public class ProductIndextView
    {
        public List<Product> Products { get; set; }

        public ProductSearchParameter ProductSearchParameter { get; set; }
    }
}