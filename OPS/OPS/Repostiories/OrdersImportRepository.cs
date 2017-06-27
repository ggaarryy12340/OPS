using OPS.Models.DAL;
using OPS.Models.OPSContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Repostiories
{
    public class OrdersImportRepository
    {
        private readonly OPSContext _db = new OPSContext();

        public OPSContext db
        {
            get
            {
                return _db;
            }
        }

        public Product GetPD(string id)
        {
            return db.Product.Find(id); 
        }

        public bool SaveOrder(Order OD)
        {
            db.Order.Add(OD);
            return db.SaveChanges() > 0;
        }


        public void Dispose()
        {
            db.Dispose();
        }
    }
}