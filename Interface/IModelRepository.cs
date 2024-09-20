using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVSAPI.Models;

namespace PVSAPI.Interface
{
    public interface IModelRepository
    {
        List<Model> Get();
        Model GetSingle(int ModelID);
        int Insert(Model model);
        bool Update(Model model);
        bool Delete(int ModelID);
        bool CheckIsExist(string ModelName);
    }
}
