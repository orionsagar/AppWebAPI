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
    public class OrderController : ApiController
    {
        private OrderRepository orderRepository;
        public OrderController()
        {
            orderRepository = new OrderRepository();
        }

        // GET: api/Brand
        [Route("api/Orders")]
        [HttpGet]
        [Authorize]
        public OrderList Get([FromUri] PagingParameterModel pagingParameterModel)
        {
            var source = orderRepository.Get();

            //Search Parameter [With null check]  
            // ------------------------------------ Search Parameter-------------------  
            if (pagingParameterModel.QueryText != null && pagingParameterModel.QuerySearch != null)
            {
                //source = source.Where(x => x.InvNo.Contains(pagingParameterModel.QuerySearch)).ToList();

                for (int i = 0; i < pagingParameterModel.QuerySearch.Length; i++)
                {
                    if (pagingParameterModel.QueryText[i] == "InvNo")
                    {
                        source = source.Where(x => x.InvNo.Contains(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Phone")
                    {
                        source = source.Where(x => x.BPhone1.Contains(pagingParameterModel.QuerySearch[i].ToString())).ToList();
                    }
                }
            }

            if (pagingParameterModel.ParentId != -1)
            {
                source = source.Where(x => x.InvID == Convert.ToInt32(pagingParameterModel.ParentId)).ToList();
            }

            if (!string.IsNullOrEmpty(pagingParameterModel.Sorting))
            {
                if (pagingParameterModel.Sorting.ToString() == "asc")
                {
                    source = source.OrderBy(s => s.InvID).ToList();
                }
                else if (pagingParameterModel.Sorting.ToString() == "desc")
                {
                    source = source.OrderByDescending(s => s.InvID).ToList();
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

            var returnItem = new OrderList
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
        [Route("api/Orders/{id}")]
        [HttpGet]
        [Authorize]
        public List<Order> GetSingle(int id)
        {
            return orderRepository.GetSingle(id);
        }

        // POST: api/Brand
        [Route("api/Orders")]
        [HttpPost]
        [Authorize]
        public int Post(Order order)
        {
            return orderRepository.Insert(order);
        }

        // POST: api/Brand
        [Route("api/Orders/{id}/Cancel")]
        [HttpPost]
        [Authorize]
        public bool Cancel(string id)
        {
            return orderRepository.Cancel(id);
        }
    }
}
