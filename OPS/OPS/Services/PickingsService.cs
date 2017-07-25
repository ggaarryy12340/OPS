using OPS.Repostiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Services
{
    public class PickingsService
    {
        private readonly PickingsRepository _repository = null;

        public PickingsRepository Repository
        {
            get { return _repository ?? new PickingsRepository(); }
        }

        public void Dispose()
        {
            Repository.Dispose();
        }
    }
}