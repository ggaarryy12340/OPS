using OPS.Models.DAL;
using OPS.Models.OPSContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OPS.Repostiories
{
    public class ProductCategoryRepository
    {
        private readonly OPSContext _db = new OPSContext();

        public OPSContext db
        {
            get
            {
                return _db;
            }
        }

        public PDCategory GetSingle(int id)
        {
            return db.PDCategory.Find(id);
        }

        public List<PDCategory> GetPDCategories()
        {
            return db.PDCategory.OrderBy(x => x.PDCategoryId).ToList();
        }

        public bool CreatePDCategory(PDCategory model)
        {
            db.PDCategory.Add(model);
            return db.SaveChanges() > 0;
        }

        public bool Edit(PDCategory model)
        {
            var PDCategoryFromDb = db.PDCategory.Find(model.PDCategoryId);
            db.Entry(PDCategoryFromDb).CurrentValues.SetValues(model);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var PDCategory = db.PDCategory.Find(id);
            db.PDCategory.Remove(PDCategory);
            return db.SaveChanges() > 0;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}