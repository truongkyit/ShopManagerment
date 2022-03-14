using MyShop.Business.Interface;
using MyShop.DAO;
using MyShop.State;
using MyShop.ValueObject.OrderProductVO;
using MyShop.ValueObject.OrderVO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Business
{
    public class OrderBusiness : IOrderBusiness
    {
        private OrderDAO _orderDAO;
        private ProductDAO _productDAO;
        private ClientDAO _clientDAO;
        private ClientBusiness _clientBus;
        private OrderStatusDAO _orderStatusDAO;

        public OrderBusiness()
        {
            _orderDAO = new OrderDAO();
            _productDAO = new ProductDAO();
            _clientDAO = new ClientDAO();
            _clientBus = new ClientBusiness();
            _orderStatusDAO = new OrderStatusDAO();
        }

        public bool deleteById(int id)
        {
            return _orderDAO.deleteById(id);
        }

        public BindingList<Order> getAll()
        {
            var t = _orderDAO.getAll();
            return new BindingList<Order>(t.ToList());
        }

        public OrderPaginationBus getAll(int skip, int limit)
        {
            var t = _orderDAO.getAll(skip, limit);
            return new OrderPaginationBus(t.count, t.orders.ToList());
        }

        public OrderPaginationBus getAll(int skip, int limit, string code)
        {
            var t = _orderDAO.getAll(skip, limit, code);
            return new OrderPaginationBus(t.count, t.orders.ToList());
        }

        public OrderPaginationBus getAll(int skip, int limit, FilterOrder filter)
        {
            filter.text = filter.text.ToUpper();
            var t = _orderDAO.getAll(skip, limit, filter);
            return new OrderPaginationBus(t.count, t.orders.ToList());
        }

        public Order InsertData(OrderCreateVO data, List<OrderProductCreateVO> products)
        {
            throw new NotImplementedException();
        }

        public Order InsertData(OrderCreateVO data)
        {
            var order = new Order()
            {
                Code = data.Code,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                TotalAmount = data.TotalAmount,
                Address = data.Address,
            };

            var orderInsert = _orderDAO.insert(order);

            var checkClient = _clientDAO.findByPhoneNumber(data.client.PhoneNumber);
            if(checkClient != null)
            {
                order.ClientID = checkClient.ID;
            }
            else
            {
                var client = _clientBus.insertData(data.client);
                order.ClientID = client.ID;
            }

            var checkOrderStatus = _orderStatusDAO.getByValue(data.Status);
            if(checkOrderStatus != null)
            {
                order.StatusID = checkOrderStatus.ID;
            }

            for (int i = 0; i < data.orderProductVOs.Count(); i++)
            {
                var orderProduct = new OrderProduct()
                {
                    Amount = data.orderProductVOs[i].Amount,
                    ProductID = data.orderProductVOs[i].ProductID,
                    Name = data.orderProductVOs[i].Name,
                    TotalAmount = data.orderProductVOs[i].TotalAmount,
                    CreatedAt = data.orderProductVOs[i].CreatedAt,
                    UpdatedAt = data.orderProductVOs[i].UpdatedAt,
                    Quantity = data.orderProductVOs[i].Quantity,
                    OrderID = orderInsert.ID
                };

                _productDAO.updateQuantityById(orderProduct.ProductID, orderProduct.Quantity);
                orderInsert.OrderProducts.Add(orderProduct);
            }

            var orderUpdate = _orderDAO.update(orderInsert);
            return orderUpdate;
        }

        public Order update(int id, OrderUpdateVO data)
        {
            return _orderDAO.update(id, data.toOrder());
        }

        public Order update(OrderCreateVO data)
        {
            throw new NotImplementedException();
        }
    }

    public class OrderPaginationBus
    {
        public int count;
        public List<Order> orders;

        public OrderPaginationBus(int count, List<Order> orders)
        {
            this.count = count;
            this.orders = orders;
        }
    }
}
