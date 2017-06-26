using OPS.Models.DAL;
using OPS.Models.OPSContext;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Repostiories
{
    public class ProductRepository
    {
        private readonly OPSContext _db = new OPSContext();

        public OPSContext db
        {
            get
            {
                return _db;
            }
        }

        public IPagedList<Product> GetAllProducts(int Page, int PageSize)
        {
            return db.Product.Include("PDCategory").OrderByDescending(x => x.CreateTime).ToPagedList(Page, PageSize);
        }

        public List<PDCategory> GetCategoryDropdownList()
        {
            return db.PDCategory.OrderBy(x => x.PDCategoryId).ToList();
        }

        public bool CreateProduct(Product model)
        {
            model.CreateTime = DateTime.Now;
            db.Product.Add(model);
            return db.SaveChanges() > 0;
        }

        public Product GetProductDetail(string id)
        {
            return db.Product.Find(id);
        }

        public bool ProductEdit(Product model)
        {
            var PDFromDb = db.Product.Find(model.ProductId);
            db.Entry(PDFromDb).CurrentValues.SetValues(model);
            return db.SaveChanges() > 0;
        }

        public bool ProductDelete(string id)
        {
            var Product = db.Product.Find(id);
            db.Product.Remove(Product);
            return db.SaveChanges() > 0;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}