using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVSAPI.Models
{
    public class Category
    {
		public int CatID { get; set; }
		public string CatName { get; set; }
		public int Parent { get; set; }
		public string CatImage { get; set; }
		public string LR { get; set; }
		public string PgTitle { get; set; }
		public string MetaDesc { get; set; }
		public string MetaKey { get; set; }
		public string tagword { get; set; }
		public string Status { get; set; }
	}
	public class CategoryList
	{
		public List<Category> items { get; set; }
		//public PagingParameterModel parameterModel { get; set; }
		public int TotalCount { get; set; }
		public int pageSize { get; set; }
		public int currentPage { get; set; }
		public string previousPage { get; set; }
		public string nextPage { get; set; }
	}
}