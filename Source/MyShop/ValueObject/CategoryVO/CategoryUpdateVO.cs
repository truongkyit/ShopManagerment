using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ValueObject.CategoryVO
{
    public class CategoryUpdateVO
    {
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CategoryUpdateVO()
        {
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
