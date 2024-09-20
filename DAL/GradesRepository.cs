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
    public class GradesRepository : IGradesRepository
    {
        private readonly IDbConnection _db;

        public GradesRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionSecond"].ConnectionString);
        }

        public bool CheckIsExist(string GradeName)
        {
            int rowsAffected = this._db.Execute(@"Select count(0) cnt from Grades Where GradeName = @GradeName",
                new { GradeName = GradeName });

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public bool Delete(int GradeID)
        {
            int rowsAffected = this._db.Execute(@"DELETE FROM [Grades] WHERE GradeID = @GradeID",
                new { GradeID = GradeID });

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public List<Grades> Get()
        {
            return this._db.Query<Grades>("Select [GradeID],[Grades].[ModelID],[GradeName], m.ModelName,[CreateDate] From [Grades] inner join Model m on m.ModelID = Grades.ModelID").ToList();
        }

        public Grades GetSingle(int GradeID)
        {
            return _db.Query<Grades>(
               "Select [GradeID],[Grades].[ModelID],[GradeName], m.ModelName,[CreateDate] From [Grades] inner join Model m on m.ModelID = Grades.ModelID Where Grades.GradeID = @GradeID",
               new { GradeID = GradeID }).SingleOrDefault();
        }

        public int Insert(Grades grades)
        {
            var CrDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

            string sql = @"INSERT [Grades] ([ModelID],[GradeName],[CreateDate]) " +
                        "values (@ModelID,@GradeName,@CreateDate); SELECT CAST(SCOPE_IDENTITY() as int)";


            int returnID = _db.Query<int>(sql, new
            {
                ModelID = grades.ModelID,
                GradeName = grades.GradeName,
                CreateDate = grades.CreateDate
            }).Single();

            return returnID;
        }

        public bool Update(Grades grades)
        {
            int rowsAffected = this._db.Execute(
                "Update [Grades] set ModelID = @ModelID, GradeName = @GradeName, CreateDate = @CreateDate where GradeID = " +
                grades.GradeID, grades);

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