using OPS.Models.OPSContext;
using OPS.Repostiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Services
{
    public class ProductCategoryService
    {
        private readonly ProductCategoryRepository _repository = null;

        public ProductCategoryRepository Repository
        {
            get { return _repository ?? new ProductCategoryRepository(); }
        }

        public List<PDCategory> GetPDCategories()
        {
            return Repository.GetPDCategories();
        }

        public bool CreatePDCategory(PDCategory model)
        {
            return Repository.CreatePDCategory(model);
        }

        public PDCategory GetSingle(int id)
        {
            return Repository.GetSingle(id);
        }

        public bool Edit(PDCategory model)
        {
            return Repository.Edit(model);
        }

        public bool Delete(int id)
        {
            return Repository.Delete(id);
        }

        public void Dispose()
        {
            Repository.Dispose();
        }
    }
}