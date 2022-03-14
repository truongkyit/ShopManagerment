using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ValueObject.CategoryVO
{
    public class CategoryCreateVO
    {
        public string Name { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CategoryCreateVO() {
            this.createdAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        public string validate()
        {
            if (this.Name.Length <= 0)
                return "Tên sản phẩm là bắt buộc!";
            return "";
        }
    }
}
