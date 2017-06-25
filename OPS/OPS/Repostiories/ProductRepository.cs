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
        public IPagedList<Product> GetAllProducts(int Page, int PageSize)
        {
            using (OPSContext db = new OPSContext())
            {
                return db.Product.Include("PDCategory").OrderByDescending(x => x.CreateTime).ToPagedList(Page, PageSize);
            }
        }

        public List<PDCategory> GetCategoryDropdownList()
        {
            using (OPSContext db = new OPSContext())
            {
                return db.PDCategory.OrderBy(x => x.PDCategoryId).ToList();
            }
        }

        public bool CreateProduct(Product model)
        {
            using (OPSContext db = new OPSContext())
            {
                model.CreateTime = DateTime.Now;
                db.Product.Add(model);
                return db.SaveChanges() > 0;
            }
        }

        public Product GetProductDetail(string id)
        {
            using (OPSContext db = new OPSContext())
            {
                return db.Product.Find(id);
            }
        }

        public bool ProductEdit(Product model)
        {
            using (OPSContext db = new OPSContext())
            {
                var PDFromDb = db.Product.Find(model.ProductId);
                db.Entry(PDFromDb).CurrentValues.SetValues(model);
                return db.SaveChanges() > 0;
            }
        }

        public bool ProductDelete(string id)
        {
            using (OPSContext db = new OPSContext())
            {
                var Product = db.Product.Find(id);
                db.Product.Remove(Product);
                return db.SaveChanges() > 0;
            }
        }
    }
}