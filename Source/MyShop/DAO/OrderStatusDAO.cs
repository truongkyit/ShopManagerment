using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    public class OrderStatusDAO
    {
        private MyShopEntities conn;

        public OrderStatusDAO()
        {
            conn = dbConnection.getInstance();
        }

        public IQueryable<OrderStatu> getAll()
        {
            var query = conn.OrderStatus.Where(tg => tg.DeletedAt == null);
            return (IQueryable<OrderStatu>)query;
        }

        public OrderStatu getByValue(int Value)
        {
            var query = conn.OrderStatus.Where(item => item.Value == Value).FirstOrDefault();
            return query;
        }
    }
}
