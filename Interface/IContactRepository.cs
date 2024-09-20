using PVSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVSAPI.Interface
{
    public interface IContactRepository
    {
        List<Contact> Get();
        Contact GetSingle(int ContactID);
        Contact LoginContact(string Username, string Password);
        int Insert(Contact contact);
        bool Update(Contact contact);
        bool Delete(int ContactId);
        bool CheckIsExist(string email);
    }
}
