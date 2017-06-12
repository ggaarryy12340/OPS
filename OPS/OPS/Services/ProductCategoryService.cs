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
    }
}