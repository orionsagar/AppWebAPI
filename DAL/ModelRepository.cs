using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PVSAPI.Interface;
using PVSAPI.Models;

namespace PVSAPI.DAL
{
    public class ModelRepository : IModelRepository
    {
        private readonly IDbConnection _db;

        public ModelRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionSecond"].ConnectionString);
        }
        public bool CheckIsExist(string ModelName)
        {
            int rowsAffected = this._db.Execute(@"Select count(0) cnt from Model Where ModelName = @ModelName",
                 new { ModelName = ModelName });

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public bool Delete(int ModelID)
        {
            int rowsAffected = this._db.Execute(@"DELETE FROM [Model] WHERE ModelID = @ModelID",
                new { ModelID = ModelID });

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public List<Model> Get()
        {
            return this._db.Query<Model>("Select [ModelID],[ModelName],[MakeID],[ModelLogo],[IsPart],[Model].[IsActive], Brand.BrandName From [Model] inner join Brand on Model.MakeID = Brand.BrandID").ToList();
        }

        public Model GetSingle(int ModelID)
        {
            return _db.Query<Model>(
               "Select [ModelID],[ModelName],[MakeID],[ModelLogo],[IsPart],[Model].[IsActive], Brand.BrandName From [Model] inner join Brand on Model.MakeID = Brand.BrandID Where ModelID = @ModelID",
               new { ModelID = ModelID }).SingleOrDefault();
        }

        public int Insert(Model model)
        {
            var CrDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

            string sql = @"INSERT [Model] ([ModelName],[MakeID],[IsPart]) " +
                        "values (@ModelName,@MakeID,@IsPart); SELECT CAST(SCOPE_IDENTITY() as int)";


            int returnID = _db.Query<int>(sql, new
            {
                ModelName = model.ModelName,
                MakeID = model.MakeID,
                IsPart = model.IsPart
            }).Single();

            return returnID;
        }

        public bool Update(Model model)
        {
            int rowsAffected = this._db.Execute(
                 "Update [Model] set ModelName = @ModelName, MakeID = @MakeID, IsPart = @IsPart where ModelID = " +
                 model.ModelID, model);

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