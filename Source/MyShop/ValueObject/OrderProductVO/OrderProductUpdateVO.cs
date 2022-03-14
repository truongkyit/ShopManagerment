using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ValueObject.OrderProductVO
{
    public class OrderProductUpdateVO
    {
        public int ID { get; set; }
        public int ProductID { get; set; }

        public int OrderID { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public double Amount { get; set; }

        public double TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public OrderProductUpdateVO()
        {
        }

        public OrderProductUpdateVO(OrderProductCreateVO ordp)
        {
            this.ID = ordp.ID;
            this.ProductID = ordp.ProductID;
            this.Name = ordp.Name;
            this.Quantity = ordp.Quantity;
            this.Amount = ordp.Amount;
            this.TotalAmount = ordp.TotalAmount;
        }
    }
}
