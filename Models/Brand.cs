using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVSAPI.Models
{
    public class Brand
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public string Description { get; set; }
        public string PgTitle { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }
        public string Tagword { get; set; }
    }

    public class BrandList
    {
        public List<Brand> items { get; set; }
        //public PagingParameterModel parameterModel { get; set; }
        public int TotalCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public string previousPage { get; set; }
        public string nextPage { get; set; }
    }
}