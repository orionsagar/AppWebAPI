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
    public class ProductController : ApiController
    {
        private PartsRepository _partsRepository;

        public ProductController()
        {
            _partsRepository = new PartsRepository();
        }


        // GET: api/Brand
        [Route("api/Products")]
        [HttpGet]
        [Authorize]
        public PartList Get([FromUri] PagingParameterModel pagingParameterModel)
        {
            var source = _partsRepository.Get();

            //Search Parameter [With null check]  
            // ------------------------------------ Search Parameter-------------------  


            if (pagingParameterModel.QueryText != null && pagingParameterModel.QuerySearch != null)
            {
                for (int i = 0; i < pagingParameterModel.QueryText.Length; i++)
                {
                    if (pagingParameterModel.QueryText[i] == "Name" && pagingParameterModel.QueryText[i] == "Featured" && pagingParameterModel.QueryText[i] == "MostPopular")
                    {
                        source = source.Where(x => x.ProductName.Contains(pagingParameterModel.QuerySearch[i]) && x.Featured == Convert.ToBoolean(pagingParameterModel.QuerySearch[i]) && x.MostPopular == Convert.ToBoolean(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Name" && pagingParameterModel.QueryText[i] == "MostPopular")
                    {
                        source = source.Where(x => x.ProductName.Contains(pagingParameterModel.QuerySearch[i]) && x.MostPopular == Convert.ToBoolean(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Name" && pagingParameterModel.QueryText[i] == "Featured")
                    {
                        source = source.Where(x => x.ProductName.Contains(pagingParameterModel.QuerySearch[i]) && x.Featured == Convert.ToBoolean(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Featured" && pagingParameterModel.QueryText[i] == "MostPopular")
                    {
                        source = source.Where(x => x.Featured == Convert.ToBoolean(pagingParameterModel.QuerySearch[i]) && x.MostPopular == Convert.ToBoolean(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Name")
                    {
                        source = source.Where(x => x.ProductName.ToLower().Contains(pagingParameterModel.QuerySearch[i].ToString().ToLower())).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Featured")
                    {
                        source = source.Where(x => x.Featured == Convert.ToBoolean(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "MostPopular")
                    {
                        source = source.Where(x => x.MostPopular == Convert.ToBoolean(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Brand")
                    {
                        source = source.Where(x => x.BrandID == Convert.ToInt32(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Model")
                    {
                        source = source.Where(x => x.ModelID.Contains(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Grade")
                    {
                        source = source.Where(x => x.GradeID.Contains(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                }



            }

            if (pagingParameterModel.ParentId != -1)
            {
                source = source.Where(x => x.CatID == Convert.ToInt32(pagingParameterModel.ParentId)).ToList();
            }

            if (!string.IsNullOrEmpty(pagingParameterModel.Sorting))
            {
                if (pagingParameterModel.Sorting.ToString() == "asc")
                {
                    if (pagingParameterModel.MostFeaturedSorting != null)
                    {
                        for (int i = 0; i < pagingParameterModel.MostFeaturedSorting.Length; i++)
                        {
                            if (pagingParameterModel.MostFeaturedSorting[i] == "MostPopular" && pagingParameterModel.MostFeaturedSorting[i] == "Featured")
                            {
                                source = source.OrderBy(s => s.MostPopular && s.Featured).ToList();
                            }
                            else if (pagingParameterModel.MostFeaturedSorting[i] == "MostPopular")
                            {
                                source = source.OrderBy(s => s.MostPopular).ToList();
                            }
                            else if (pagingParameterModel.MostFeaturedSorting[i] == "Featured")
                            {
                                source = source.OrderBy(s => s.Featured).ToList();
                            }
                        }
                    }
                    else
                    {
                        source = source.OrderBy(s => s.ProductID).ToList();
                    }

                }
                else if (pagingParameterModel.Sorting.ToString() == "desc")
                {
                    if (pagingParameterModel.MostFeaturedSorting != null)
                    {
                        for (int i = 0; i < pagingParameterModel.MostFeaturedSorting.Length; i++)
                        {
                            if (pagingParameterModel.MostFeaturedSorting[i] == "MostPopular" && pagingParameterModel.MostFeaturedSorting[i] == "Featured")
                            {
                                source = source.OrderByDescending(s => s.MostPopular && s.Featured).ToList();
                            }
                            else if (pagingParameterModel.MostFeaturedSorting[i] == "MostPopular")
                            {
                                source = source.OrderByDescending(s => s.MostPopular).ToList();
                            }
                            else if (pagingParameterModel.MostFeaturedSorting[i] == "Featured")
                            {
                                source = source.OrderByDescending(s => s.Featured).ToList();
                            }
                        }
                    }
                    else
                    {
                        source = source.OrderByDescending(s => s.ProductID).ToList();
                    }
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

            var returnItem = new PartList
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
        [Route("api/Products/{id}")]
        [HttpGet]
        [Authorize]
        public List<PartsProduct> GetSingle(int id)
        {
            return _partsRepository.GetSingle(id);
        }
    }
}
