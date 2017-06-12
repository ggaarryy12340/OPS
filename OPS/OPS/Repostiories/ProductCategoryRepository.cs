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
        private readonly OPSContext _db = null;

        public OPSContext db
        {
            get { return _db ?? new OPSContext(); }
        }

        public List<PDCategory> GetPDCategories()
        {
                return db.PDCategory.ToList();             
        }

        public bool CreatePDCategory(PDCategory model)
        {
            bool rs = false;
            try
            {
                db.PDCategory.Add(model);
                rs = db.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw;
            }

            return rs;                            
        }

    }
}