using PVSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVSAPI.Interface
{
    public interface IOrderRepository
    {
        List<Order> Get();
        List<Order> GetSingle(int orderID);
        int Insert(Order order);
        bool Update(Order order);

        bool Cancel(string orderId);
        bool Delete(int orderId);
        bool CheckIsExist(string orderid);
    }
}
