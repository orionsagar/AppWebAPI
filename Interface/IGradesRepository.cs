using PVSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVSAPI.Interface
{
    public interface IGradesRepository
    {
        List<Grades> Get();
        Grades GetSingle(int GradeID);
        int Insert(Grades grades);
        bool Update(Grades grades);
        bool Delete(int GradeID);
        bool CheckIsExist(string GradeName);
    }
}
