using OPS.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Repostiories
{
    public class PickingsRepository
    {
        private readonly OPSContext _db = new OPSContext();

        public OPSContext db
        {
            get
            {
                return _db;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}