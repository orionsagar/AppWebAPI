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
    public class ContactController : ApiController
    {
        private ContactRepository _contactRepository;

        public ContactController()
        {
            _contactRepository = new ContactRepository();
        }



        // GET: api/Brand
        [Route("api/Contact")]
        [HttpGet]
        [Authorize]
        public ContactList Get([FromUri] PagingParameterModel pagingParameterModel)
        {
            var source = _contactRepository.Get();

            //Search Parameter [With null check]  
            // ------------------------------------ Search Parameter-------------------  
            if (pagingParameterModel.QueryText != null && pagingParameterModel.QuerySearch != null)
            {
                //source = source.Where(x => x.Email == pagingParameterModel.QuerySearch).ToList();
                for (int i = 0; i < pagingParameterModel.QuerySearch.Length; i++)
                {
                    if (pagingParameterModel.QueryText[i] == "Email")
                    {
                        source = source.Where(x => x.Email.Contains(pagingParameterModel.QuerySearch[i])).ToList();
                    }
                    else if (pagingParameterModel.QueryText[i] == "Mobile")
                    {
                        source = source.Where(x => x.Phone1 == pagingParameterModel.QuerySearch[i]).ToList();
                    }
                }
            }

            if (!string.IsNullOrEmpty(pagingParameterModel.Sorting))
            {
                if (pagingParameterModel.Sorting.ToString() == "asc")
                {
                    source = source.OrderBy(s => s.ContactID).ToList();
                }
                else if (pagingParameterModel.Sorting.ToString() == "desc")
                {
                    source = source.OrderByDescending(s => s.ContactID).ToList();
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

            var returnItem = new ContactList
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
        [Route("api/Contact/{id}")]
        [HttpGet]
        [Authorize]
        public Contact GetSingle(int id)
        {
            return _contactRepository.GetSingle(id);
        }


        // POST: api/Brand
        [Route("api/Contact")]
        [HttpPost]
        [Authorize]
        public int Post([FromBody] Contact contact)
        {
            return _contactRepository.Insert(contact);
        }

        // PUT: api/Brand/5
        [Route("api/Contact")]
        [HttpPut]
        [Authorize]
        public bool Put([FromBody] Contact contact)
        {
            return _contactRepository.Update(contact);
        }

        // DELETE: api/Brand/5
        [Route("api/Contact/{id}")]
        [HttpDelete]
        [Authorize]
        public bool Delete(int id)
        {
            return _contactRepository.Delete(id);
        }


        // Get: api/Contact/{Username}/{Password}
        [Route("api/Contact/{Username}/{Password}")]
        [HttpGet]
        [Authorize]
        public Contact LoginContact(string Username, string Password)
        {
            return _contactRepository.LoginContact(Username, Password);
        }
    }
}


