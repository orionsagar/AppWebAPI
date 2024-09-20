using Dapper;
using PVSAPI.Interface;
using PVSAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PVSAPI.DAL
{
    public class ContactRepository : IContactRepository
    {
        private readonly IDbConnection _db;

        public ContactRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionSecond"].ConnectionString);
        }
        public List<Contact> Get()
        {
            return this._db.Query<Contact>("Select [ContactID],[ContactName],[Phone1],[Phone2],[Phone3],[UserName],[Password],[Email],[Address],[City],[Country],[CStatus],[Rating],[Gender],[LName],[District],[ContactType],[ContactDate] From [Contact]").ToList();
        }

        public Contact GetSingle(int ContactID)
        {
            return _db.Query<Contact>(
                "Select [ContactID],[ContactName],[Phone1],[Phone2],[Phone3],[UserName],[Password],[Email],[Address],[City],[Country],[CStatus],[Rating],[Gender],[LName],[District],[ContactType],[ContactDate] From [Contact] Where ContactID = @ContactID",
                new { ContactID = ContactID }).SingleOrDefault();
        }

        public int Insert(Contact contact)
        {
            var CrDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

            string sql = @"INSERT [Contact] ([ContactName],[Phone1],[Phone2],[UserName],[Password],[Email],[Address],[City],[Country],[CStatus],[Rating],[Gender],[LName],[District],[ContactType],[ContactDate]) " +
                        "values (@ContactName,@Phone1,@Phone2,@UserName,@Password,@Email,@Address,@City,@Country,@CStatus,@Rating,@Gender,@LName,@District,@ContactType,@ContactDate); SELECT CAST(SCOPE_IDENTITY() as int)";


            int returnID = _db.Query<int>(sql, new
            {
                ContactName = contact.ContactName,
                Phone1 = contact.Phone1,
                Phone2 = contact.Phone2,
                UserName = contact.UserName,
                Password = contact.Password,
                Email = contact.Email,
                Address = contact.Address,
                City = contact.City,
                Country = contact.Country,
                CStatus = contact.CStatus,
                Rating = contact.Rating,
                Gender = contact.Gender,
                LName = contact.LName,
                District = contact.District,
                ContactType = contact.ContactType,
                ContactDate = CrDate
            }).Single();

            return returnID;
        }

        public bool Update(Contact contact)
        {
            int rowsAffected = this._db.Execute(
                "Update [Contact] set [ContactName] = @ContactName,[Phone1] = @Phone1,[Phone2] = @Phone2,[UserName] = @UserName,[Password] = @Password,[Email] = @Email,[Address] = @Address,[City] = @City,[Country] = @Country,[CStatus] = @CStatus,[Rating] = @Rating,[Gender] = @Gender,[LName] = @LName,[District] = @District,[ContactType] = @ContactType,[ContactDate] = @ContactDate where ContactID = " +
                contact.ContactID, contact);

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public bool CheckIsExist(string email)
        {
            int rowsAffected = this._db.Execute(@"Select count(0) cnt from Contact Where Email = @Email",
                new { Email = email });

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public bool Delete(int ContactId)
        {
            int rowsAffected = this._db.Execute(@"DELETE FROM [Contact] WHERE [ContactID] = @ContactId",
               new { ContactId = ContactId });

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public Contact LoginContact(string Username, string Password)
        {
            return _db.Query<Contact>(
                "Select [ContactID],[ContactName],[Phone1],[Phone2],[Phone3],[UserName],[Password],[Email],[Address],[City],[Country],[CStatus],[Rating],[Gender],[LName],[District],[ContactType],[ContactDate] From [Contact] Where [Contact].[UserName] = @UserName or [Contact].[Phone1] like CONCAT('%',@UserName,'%') and [Contact].[Password] = @Password",
                new { userName = Username, password = Password }).SingleOrDefault();
        }
    }
}