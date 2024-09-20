using PVSAPI.DAL;
using PVSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PVSAPI.Controllers
{
    public class GradesController : ApiController
    {
        private GradesRepository _gradesRespository;

        public GradesController()
        {
            _gradesRespository = new GradesRepository();
        }

        // GET: api/Brand
        [Route("api/Grades")]
        [HttpGet]
        [Authorize]
        public GradesList Get([FromUri] PagingParameterModel pagingParameterModel)
        {
            var source = _gradesRespository.Get();

            //Search Parameter [With null check]  
            // ------------------------------------ Search Parameter-------------------  
            if (pagingParameterModel.QueryText != null && pagingParameterModel.QuerySearch != null )
            {
                //source = source.Where(x => x.GradeName.Contains(pagingParameterModel.QuerySearch)).ToList();

                for (int i = 0; i < pagingParameterModel.QuerySearch.Length; i++)
                {
                    if (pagingParameterModel.QueryText[i] == "Name")
                    {
                        source = source.Where(x => x.GradeName.Contains(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                }
            }

            if (pagingParameterModel.ParentId != -1)
            {
                source = source.Where(x => x.ModelID == Convert.ToInt32(pagingParameterModel.ParentId)).ToList();
            }

            if (!string.IsNullOrEmpty(pagingParameterModel.Sorting))
            {
                if (pagingParameterModel.Sorting.ToString() == "asc")
                {
                    source = source.OrderBy(s => s.GradeID).ToList();
                }
                else if (pagingParameterModel.Sorting.ToString() == "desc")
                {
                    source = source.OrderByDescending(s => s.GradeID).ToList();
                }
            }

            //Get's No of rows count
            int count = source.Count();

            //Parameter is passed from Query string if it is null then it default Value will be pageNumber:1 
            int CurrentPage = pagingParameterModel.pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pagingParameterModel.PageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            source = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            //      QuerySearch = string.IsNullOrEmpty(pagingParameterModel.QuerySearch) ?
            //"No Parameter Passed" : pagingParameterModel.QuerySearch
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage,
            };

            var returnItem = new GradesList
            {
                items = source,
                currentPage = CurrentPage,
                pageSize = PageSize,
                TotalCount = TotalCount,
                previousPage = previousPage,
                nextPage = nextPage
            };
            return returnItem;
        }

        // Get: api/Brand
        [Route("api/Grades/{id}")]
        [HttpGet]
        [Authorize]
        public Grades GetSingle(int id)
        {
            return _gradesRespository.GetSingle(id);
        }


        // POST: api/Brand
        [Route("api/Grades")]
        [HttpPost]
        [Authorize]
        public int Post([FromBody] Grades grades)
        {
            return _gradesRespository.Insert(grades);
        }

        // PUT: api/Brand/5
        [Route("api/Grades")]
        [HttpPut]
        [Authorize]
        public bool Put([FromBody] Grades grades)
        {
            return _gradesRespository.Update(grades);
        }

        // DELETE: api/Brand/5
        [Route("api/Grades/{id}")]
        [HttpDelete]
        [Authorize]
        public bool Delete(int id)
        {
            return _gradesRespository.Delete(id);
        }
    }
}
