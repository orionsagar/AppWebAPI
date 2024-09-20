using System.Collections.Generic;
using PVSAPI.Models;

namespace PVSAPI.DAL
{
    internal interface IUserAccount
    {
        List<UserAccount> GetUserAccounts(string userName, string password);
        UserAccount GetSingleUserAccount(int userId);
        bool UpdateUserAccount(UserAccount ourUserAccount);
    }
}