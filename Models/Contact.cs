using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVSAPI.Models
{
    public class Contact
    {
		public int ContactID { get; set; }
		public string ContactName { get; set; }
		public string Phone1 { get; set; }
		public string Phone2 { get; set; }
		public string Phone3 { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string CStatus { get; set; }
		public int Rating { get; set; }
		public string Gender { get; set; }
		public string LName { get; set; }
		public string District { get; set; }
		public string ContactType { get; set; }
		public DateTime ContactDate { get; set; }
	}

	public class ContactList
	{
		public List<Contact> items { get; set; }
		//public PagingParameterModel parameterModel { get; set; }
		public int TotalCount { get; set; }
		public int pageSize { get; set; }
		public int currentPage { get; set; }
		public string previousPage { get; set; }
		public string nextPage { get; set; }
	}
}