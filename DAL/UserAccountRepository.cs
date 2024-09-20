using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using PVSAPI.Models;

namespace PVSAPI.DAL
{
    public class UserAccountRepository : IDisposable
    {
        private readonly IDbConnection _db;

        public UserAccountRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        public List<UserAccount> GetUserAccounts(string userName, string password)
        {
            return _db.Query<UserAccount>(
                "Select [UserID],[UserName],[Password],[UserRole],[BranchID],[Name],[Email], ur.RoleName from [User] inner join UserRole ur on ur.RoleID = [User].UserRole Where [User].[UserName] = @UserName and [User].[Password] = @Password",
                new {userName = userName, password = password}).ToList();
        }

        public UserAccount GetSingleUserAccount(int userId)
        {
            return _db.Query<UserAccount>(
                "Select [UserID],[UserName],[Password],[UserRole],[BranchID],[Name],[Email], ur.RoleName from [User] inner join UserRole ur on ur.RoleID = [User].UserRole Where [User].[UserID] = @UserID",
                new { userId = userId }).SingleOrDefault();
        }

        public bool UpdateUserAccount(UserAccount ourUserAccount)
        {
            int rowsAffected = this._db.Execute(
                "UPDATE [User] SET [UserName] = @UserName, [Email] = @Email WHERE UserID = " +
                ourUserAccount.UserID, ourUserAccount);

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}