using MyShop.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    class OrderDAO
    {
        private MyShopEntities conn;

        public OrderDAO()
        {
            conn = dbConnection.getInstance();
        }

        public IQueryable<Order> getAll()
        {
            var query = conn.Orders.Where(tg => tg.DeletedAt == null);
            return (IQueryable<Order>)query;
        }

        public OrderPagination getAll(int skip, int limit)
        {
            var query = conn.Orders.Where(tg => tg.DeletedAt == null);
            var count = query.Count();
            query = query.OrderByDescending(t => t.CreatedAt).Skip(skip).Take(limit);
            return new OrderPagination(count, query);
        }

        public OrderPagination getAll(int skip, int limit, string code)
        {
            var query = conn.Orders.Where(tg => tg.DeletedAt == null);
            if (code.Length > 0)
            {
                query = query.Where(tg => tg.Code.Contains(code));
            }
            var count = query.Count();
            query = query.OrderByDescending(t => t.CreatedAt).Skip(skip).Take(limit);
            return new OrderPagination(count, query);
        }

        public OrderPagination getAll(int skip, int limit, FilterOrder filter)
        {
            var query = conn.Orders.Where(tg => tg.DeletedAt == null);
            if (filter.text.Length > 0)
            {
                query = query.Where(tg => tg.Code.Contains(filter.text));
            }
            if(filter.ValueStatus != -1)
            {
                query = query.Where(tg => tg.OrderStatu.Value == filter.ValueStatus);
            }
            if(filter.MoneySearch.Length > 0)
            {
                int mn = int.Parse(filter.MoneySearch);
                if (mn > 0)
                {
                    if (filter.BelowMoney == true)
                    {
                        query = query.Where(tg => tg.TotalAmount < mn);
                    }
                    else
                    {
                        query = query.Where(tg => tg.TotalAmount >= mn);
                    }
                }
            }
            var count = query.Count();
            query = query.OrderByDescending(t => t.CreatedAt).Skip(skip).Take(limit);
            return new OrderPagination(count, query);
        }
        public IQueryable<Order> getById(int id)
        {
            var query = conn.Orders.Where(tg => tg.DeletedAt == null && tg.ID == id);
            return (IQueryable<Order>)query;
        }


        public Order insert(Order order)
        {
            var query = conn.Orders.Add(order);
            conn.SaveChanges();
            return query;
        }

        public Order update(Order order)
        {
            conn.SaveChanges();
            return conn.Orders.Where(item => item.ID == order.ID).FirstOrDefault();
        }

        public Order update(int id, Order order)
        {
            var orderUpdate = conn.Orders.Where(item => item.DeletedAt == null && item.ID == id).FirstOrDefault();
            var findOrderStatus = conn.OrderStatus.Where(item => item.Value == order.OrderStatu.Value).FirstOrDefault();
            if(findOrderStatus != null)
            {
                orderUpdate.StatusID = findOrderStatus.ID;
            }
            var findClient = conn.Clients.Where(item => item.PhoneNumber.Equals(order.Client.PhoneNumber)).FirstOrDefault();
            if(findClient != null)
            {
                orderUpdate.ClientID = findClient.ID;
            }
            else
            {
                var client = new Client()
                {
                    Name = order.Client.Name,
                    PhoneNumber = order.Client.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var clientInsert = conn.Clients.Add(client);
                orderUpdate.ClientID = clientInsert.ID;
            }
            orderUpdate.Address = order.Address;
            orderUpdate.UpdatedAt = order.UpdatedAt;
            var listOrdPT = orderUpdate.OrderProducts.ToList();
            for (int i = 0; i < listOrdPT.Count; i++)
            {
                conn.OrderProducts.Remove(listOrdPT[i]);
                var productId = listOrdPT[i].ProductID;
                var product = conn.Products.Where(item => item.ID == productId).FirstOrDefault();
                if (product != null)
                {
                    product.Quantity = product.Quantity + listOrdPT[i].Quantity;
                }
                conn.SaveChanges();

            }
            var listOrdP = order.OrderProducts.ToList();
            for (int i = 0; i < listOrdP.Count; i++)
            {
                conn.OrderProducts.Add(listOrdP[i]);
                listOrdP[i].CreatedAt = DateTime.Now;
                listOrdP[i].UpdatedAt = DateTime.Now;
                listOrdP[i].OrderID = id;
                var productId = listOrdP[i].ProductID;
                var product = conn.Products.Where(item => item.ID == productId).FirstOrDefault();
                if (product != null)
                {
                    product.Quantity = product.Quantity - listOrdP[i].Quantity;
                }
                conn.SaveChanges();

            }
            orderUpdate.TotalAmount = orderUpdate.OrderProducts.Select(item => item.TotalAmount).Sum(); 
            conn.SaveChanges();
            return conn.Orders.Where(item => item.ID == orderUpdate.ID).FirstOrDefault();
        }
        public Order insert(Order order, List<OrderProduct> orderProducts)
        {
            var query = conn.Orders.Add(order);
            conn.SaveChanges();
            for (int i = 0; i < orderProducts.Count; i++)
            {
                orderProducts[i].OrderID = query.ID;
                query.OrderProducts.Add(orderProducts[i]);
                var idOrdP = orderProducts[i].ID;
                var product = conn.Products.Where(item => item.ID == idOrdP).FirstOrDefault();
                if (product != null)
                {
                    product.Quantity = product.Quantity - orderProducts[i].Quantity;
                    conn.SaveChanges();
                }
            }
            conn.SaveChanges();
            return query;
        }

        public bool deleteById(int id)
        {
            var order = conn.Orders.Where(item => item.ID == id).FirstOrDefault();
            order.DeletedAt = DateTime.Now;
            var listOrdP = order.OrderProducts.ToList();
            for (int i = 0; i < order.OrderProducts.Count; i++)
            {
                var idOrdP = listOrdP[i].ID;
                var ordP = conn.OrderProducts.Where(item => item.ID == idOrdP).FirstOrDefault();
                ordP.DeletedAt = DateTime.Now;
                var productId = listOrdP[i].ProductID;
                var product = conn.Products.Where(item => item.ID == productId).FirstOrDefault();
                if (product != null)
                {
                    product.Quantity = product.Quantity + listOrdP[i].Quantity;
                }

                conn.SaveChanges();
            }
            conn.SaveChanges();
            return true;
        }
    }
    public class OrderPagination
    {
        public int count;
        public IQueryable<Order> orders;

        public OrderPagination(int count, IQueryable<Order> orders)
        {
            this.count = count;
            this.orders = orders;
        }
    }

}
