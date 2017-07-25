using OPS.Repostiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Services
{
    public class PickingsAsignService
    {
        private readonly PickingAsignRepository _repository = null;

        public PickingAsignRepository Repository
        {
            get { return _repository ?? new PickingAsignRepository(); }
        }

        public void Dispose()
        {
            Repository.Dispose();
        }
    }
}