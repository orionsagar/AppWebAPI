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
    public class UserAccountController : ApiController
    {
        private UserAccountRepository _accountRespository;

        public UserAccountController()
        {
            _accountRespository = new UserAccountRepository();
        }

        // GET: api/UserAccount
        [Route("api/Account/{userName}/{password}")]
        [HttpGet]
        public List<UserAccount> Get(string userName, string password)
        {
            return _accountRespository.GetUserAccounts(userName, password);
        }

        // GET: api/UserAccount/5
        [Route("api/Account/{id}")]
        [HttpGet]
        public UserAccount Get(int id)
        {
            return _accountRespository.GetSingleUserAccount(id);
        }

        // POST: api/UserAccount
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT: api/UserAccount/5
        [Route("api/Account")]
        [HttpPut]
        public bool Put([FromBody]UserAccount value)
        {
            return _accountRespository.UpdateUserAccount(value);
        }

        // DELETE: api/UserAccount/5
        //public void Delete(int id)
        //{
        //}
    }
}
