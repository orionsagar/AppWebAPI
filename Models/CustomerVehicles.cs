using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVSAPI.Models
{
    public class CustomerVehicles
    {
		public int CVID { get; set; }
		public int CustomerID { get; set; }
		public int BrandID { get; set; }
		public int ModelID { get; set; }
		public int GradeID { get; set; }
		public bool IsSelected { get; set; }
		public DateTime CreateDate { get; set; }
	}

    public class CustomerVehiclesList
    {
        public List<CustomerVehicles> items { get; set; }
        //public PagingParameterModel parameterModel { get; set; }
        public int TotalCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public string previousPage { get; set; }
        public string nextPage { get; set; }
    }
}