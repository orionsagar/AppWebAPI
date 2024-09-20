using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVSAPI.Models
{
    public class Model
    {
        public int ModelID { get; set; }
        public string ModelName { get; set; }
        public string ModelLogo { get; set; }
        public int MakeID { get; set; }
        public bool IsPart { get; set; }
        public string IsActive { get; set; }
        public string BrandName { get; set; }
    }

    public class ModelList
    {
        public List<Model> items { get; set; }
        //public PagingParameterModel parameterModel { get; set; }
        public int TotalCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public string previousPage { get; set; }
        public string nextPage { get; set; }
    }
}