using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVSAPI.Models
{
    public class Grades
    {
        public int GradeID { get; set; }
        public int ModelID { get; set; }
        public string GradeName { get; set; }
        public string ModelName { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class GradesList
    {
        public List<Grades> items { get; set; }
        //public PagingParameterModel parameterModel { get; set; }
        public int TotalCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public string previousPage { get; set; }
        public string nextPage { get; set; }
    }
}