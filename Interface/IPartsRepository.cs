using PVSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVSAPI.Interface
{
    public interface IPartsRepository
    {
        List<PartsProduct> Get();
        List<PartsProduct> GetSingle(int ProductID);
    }
}
