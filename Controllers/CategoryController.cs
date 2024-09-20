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
    public class CategoryController : ApiController
    {
        private CategoryRepository _categoryRepository;

        public CategoryController()
        {
            _categoryRepository = new CategoryRepository();
        }

        // GET: api/Brand
        [Route("api/Category")]
        [HttpGet]
        [Authorize]
        public CategoryList Get([FromUri] PagingParameterModel pagingParameterModel)
        {
            var source = _categoryRepository.Get();

            //Search Parameter [With null check]  
            // ------------------------------------ Search Parameter-------------------  
            if (pagingParameterModel.QueryText != null && pagingParameterModel.QuerySearch != null)
            {
                for (int i = 0; i < pagingParameterModel.QuerySearch.Length; i++)
                {
                    if (pagingParameterModel.QueryText[i] == "Name")
                    {
                        source = source.Where(x => x.CatName.Contains(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Status")
                    {
                        source = source.Where(x => x.Status == "Active").ToList();
                    }
                }
            }

            if (pagingParameterModel.ParentId != -1)
            {
                source = source.Where(x => x.Parent == Convert.ToInt32(pagingParameterModel.ParentId)).ToList();
            }

            if (!string.IsNullOrEmpty(pagingParameterModel.Sorting))
            {
                if (pagingParameterModel.Sorting.ToString() == "asc")
                {
                    source = source.OrderBy(s => s.CatID).ToList();
                }
                else if (pagingParameterModel.Sorting.ToString() == "desc")
                {
                    source = source.OrderByDescending(s => s.CatID).ToList();
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
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage,
            };
            //      QuerySearch = string.IsNullOrEmpty(pagingParameterModel.QuerySearch) ?
            //"No Parameter Passed" : pagingParameterModel.QuerySearch

            var returnItem = new CategoryList
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
        [Route("api/Category/{id}")]
        [HttpGet]
        [Authorize]
        public Category GetSingle(int id)
        {
            return _categoryRepository.GetSingle(id);
        }

    }
}
