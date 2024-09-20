using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace PVSAPI.Models
{
    public class PagingParameterModel
    {
        const int maxPageSize = 20;
        public int pageNumber { get; set; } = 1;
        public int _pageSize { get; set; } = 100;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public string[] QueryText
        {
            get;
            set;
        }
        public string[] QuerySearch { get; set; }
        public string Sorting { get; set; } = "";

        public string[] MostFeaturedSorting { get; set; }

        public int ParentId { get; set; } = -1;
    }
}