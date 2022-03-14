using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ValueObject.OrderProductVO
{
    public class OrderProductCreateVO
    {
        public int ID { get; set; }
        public int ProductID { get; set; }

        public int OrderID { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public double Amount { get; set; }

        public double TotalAmount { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }

        public OrderProductCreateVO()
        {
            Amount = 0;
            Quantity = 0;
            TotalAmount = 0;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void SetQuantityAndAmount(int quantity,double TotalAmount)
        {
            this.Quantity = quantity;
            this.TotalAmount = TotalAmount;
        }

        public OrderProductCreateVO(OrderProduct ordp)
        {
            this.ID = ordp.ID;
            this.ProductID = ordp.ProductID;
            this.Name = ordp.Name;
            this.Quantity = ordp.Quantity;
            this.Amount = ordp.Amount;
            this.TotalAmount = ordp.TotalAmount;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public OrderProductCreateVO(Product prd)
        {
            this.ProductID = prd.ID;
            this.Name = prd.Name;
            if(prd.Price != null)
            {
                this.Amount = (double)prd.Price;
            }
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public OrderProductCreateVO(OrderProductCreateVO ordp)
        {
            this.ProductID = ordp.ProductID;
            this.Name = ordp.Name;
            this.Quantity = ordp.Quantity;
            this.Amount = ordp.Amount;
            this.TotalAmount = ordp.TotalAmount;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
