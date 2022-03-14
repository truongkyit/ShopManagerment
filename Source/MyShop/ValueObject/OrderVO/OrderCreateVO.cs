using MyShop.Common;
using MyShop.ValueObject.ClientVO;
using MyShop.ValueObject.OrderProductVO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ValueObject.OrderVO
{
    public class OrderCreateVO : INotifyPropertyChanged
    {
        public string Code { get; set; }

        public int Id { get; set; }
        public double TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int Status { get; set; }
        public ClientCreateVO client { get; set; }

        public BindingList<OrderProductCreateVO> orderProductVOs = new BindingList<OrderProductCreateVO>();

        public string Address { get; set; }

        public OrderCreateVO()
        {
            Code = FunctionHelper.RandomString(8);
            TotalAmount = 0;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            client = new ClientCreateVO();
            Status = (int)OrderStatus.New;
            Address = "";
            client.Name = "";
            client.PhoneNumber = "";
        }

        public void PassValue(Order order)
        {
            this.Id = order.ID;
            this.CreatedAt = order.CreatedAt;
            this.Code = order.Code;
            this.TotalAmount = order.TotalAmount;
            orderProductVOs.Clear();
            var listOrder = order.OrderProducts.ToList();
            for (int i = 0; i < order.OrderProducts.Count(); i++)
            {
                orderProductVOs.Add(new OrderProductCreateVO(listOrder[i]));
            }
            this.Status = order.OrderStatu.Value;
            this.Address = order.Address;
            this.client.Name = order.Client.Name;
            this.client.PhoneNumber = order.Client.PhoneNumber;
        }

        public void reset()
        {
            CreatedAt = DateTime.Now;
            Code = FunctionHelper.RandomString(8);
            TotalAmount = 0;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            orderProductVOs.Clear();
            Address = "";
            client.Name = "";
            client.PhoneNumber = "";
        }

        public void PassValue(OrderCreateVO order)
        {
            this.Id = order.Id;
            this.CreatedAt = order.CreatedAt;
            this.Code = order.Code;
            this.TotalAmount = order.TotalAmount;
            orderProductVOs.Clear();
            var listOrder = order.orderProductVOs.ToList();
            for (int i = 0; i < order.orderProductVOs.Count(); i++)
            {
                orderProductVOs.Add(new OrderProductCreateVO(listOrder[i]));
            }
            this.Status = order.Status;
            this.client.Name = order.client.Name;
            this.client.PhoneNumber = order.client.PhoneNumber;
            this.Address = order.Address;
        }

        public OrderUpdateVO toOrderUpdateVO()
        {
            var ordU = new OrderUpdateVO()
            {
                TotalAmount = this.TotalAmount,
                UpdatedAt = this.UpdatedAt,
                Address = this.Address,
                client = new ClientUpdateVO(this.client),
                Status = this.Status
            };

            for (int i = 0; i < this.orderProductVOs.Count(); i++)
            {
                var orderProduct = new OrderProductUpdateVO()
                {
                    ID = this.orderProductVOs[i].ID,
                    Amount = this.orderProductVOs[i].Amount,
                    ProductID = this.orderProductVOs[i].ProductID,
                    Name = this.orderProductVOs[i].Name,
                    TotalAmount = this.orderProductVOs[i].TotalAmount,
                    Quantity = this.orderProductVOs[i].Quantity
                };

                ordU.orderProductVOs.Add(orderProduct);
            }

            return ordU;
        }

        public string Validate()
        {
            if(client.Name.Length <= 0)
            {
                return "Phải bắt buộc có tên";
            }
            if(client.PhoneNumber.Length <= 0)
            {
                return "Số điện thoại không đúng định dạng";
            }
            if(Address.Length <= 0)
            {
                return "Địa chỉ là bắt buộc";
            }
            if(orderProductVOs.Count() <= 0)
            {
                return "Đơn hàng phải có ít nhất một sản phẩm";
            }
            return "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
