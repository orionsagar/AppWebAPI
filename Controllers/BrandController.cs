using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.WebSockets;
using PVSAPI.DAL;
using PVSAPI.Models;

namespace PVSAPI.Controllers
{
    public class BrandController : ApiController
    {
        private BrandRepository _brandRespository;

        public BrandController()
        {
            _brandRespository = new BrandRepository();
        }


        // GET: api/Brand
        [Route("api/Brand")]
        [HttpGet]
        [Authorize]
        public BrandList Get([FromUri] PagingParameterModel pagingParameterModel)
        {
            var source = _brandRespository.Get();

            //Search Parameter [With null check]  
            // ------------------------------------ Search Parameter-------------------  
            if (pagingParameterModel.QueryText != null && pagingParameterModel.QuerySearch != null)
            {
                for (int i = 0; i < pagingParameterModel.QuerySearch.Length; i++)
                {
                    if (pagingParameterModel.QueryText[i] == "Name")
                    {
                        source = source.Where(x => x.BrandName.Contains(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                }
                
            }

            if (pagingParameterModel.ParentId != -1)
            {
                source = source.Where(x => x.BrandID == Convert.ToInt32(pagingParameterModel.ParentId)).ToList();
            }

            if (!string.IsNullOrEmpty(pagingParameterModel.Sorting))
            {
                if (pagingParameterModel.Sorting.ToString() == "asc")
                {
                    source = source.OrderBy(s => s.BrandID).ToList();
                }
                else if (pagingParameterModel.Sorting.ToString() == "desc")
                {
                    source = source.OrderByDescending(s => s.BrandID).ToList();
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

            var returnItem = new BrandList
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
        [Route("api/Brand/{id}")]
        [HttpGet]
        [Authorize]
        public Brand GetSingle(int id)
        {
            return _brandRespository.GetSingle(id);
        }


        // POST: api/Brand
        [Route("api/Brand")]
        [HttpPost]
        [Authorize]
        public int Post([FromBody] Brand brand)
        {
            return _brandRespository.Insert(brand);
        }

        // PUT: api/Brand/5
        [Route("api/Brand")]
        [HttpPut]
        [Authorize]
        public bool Put([FromBody] Brand brand)
        {
            return _brandRespository.Update(brand);
        }

        // DELETE: api/Brand/5
        [Route("api/Brand/{id}")]
        [HttpDelete]
        [Authorize]
        public bool Delete(int id)
        {
            return _brandRespository.Delete(id);
        }
    }
}
