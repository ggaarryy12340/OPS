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
        public PDCategory GetSingle(int id)
        {
            using (OPSContext db = new OPSContext())
            {
                return db.PDCategory.Find(id);
            }
        }

        public List<PDCategory> GetPDCategories()
        {
            using (OPSContext db = new OPSContext())
            {
                return db.PDCategory.OrderBy(x => x.PDCategoryId).ToList();  
            }                          
        }

        public bool CreatePDCategory(PDCategory model)
        {
            using (OPSContext db = new OPSContext())
            {
                db.PDCategory.Add(model);
                return db.SaveChanges() > 0;
            }                       
        }

        public bool Edit(PDCategory model)
        {
            using (OPSContext db = new OPSContext())
            {
                var PDCategoryFromDb = db.PDCategory.Find(model.PDCategoryId);
                db.Entry(PDCategoryFromDb).CurrentValues.SetValues(model);
                return db.SaveChanges() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (OPSContext db = new OPSContext())
            {
                var PDCategory = db.PDCategory.Find(id);
                db.PDCategory.Remove(PDCategory);
                return db.SaveChanges() > 0;
            }
        }

    }
}