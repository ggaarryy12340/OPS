using OPS.Models;
using OPS.Models.OPSContext;
using OPS.Repostiories;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository = null;

        public ProductRepository Repository
        {
            get { return _repository ?? new ProductRepository(); }
        }

        public IPagedList<Product> GetProductList(ProductSearchParameter searchpara, int Page, int PageSize)
        {
            return Repository.GetProductList(searchpara, Page, PageSize);
        }

        public List<PDCategory> GetCategoryDropdownList()
        {
            return Repository.GetCategoryDropdownList();
        }

        public bool CreateProduct(Product model)
        {
            return Repository.CreateProduct(model);
        }

        public Product GetProductDetail(string id)
        {
            return Repository.GetProductDetail(id);
        }

        public bool ProductEdit(Product model)
        {
            return Repository.ProductEdit(model);
        }

        public bool ProductDelete(string id)
        {
            return Repository.ProductDelete(id);
        }

        public void Dispose()
        {
            Repository.Dispose();
        }
    }
}