using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ValueObject.ProductVO
{
    public class ProductUpdateVO
    {
        public string name { get; set; }
        public int catId { get; set; }
        public string imagePath { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
        public string SKU { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public ProductUpdateVO()
        {
            this.updatedAt = DateTime.Now;
        }

        public string validate()
        {
            if (this.name.Length <= 0)
            {
                return "Phải có tên sản phẩm!";
            }
            if (!int.TryParse(this.catId.ToString(), out _) || int.TryParse(this.catId.ToString(), out _) && this.catId <= 0)
            {
                return "Loại sản phẩm sai định dạng!";
            }
            if (this.imagePath.Length <= 0)
            {
                return "Phải có hình ảnh sản phẩm!";
            }
            if (!int.TryParse(this.price.ToString(), out _) || int.TryParse(this.price.ToString(), out _) && this.price < 0)
            {
                return "Giá sản phẩm sai định dạng";
            }
            if (!int.TryParse(this.quantity.ToString(), out _) || int.TryParse(this.quantity.ToString(), out _) && this.quantity < 0)
            {
                return "Số lượng sản phẩm sai định dạng!";
            }
            if (this.SKU.Length <= 0)
            {
                return "SKU sai định dạng!";
            }

            return "";
        }
    }
}
