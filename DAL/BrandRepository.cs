using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using PVSAPI.Interface;
using PVSAPI.Models;

namespace PVSAPI.DAL
{
    public class BrandRepository : IBrandRepository
    {
        private readonly IDbConnection _db;
        public BrandRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionSecond"].ConnectionString);
        }
        public bool CheckIsExist(string BrandName)
        {
            int rowsAffected = this._db.Execute(@"Select count(0) cnt from Brand Where BrandName = @BrandName",
                new { BrandName = BrandName });

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public bool Delete(int brandId)
        {
            int rowsAffected = this._db.Execute(@"DELETE FROM [Brand] WHERE BrandID = @BrandID",
                new { brandId = brandId });

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public List<Brand> Get()
        {
            return this._db.Query<Brand>("Select [BrandID],[BrandName],[BrandLogo],[Description],[PgTitle],[MetaDesc],[MetaKey],[tagword] From [Brand]").ToList();
        }

        public Brand GetSingle(int BrandID)
        {
            return _db.Query<Brand>(
               "Select [BrandID],[BrandName],[BrandLogo],[Description],[PgTitle],[MetaDesc],[MetaKey],[tagword] From [Brand] Where BrandID = @BrandID",
               new { BrandID = BrandID }).SingleOrDefault();
        }

        public int Insert(Brand brand)
        {
            var CrDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

            string sql = @"INSERT [Brand] ([BrandName],[BrandLogo],[Description],[PgTitle],[MetaDesc],[MetaKey],[tagword]) " +
                        "values (@BrandName,@BrandLogo,@Description,@PgTitle,@MetaDesc,@MetaKey,@tagword); SELECT CAST(SCOPE_IDENTITY() as int)";


            int returnID = _db.Query<int>(sql, new
            {
                BrandName = brand.BrandName,
                BrandLogo = brand.BrandLogo,
                Description = brand.Description,
                PgTitle = brand.PgTitle,
                MetaDesc = brand.MetaDesc,
                MetaKey = brand.MetaKey,
                Tagword = brand.Tagword
            }).Single();

            return returnID;
        }

        public bool Update(Brand brand)
        {
            int rowsAffected = this._db.Execute(
                "Update [Brand] set BrandName = @BrandName, BrandLogo = @BrandLogo, Description = @Description, PgTitle = @PgTitle, MetaDesc = @MetaDesc, MetaDesc = @MetaDesc, MetaKey = @MetaKey, tagword = @tagword where BrandID = " +
                brand.BrandID, brand);

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