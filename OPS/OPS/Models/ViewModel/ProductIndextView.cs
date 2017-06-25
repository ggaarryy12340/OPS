using OPS.Models.OPSContext;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Models.ViewModel
{
    public class ProductIndextView
    {
        public IPagedList<Product> Products { get; set; }

        public ProductSearchParameter ProductSearchParameter { get; set; }
    }
}