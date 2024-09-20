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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnection _db;

        public CategoryRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionSecond"].ConnectionString);
        }

        public List<Category> Get()
        {
            return this._db.Query<Category>("Select [CatID],[CatName],[Parent],[CatImage],[LR],[PgTitle],[MetaDesc],[MetaKey],[tagword],[Status] From [Category]").ToList();
        }

        public Category GetSingle(int CategoryID)
        {
            return _db.Query<Category>(
               "Select [CatID],[CatName],[Parent],[CatImage],[LR],[PgTitle],[MetaDesc],[MetaKey],[tagword],[Status] From [Category] Where CatID = @CatID",
               new { CatID = CategoryID }).SingleOrDefault();
        }
    }
}