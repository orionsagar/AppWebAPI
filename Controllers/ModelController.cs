using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PVSAPI.DAL;
using PVSAPI.Models;

namespace PVSAPI.Controllers
{
    public class ModelController : ApiController
    {
        private ModelRepository _modelRespository;

        public ModelController()
        {
            _modelRespository = new ModelRepository();
        }

        // GET: api/Model
        [Route("api/Model")]
        [HttpGet]
        [Authorize]
        public ModelList Get([FromUri] PagingParameterModel pagingParameterModel)
        {
            var source = _modelRespository.Get();

            //Search Parameter [With null check]  
            // ------------------------------------ Search Parameter-------------------  
            if (pagingParameterModel.QueryText != null && pagingParameterModel.QuerySearch != null)
            {
                // source = source.Where(x => x.ModelName.Contains(pagingParameterModel.QuerySearch) || x.BrandName.Contains(pagingParameterModel.QuerySearch)).ToList();

                for (int i = 0; i < pagingParameterModel.QuerySearch.Length; i++)
                {
                    if (pagingParameterModel.QueryText[i] == "Name")
                    {
                        source = source.Where(x => x.ModelName.Contains(pagingParameterModel.QuerySearch[i]) || x.BrandName.Contains(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Status")
                    {
                        source = source.Where(x => x.IsActive == pagingParameterModel.QuerySearch[i].ToString()).ToList();
                    }
                }
            }

            if (pagingParameterModel.ParentId != -1)
            {
                source = source.Where(x => x.MakeID == Convert.ToInt32(pagingParameterModel.ParentId)).ToList();
            }

            if (!string.IsNullOrEmpty(pagingParameterModel.Sorting))
            {
                if (pagingParameterModel.Sorting.ToString() == "asc")
                {
                    source = source.OrderBy(s => s.ModelID).ToList();
                }
                else if (pagingParameterModel.Sorting.ToString() == "desc")
                {
                    source = source.OrderByDescending(s => s.ModelID).ToList();
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

            var returnItem = new ModelList
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
        [Route("api/Model/{id}")]
        [HttpGet]
        [Authorize]
        public Model GetSingle(int id)
        {
            return _modelRespository.GetSingle(id);
        }


        // POST: api/Model
        [Route("api/Model")]
        [HttpPost]
        [Authorize]
        public int Post([FromBody] Model model)
        {
            return _modelRespository.Insert(model);
        }

        // PUT: api/Model/5
        [Route("api/Model")]
        [HttpPut]
        [Authorize]
        public bool Put([FromBody] Model model)
        {
            return _modelRespository.Update(model);
        }

        // DELETE: api/Model/5
        [Route("api/Model/{id}")]
        [HttpDelete]
        [Authorize]
        public bool Delete(int id)
        {
            return _modelRespository.Delete(id);
        }
    }
}
