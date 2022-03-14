using MyShop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace MyShop.ValueObject.ProductVO
{
    public class ProductCreateVO : INotifyPropertyChanged
    {
        public string name { get; set; }
        public int catId { get; set; }
        public string imagePath { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
        public string SKU { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public Category category { get; set; }

        public ProductCreateVO(Product product)
        {
            this.name = product.Name;
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;

        }

        public ProductCreateVO()
        {
            name = "";
            imagePath = "";
            price = 0;
            quantity = 0;
            SKU = FunctionHelper.generalSKU("product");
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;
        }

        public void PassValue(Product product)
        {
            this.name = product.Name;
            if (product.CatID != null)
                this.catId = (int)product.CatID;
            this.category = product.Category;
            this.imagePath = product.ImagePath;
            if (product.Price != null)
                this.price = (double)product.Price;
            if (product.Quantity != null)
                this.quantity = (int)product.Quantity;
            this.SKU = product.SKU;
            this.createdAt = product.CreatedAt;
            this.updatedAt = product.UpdatedAt;
        }

        public void passImage(string image)
        {
            this.imagePath = image;
        }

        public string validate()
        {
            if(this.name.Length <= 0)
            {
                return "Phải có tên sản phẩm!";
            }
            if(!int.TryParse(this.catId.ToString(), out _) || int.TryParse(this.catId.ToString(), out _) && this.catId <=0)
            {
                return "Loại sản phẩm sai định dạng!";
            }
            if(this.imagePath.Length <= 0)
            {
                return "Phải có hình ảnh sản phẩm!";
            }
            if(!int.TryParse(this.price.ToString(), out _) || int.TryParse(this.price.ToString(), out _) && this.price < 0)
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

        public void clear()
        {
            name = "";
            imagePath = "";
            price = 0;
            quantity = 0;
            SKU = FunctionHelper.generalSKU("product");
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
