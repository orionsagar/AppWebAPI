using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVSAPI.Models;

namespace PVSAPI.Interface
{
    public interface IBrandRepository
    {
        List<Brand> Get();
        Brand GetSingle(int BrandID);
        int Insert(Brand brand);
        bool Update(Brand brand);
        bool Delete(int brandId);
        bool CheckIsExist(string BrandName);
    }
}
